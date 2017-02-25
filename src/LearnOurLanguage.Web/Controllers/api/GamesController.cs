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
        private IHangmanService HangmanService { get; }
        private IFlashcardsService FlashcardsService {get; }

        public GamesController(IGamesService gamesService, IQuizService quizService, IHangmanService hangmanService, IFlashcardsService flashcardsService)
        {
            GamesService = gamesService;
            QuizService = quizService;
            HangmanService = hangmanService;
            FlashcardsService = flashcardsService;
        }

        [HttpPost("InitializeQuizGame")]
        public ActionResult InitializeQuizGame([FromBody] QuizParameters param)
        {
            AccessGuardian(Roles.AccessUser, param.UserId);

            return JsonHelper.Success(QuizService.InitializeQuestions(param));
        }

        [HttpPost("InitializeHangmanGame")]
        public ActionResult InitializeHangmanGame([FromBody] HangmanParameters param)
        {
            AccessGuardian(Roles.AccessUser, param.UserId);

            return JsonHelper.Success(HangmanService.InitializeQuestions(param));
        }

        [HttpPost("InitializeFlashcardsGame")]
        public ActionResult InitializeFlashcardsGame([FromBody] FlashcardsParameters param)
        {
            AccessGuardian(Roles.AccessUser, param.UserId);

            return JsonHelper.Success(FlashcardsService.InitializeQuestions(param));
        }

        [HttpPost("InsertAnswers")]
        public ActionResult InsertAnswers([FromBody] IList<AnswerUpdateModel> answers)
        {
            AccessGuardian(Roles.AccessUser);

            return JsonHelper.Response(GamesService.InsertAnswers(answers));
        }

        [HttpPost("FinishGameSession/{gameSessionId}")]
        public ActionResult FinishGameSession(int gameSessionId)
        {
            AccessGuardian(Roles.AccessUser);
            GamesService.FinishGameSession(gameSessionId);

            return JsonHelper.Success("zakończono sesje");
        }
    }
}
