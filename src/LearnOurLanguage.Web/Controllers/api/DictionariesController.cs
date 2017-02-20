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
    public class DictionariesController : BaseController
    {
        private IDictionariesRepository DictionariesRepository { get; }
        public DictionariesController(IDictionariesRepository repo)
        {
            DictionariesRepository = repo;
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
            AccessGuardian(new AccessRole(Roles.AccessUser, DictionariesRepository.GetById(id).UserId));

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

        [HttpPost]
        public ActionResult Post([FromBody] Dictionary input)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser));

            return JsonHelper.Response(
                DictionariesRepository.Insert(input) && DictionariesRepository.Save()
            );
        }

        [HttpPut]
        public ActionResult Put([FromBody] Dictionary data)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, data.UserId));

            if (!data.IsValid) return JsonHelper.Error(ExceptionConst.OwnerAccess);
            return JsonHelper.Response(
                DictionariesRepository.Update(data) && DictionariesRepository.Save()
            );
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            AccessGuardian(new AccessRole(Roles.AccessUser, DictionariesRepository.GetById(id).UserId));

            return JsonHelper.Response(
                DictionariesRepository.Delete(id) && DictionariesRepository.Save()
            );
        }
    }
}
