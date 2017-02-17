﻿using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Auth;
using Model.Const;
using Model.Core;

namespace LearnOurLanguage.Web.Base
{
    /// <summary>
    /// Bazowy kontroler świadczący obsługę kontroli dostępu
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// Aktualny context Http / Sesja
        /// </summary>
        protected ISession Session => Context.Session; // HttpContext.Session;

        /// <summary>
        /// Inizjalizacja logera do obsługi komunikatów
        /// </summary>
        protected ILogger<BaseController> Logger => App.LoggerFactory.CreateLogger<BaseController>();

        /// <summary>
        /// Metoda sprawdzająca dostęp do metody kontrolera
        /// </summary>
        /// <param name="accessLevel">Minimalny, wymagalny poziom dostępu</param>
        protected void AccessGuardian(int accessLevel)
        {
            var currentAccessLevel = GetCurrentUserProperties(Session);
            var accepted = currentAccessLevel >= accessLevel;

            if (accepted) return;

            if (currentAccessLevel == -1)
            {
                Context.Response.StatusCode = 401;
                Context.Response.Body = Stream.Null;
            }
            else if (currentAccessLevel < accessLevel)
            {
                Context.Response.StatusCode = 403;
                Context.Response.Body = Stream.Null;
            }

            if (!accepted)
                throw new AuthenticationException(ExceptionConst.AccessDenied);
        }

        /// <summary>
        /// Metoda sprawdzająca dostęp do metody kontrolera
        /// </summary>
        /// <param name="accessLevel">Minimalny, wymagalny poziom dostępu</param>
        /// <param name="userId">Id użytkownika, który ma prawo do edytowanego obiektu</param>
        protected void AccessGuardian(int accessLevel, int userId)
        {
            AccessGuardian(accessLevel);

            if (GetCurrentUserId(Session) != userId)
            {
                Context.Response.StatusCode = 403;
                Context.Response.Body = Stream.Null;

                Logger.LogWarning($"{LoggingConst.AccessGuardian}: {ExceptionConst.AccessDenied}");
                throw new AuthenticationException(ExceptionConst.AccessDenied);
            }
        }

        /// <summary>
        /// Metoda sprawdzająca dostęp do metody kontrolera
        /// </summary>
        /// <param name="roles">Tablica ról opisujący, kto ma dostęp do danej metody kontrolera</param>
        protected void AccessGuardian(params AccessRole[] roles)
        {
            var accepted = false;
            var currentAccessLevel = GetCurrentUserProperties(Session);
            var currentUser = GetCurrentUserId(Session);

            foreach (var role in roles)
                if (role.Asert(currentAccessLevel, currentUser))
                    accepted = true;
            int status = 0;

            if (!accepted && (currentUser == null))
            {
                status = Context.Response.StatusCode = 401;
                Context.Response.Body = Stream.Null;

                Logger.LogWarning($"{LoggingConst.AccessGuardian}: {ExceptionConst.Unauthorized}");
                throw new HttpException(HttpStatusCode.Unauthorized, ExceptionConst.Unauthorized);
            }

            if (!accepted && (currentUser > -1))
            {
                status = Context.Response.StatusCode = 403;
                Context.Response.Body = Stream.Null;

                Logger.LogWarning($"{LoggingConst.AccessGuardian}: {ExceptionConst.Forbidden}");
                throw new HttpException(HttpStatusCode.Forbidden, ExceptionConst.Forbidden);
            }

            if (!accepted)
            {
                Logger.LogWarning($"{LoggingConst.AccessGuardian}: ({status}) {ExceptionConst.AccessDenied}");
                //throw new HttpException(status, ExceptionConst.AccessDenied);
                //HttpContext.Response.Com();
                //throw new AuthenticationException(ExceptionConst.AccessDenied);
            }
        }

        private static int GetCurrentUserProperties(ISession session)
        {
            return session.GetInt32(ParametersConst.AccessTokenName) ?? -1;
        }

        private static int? GetCurrentUserId(ISession session)
        {
            return session.GetInt32(ParametersConst.UserIdToken);
        }
    }
}