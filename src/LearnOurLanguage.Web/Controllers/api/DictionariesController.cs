using System.Linq;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Model.Auth;
using Model.Const;
using Model.Models.Database;
using Model.Models.Dictionaries;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{

    [Route("api/[controller]")]
    public class DictionariesController : BaseController
    {
        private IDictionariesRepository DictionariesRepository { get; }
        private IDictionariesService DictionariesService { get; }

        public DictionariesController(IDictionariesRepository repo, IDictionariesService dictionariesService)
        {
            DictionariesRepository = repo;
            DictionariesService = dictionariesService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(DictionariesRepository.GetAll().ToList());
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var dictionary = DictionariesRepository.GetById(id);
            if (dictionary.IsPublic)
            {
                AccessGuardian(Roles.AccessUser);
            }
            else
            {
                AccessGuardian(Roles.AccessUser, dictionary.UserId);
            }
            return JsonHelper.Success(DictionariesRepository.GetById(id));
        }

        [HttpGet("GetAllPublic")]
        public ActionResult GetAllPublic()
        {
            AccessGuardian(Roles.AccessUser);

            return JsonHelper.Success(DictionariesRepository.GetAllPublic());
        }

        [HttpGet("GetForUser")]
        public ActionResult GetForUser(int userId)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, userId));

            return JsonHelper.Success(DictionariesRepository.GetForUser(userId));
        }

        [HttpGet("CopyDictionary")]
        public ActionResult CopyDictionary(int dictionaryId, int userId)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(DictionariesService.CopyDictionary(dictionaryId, userId));
        }

        [HttpGet("UpdateDictionary")]
        public ActionResult UpdateDictionary(int dictionaryId)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(DictionariesService.UpdateDictionary(dictionaryId));
        }

        [HttpPost]
        public ActionResult Post([FromBody] DictionaryDTO input)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Success(DictionariesService.InsertOrUpdate(input));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, DictionariesRepository.GetById(id).UserId));

            return JsonHelper.Response(DictionariesService.Delete(id));
        }
    }
}
