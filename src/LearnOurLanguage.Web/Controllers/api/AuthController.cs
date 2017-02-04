﻿using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Const;
using Model.Models.Account;
using Model.Services.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{
    /// <summary>
    /// Kontroler przeznaczony do autoryzacji użytkownika i kończenia sesji
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private IAuthService AuthService { get; set; }

        /// <summary>
        /// Konstruktor wstrzykujący AuthService
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        /// <summary>
        /// Metoda służaca do autoryzacji użytkownika.
        /// </summary>
        /// <param name="user">Jako parametr przyje obiekt zawierający email i hasło</param>
        /// <returns>Poziom dostępu użytkownika</returns>
        [HttpPost]
        public ActionResult Login([FromBody] AppUserAuthVo user)
        {
            var accessLevel = AuthService.CheckAuthorization(user);
            HttpContext.Session.SetInt32(ParametersConst.AccessTokenName, accessLevel.AccessLevel);
            HttpContext.Session.SetInt32(ParametersConst.UserIdToken, accessLevel.UserId);
            return JsonHelper.Success(new
            {
                accessLevel = accessLevel.AccessLevel,
                userId = accessLevel.UserId
            });
        }

        /// <summary>
        /// Metoda wylogowująca użytkownika
        /// </summary>
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.SetInt32(ParametersConst.AccessTokenName, -1);
            HttpContext.Session.SetInt32(ParametersConst.UserIdToken, -1);
            HttpContext.Session.Remove(ParametersConst.AccessTokenName);
            HttpContext.Session.Remove(ParametersConst.UserIdToken);

            return JsonHelper.Success(string.Empty);
        }

        /// <summary>
        /// Metoda zwracająca aktualne poświadczenia na serwerze
        /// </summary>
        [HttpGet("GetAccess")]
        public ActionResult GetAccess()
        {
            var accessLevel = HttpContext.Session.GetInt32(ParametersConst.AccessTokenName) ?? -1;
            var userId = HttpContext.Session.GetInt32(ParametersConst.UserIdToken) ?? -1;
            return JsonHelper.Success(new { accessLevel, userId });
        }
    }
}
