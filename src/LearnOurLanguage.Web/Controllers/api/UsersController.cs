using System;
using System.Linq;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Model.Auth;
using Model.Const;
using Model.Helpers;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{
    /// <summary>
    /// Kontroler do obsługi użytkowników
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private IUsersRepository UsersRepository { get; set; }

        /// <summary>
        /// Wstrzykiwanie zależności przez konstruktor
        /// </summary>
        /// <param name="repo">Repozytorium użytkowników</param>
        public UsersController(IUsersRepository repo)
        {
            UsersRepository = repo;
        }

        /// <summary>
        /// Pobieranie wszystkich użytkowników
        /// </summary>
        /// <returns>Lista użytkowników</returns>
        [HttpGet]
        public ActionResult Get()
        {
            AccessGuardian(new AccessRole(Roles.AccessEveryone));

            return JsonHelper.Success(UsersRepository.GetAll().ToList());
        }

        /// <summary>
        /// Pobieranie jednego użytkownika
        /// </summary>
        /// <param name="id">Identyfikator użytkownika</param>
        /// <returns>Pełne informacje o użytkowniku</returns>
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessEveryone));

            return JsonHelper.Success(UsersRepository.GetById(id));
        }
        
        /// <summary>
        /// Dodawanie użytkownika
        /// </summary>
        /// <param name="input">Klasa z pełnymi informacjami o użytkowniku</param>
        [HttpPost]
        public ActionResult Post([FromBody] User input)
        {
            AccessGuardian(new AccessRole(Roles.AccessEveryone));

            var salt = HashHelper.GenerateSalt();
            var data = input;
            data.Role = null;
            data.RoleId = input.RoleId > 0 ? input.RoleId : Roles.IdUser;
            data.Salt = salt;
            data.Password = HashHelper.ComputeHash(input.Password.Trim(), salt);
            data.Date = DateTime.Now;

            if (!data.IsValid) return JsonHelper.Error(ExceptionConst.WrongData);
            return JsonHelper.Response(
                UsersRepository.Insert(data) && UsersRepository.Save()
            );
        }
        
        /// <summary>
        /// Aktualizacja użytkownika
        /// </summary>
        /// <param name="data">Klasa z pełnymi informacjami o użytkowniku</param>
        [HttpPut]
        public ActionResult Put([FromBody] User data)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, data.Id));

            UsersRepository.PrepareData(data);
            if (!data.IsValid) return JsonHelper.Error(ExceptionConst.OwnerAccess);
            return JsonHelper.Response(
                UsersRepository.Update(data) && UsersRepository.Save()
            );
        }

        /// <summary>
        /// Usuwanie użytkoniwka
        /// </summary>
        /// <param name="id">Identyfikator użytkownika</param>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, id));

            return JsonHelper.Response(
                UsersRepository.Delete(id) && UsersRepository.Save()
            );
        }
    }
}
