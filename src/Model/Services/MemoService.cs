using System;
using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Model.Models.Database;
using Model.Models.Games;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class MemoService : IMemoService
    {
        private DatabaseContext Context { get; set; }
        private IGamesService GamesService { get; }

        private ITranslationsRepository TranslationsRepository { get; set; }

        public MemoService(DatabaseContext db, IGamesService gamesService, ITranslationsRepository translationsRepository)
        {
            Context = db;
            GamesService = gamesService;
            TranslationsRepository = translationsRepository;
        }

        public IList<MemoModel> InitializeQuestions(MemoParameters param)
        {
            var gameSession = GamesService.InitializeBaseGame(param.DictionaryId, param.UserId, GamesEnum.Memo);
            var translations = TranslationsRepository.GetTranslationsForDictionary(param.DictionaryId).ToList();
            return GetQuestions(translations, gameSession, param);
        }

        private IList<MemoModel> GetQuestions(IList<Translation> translations, GameSession gameSession, MemoParameters param)
        {
            var result = new List<MemoModel>();
            var min = Math.Min(param.MaxNumberOfQuestions, translations.Count);
            for (var i = 0; i < param.NumberOfGames; i++)
            {
                result.Add(new MemoModel
                {
                    GameSessionId = gameSession.GameId,
                    Translations = translations.OrderBy(translation => Guid.NewGuid()).Take(min).ToList()
                });
            }

            return result;
        }
    }
}
