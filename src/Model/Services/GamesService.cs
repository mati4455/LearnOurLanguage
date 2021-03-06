﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Core;
using Model.Models;
using Model.Models.Database;
using Model.Models.Games;
using Model.Repositories.Interfaces;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class GamesService : IGamesService
    {
        private DatabaseContext Context { get; }
        private ITranslationsRepository TranslationsRepository { get; }
        private IGameSessionsRepository GameSessionsRepository { get; }
        private IGameSessionTranslationsRepository GameSessionTranslationsRepository { get; }
        private ILogger<GamesService> Logger => App.LoggerFactory.CreateLogger<GamesService>();

        public GamesService(
            DatabaseContext db,
            ITranslationsRepository translationsRepo,
            IGamesRepository gamesRepo,
            IGameSessionsRepository gameSessionsRepo,
            IGameSessionTranslationsRepository gameSessionTranslationsRepo)
        {
            Context = db;
            TranslationsRepository = translationsRepo;
            GameSessionsRepository = gameSessionsRepo;
            GameSessionTranslationsRepository = gameSessionTranslationsRepo;
        }

        public IList<Translation> GetDictionaryTranslations(int dictionaryId)
        {
            return TranslationsRepository.GetTranslationsForDictionary(dictionaryId).ToList();
        }

        public IList<Translation> GetTranslationsById(IEnumerable<int> ids, bool reverse) {
            var idsList = ids.ToList();
            var data = TranslationsRepository.GetAll().Where(x => idsList.Contains(x.Id));
            return reverse
                ? ReverseTranslations(data).ToList()
                : data.ToList();
        }

        public GameSession InitializeBaseGame(int dictionaryId, int userId, GamesEnum gameId)
        {
            var gameSession = new GameSession
            {
                DictionaryId = dictionaryId,
                UserId = userId,
                GameId = (int)gameId,
                DateStart = DateTime.Now
            };
            GameSessionsRepository.Insert(gameSession);
            GameSessionsRepository.Save();

            return gameSession;
        }

        public IList<QuestionPair> InitializeGame(int dictionaryId, int userId, GamesEnum gameId, int count)
        {
            var questions = GetQuestionsIds(dictionaryId, userId, gameId, count).ToList();
            var gameSession = InitializeBaseGame(dictionaryId, userId, gameId);
            var result = questions.Select(x => new QuestionPair
            {
                GameSessionId = gameSession.Id,
                TranslationId = x
            })
            .ToList();

            return result; 
        }

        public bool InsertAnswers(IList<AnswerUpdateModel> answers)
        {
            var gst = new List<GameSessionTranslation>();
            answers.ToList().ForEach(abs =>
            {
                gst.Add(new GameSessionTranslation
                {
                    GameSessionId = abs.GameSessionId,
                    TranslationId = abs.TranslationId,
                    Correct = abs.Correct,
                    Duration = abs.Duration
                });
            });

            return GameSessionTranslationsRepository.Insert(gst) &&
                   GameSessionTranslationsRepository.Save();
        }

        public void FinishGameSession(int gameSessionId)
        {
            var gs = new GameSession {Id = gameSessionId, DateEnd = DateTime.Now};
            Context.GameSessions.Attach(gs);
            Context.Entry(gs).Property(x => x.DateEnd).IsModified = true;
            Context.SaveChanges();
        }

        /// <summary>
        /// Metoda wybierająca pytania dla wybranego słownika, użytkownika oraz gry
        /// Sposób wyboru:
        /// - wszystkie odpowiedzi dla danego słownika, użytkownika oraz gry
        /// - pobranie słowek ze słownika, dla których nie ma odpowiedzi
        /// - pogrupowanie po pytaniu (słówku)
        /// - wyliczenie liczby odpowiedzi na dane pytanie
        /// - wyliczenie stosunku poprawnych odpowiedzi do błędnych odpowiedzi
        /// - sortowanie po liczbe odpowiedzi (od najmniejszej)
        /// - następne sortowanie po ratio (od największego - najgorszy wynik)
        /// - ostatnie "sortowanie" to rozrzucenie pytań, aby zmienić ich kolejność na losową
        /// - wybór min(count, liczbaPytanWSlowniku) pierwszych pytań z listy
        /// - zwrócenie listy idków pytań
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="gameId">Id gry</param>
        /// <param name="count">Max liczba pytań</param>
        /// <returns>List idków słówek do pytania</returns>
        private IList<int> GetQuestionsIds(int dictionaryId, int userId, GamesEnum gameId, int count)
        {
            var answeredQuestions = Context.GameSessionTranslations
                .Where(x => x.GameSession.GameId == (int) gameId)
                .Where(x => x.GameSession.UserId == userId)
                .Where(x => x.GameSession.DictionaryId == dictionaryId)
                .Select(x => new {x.TranslationId, x.Correct})
                .ToList();

            var ids = answeredQuestions.Select(x => x.TranslationId).ToList();
            var notAnsweredQuestions = Context.Translations
                .Where(x => x.DictionaryId == dictionaryId && !ids.Contains(x.Id))
                .Select(x => new {TranslationId = x.Id, Correct = false})
                .ToList();
            answeredQuestions.AddRange(notAnsweredQuestions);

            var result = answeredQuestions
                .GroupBy(x => x.TranslationId, (k, v) => new
                {
                    TranslationId = k,
                    Answers = v.Count(),
                    // zabezpieczenie przed dzieleniem przez 0
                    Ratio = (decimal) (v.Count(y => y.Correct) + 1) / (v.Count(y => !y.Correct) + 1)
                })
                .OrderBy(x => x.Answers)
                .ThenByDescending(x => x.Ratio)
                .ThenBy(x => Guid.NewGuid())
                .ToList();

            var maxElements = Math.Min(count, result.Count);

            return result
                .Take(maxElements)
                .Select(x => x.TranslationId)
                .ToList();
        }

        public IList<Translation> ReverseTranslations(IEnumerable<Translation> translations) {
            return translations.ToList().Select(x => new Translation {
                Id = x.Id,
                DictionaryId = x.DictionaryId,
                Dictionary = x.Dictionary,
                FirstLangWord = x.SecondLangWord,
                SecondLangWord = x.FirstLangWord
            }).ToList();
        }
    }
}