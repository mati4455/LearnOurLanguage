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
    public class TranslationsController : BaseController
    {
        private ITranslationsRepository TranslationsRepository { get; }
        public TranslationsController(ITranslationsRepository repo)
        {
            TranslationsRepository = repo;
        }

        [HttpGet]
        public ActionResult Get()
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(TranslationsRepository.GetAll().ToList());
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(TranslationsRepository.GetById(id));  
        }
        
        [HttpGet("GetForDictionary")]
        public ActionResult GetForDictionary(int dictionaryId)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(TranslationsRepository.GetTranslationsForDictionary(dictionaryId));
        }

        [HttpPost]
        public ActionResult Post([FromBody] Translation input)
        {
            AccessGuardian(new AccessRole(Roles.AccessEveryone));

            return JsonHelper.Response(
                TranslationsRepository.Insert(input) && TranslationsRepository.Save()
            );
        }

        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Translation data)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, data.Id));

            if (!data.IsValid) return JsonHelper.Error(ExceptionConst.OwnerAccess);
            return JsonHelper.Response(
                TranslationsRepository.Update(data) && TranslationsRepository.Save()
            );
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Response(
                TranslationsRepository.Delete(id) && TranslationsRepository.Save()
            );
        }
    }
}
