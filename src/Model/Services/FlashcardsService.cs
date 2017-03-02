using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Model.Models.Database;
using Model.Models.Games;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class FlashcardsService : IFlashcardsService
    {
        private DatabaseContext Context { get; set; }
        private IGamesService GamesService { get; set; }

        public FlashcardsService(DatabaseContext db, IGamesService gamesService)
        {
            Context = db;
            GamesService = gamesService;
        }

        public IList<FlashcardsModel> InitializeQuestions(FlashcardsParameters param)
        {
            var questionsIds = GamesService.InitializeGame(param.DictionaryId, param.UserId, GamesEnum.Flashcards, param.MaxNumberOfQuestions);
            var ids = questionsIds.Select(x => x.TranslationId).ToList();
            var translations = GamesService.GetTranslationsById(ids, param.ReverseLangs);

            return GetQuestions(translations, questionsIds);
        }

        private IList<FlashcardsModel> GetQuestions(IList<Translation> translations, IList<QuestionPair> questionsIds)
        {
            var questions = questionsIds.Select(q => new FlashcardsModel
            {
                GameSessionId = q.GameSessionId,
                Translation = translations.Single(x => x.Id == q.TranslationId)
            })
            .ToList();

            return questions;
        }
    }
}
