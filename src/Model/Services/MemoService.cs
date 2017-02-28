using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Model.Models.Database;
using Model.Models.Games;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class MemoService : IMemoService
    {
        private DatabaseContext Context { get; set; }
        private IGamesService GamesService { get; }

        public MemoService(DatabaseContext db, IGamesService gamesService)
        {
            Context = db;
            GamesService = gamesService;
        }

        public IList<MemoModel> InitializeQuestions(MemoParameters param)
        {
            var questionsIds = GamesService.InitializeGame(param.DictionaryId, param.UserId, GamesEnum.Memo, param.MaxNumberOfQuestions);
            var ids = questionsIds.Select(x => x.TranslationId).ToList();
            var translations = GamesService.GetTranslationsById(ids);

            return GetQuestions(translations, questionsIds);
        }

        private IList<MemoModel> GetQuestions(IList<Translation> translations, IList<QuestionPair> questionsIds)
        {
            var questions = questionsIds.Select(q => new MemoModel()
            {
                GameSessionId = q.GameSessionId,
                Translation = translations.Single(x => x.Id == q.TranslationId)
            })
            .ToList();

            return questions;
        }
    }
}
