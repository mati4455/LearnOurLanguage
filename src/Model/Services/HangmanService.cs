using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Model.Models.Database;
using Model.Models.Games;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class HangmanService : IHangmanService
    {
        private DatabaseContext Context { get; set; }
        private IGamesService GamesService { get; set; }

        public HangmanService(DatabaseContext db, IGamesService gamesService)
        {
            Context = db;
            GamesService = gamesService;
        }

        public IList<HangmanModel> InitializeQuestions(HangmanParameters param)
        {
            var questionsIds = GamesService.InitializeGame(param.DictionaryId, param.UserId, GamesEnum.Hangman, param.MaxNumberOfQuestions);
            var ids = questionsIds.Select(x => x.TranslationId).ToList();
            var translations = GamesService.GetTranslationsById(ids, param.ReverseLangs);

            return GetQuestions(translations, questionsIds);
        }

        private IList<HangmanModel> GetQuestions(IList<Translation> translations, IList<QuestionPair> questionsIds)
        {
            var questions = questionsIds.Select(q => new HangmanModel
            {
                GameSessionId = q.GameSessionId,
                Translation = translations.Single(x => x.Id == q.TranslationId)
            })
            .ToList();

            return questions;
        }
    }
}
