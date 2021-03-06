﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model.Models;
using Model.Models.Database;
using Model.Models.Games;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class QuizService : IQuizService
    {
        private DatabaseContext Context { get; set; }
        private IGamesService GamesService { get; set; }

        public QuizService(DatabaseContext db, IGamesService gamesService)
        {
            Context = db;
            GamesService = gamesService;
        }

        public IList<QuizModel> InitializeQuestions(QuizParameters param)
        {
            var translations = GamesService.GetDictionaryTranslations(param.DictionaryId);
            if (param.ReverseLangs) {
                translations = GamesService.ReverseTranslations(translations).ToList();
            }
            var questionsIds = GamesService.InitializeGame(param.DictionaryId, param.UserId, GamesEnum.Quiz, param.MaxNumberOfQuestions);

            return GetQuestions(param.MaxNumberOfAnswers, translations, questionsIds);
        }

        private IList<QuizModel> GetQuestions(int maxNumberOfAnswers, IList<Translation> translations, IList<QuestionPair> questionsIds)
        {
            var maxAns = Math.Min(translations.Count - 1, maxNumberOfAnswers - 1);

            var questions = questionsIds.Select(q => new QuizModel
            {
                GameSessionId = q.GameSessionId,
                Translation = translations.Single(x => x.Id == q.TranslationId),
                Answers = translations
                    .Where(x => x.Id != q.TranslationId)
                    .Select(x => x.SecondLangWord)
                    .Take(maxAns)
                    .ToList()
            })
            .ToList();

            questions.ForEach(q => q.Answers.Add(q.Translation.SecondLangWord));

            return questions;
        }
    }
}
