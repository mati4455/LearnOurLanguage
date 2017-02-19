using System.Linq;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Model.Auth;
using Model.Const;
using Model.Models.Database;
using Model.Repositories.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{
    [Route("api/[controller]")]
    public class LanguagesController : BaseController
    {
        private ILanguagesRepository LanguagesRepository { get; }
        public LanguagesController(ILanguagesRepository repo)
        {
            LanguagesRepository = repo;
        }

        [HttpGet]
        public ActionResult Get()
        {
            AccessGuardian(new AccessRole(Roles.AccessEveryone));

            return JsonHelper.Success(LanguagesRepository.GetSortedLanguages().ToList());
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessEveryone));

            return JsonHelper.Success(LanguagesRepository.GetById(id));
        }

        [HttpPost]
        public ActionResult Post([FromBody] Language input)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Response(
                LanguagesRepository.Insert(input) && LanguagesRepository.Save()
            );
        }

        [HttpPut]
        public ActionResult Put([FromBody] Language data)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            if (!data.IsValid) return JsonHelper.Error(ExceptionConst.OwnerAccess);
            return JsonHelper.Response(
                LanguagesRepository.Update(data) && LanguagesRepository.Save()
            );
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Response(
                LanguagesRepository.Delete(id) && LanguagesRepository.Save()
            );
        }

    }
}
