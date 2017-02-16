using System.Collections.Generic;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Model.Const;
using Model.Models.Games;
using Model.Services.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{
    [Route("api/[controller]")]
    public class GamesController : BaseController
    {
        private IGamesService GamesService { get; }
        private IQuizService QuizService { get; }

        public GamesController(IGamesService gamesService, IQuizService quizService)
        {
            GamesService = gamesService;
            QuizService = quizService;
        }

        [HttpPost("InitializeQuizGame")]
        public ActionResult InitializeQuizGame([FromBody] QuizParameters param)
        {
            AccessGuardian(Roles.AccessUser, param.UserId);

            return JsonHelper.Success(QuizService.InitializeQuestions(param));
        }

        [HttpPost("InsertAnswers")]
        public ActionResult InsertAnswers([FromBody] IList<AnswerUpdateModel> answers)
        {
            AccessGuardian(Roles.AccessUser);

            return JsonHelper.Response(GamesService.InsertAnswers(answers));
        }

        [HttpPut("FinishGameSession/{gameSessionId}")]
        public ActionResult FinishGameSession(int gameSessionId)
        {
            AccessGuardian(Roles.AccessUser);
            GamesService.FinishGameSession(gameSessionId);

            return JsonHelper.Success("zakończono sesje");
        }
    }
}
