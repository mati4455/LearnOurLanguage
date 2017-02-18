using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Model.Models.Database;
using Model.Models.Statistics;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class StatisticsService : IStatisticsService
    {
        #region Opis serwisu
        /*
         * Serwis do obsługi statystyk w aplikacji
         *
         * Jako słówko, którego się uczymy traktujemy SecondLangWord (ono będzie najważniejsze w statystyce)
         *
         */
        #endregion

        #region Context, repozytoria i konstruktor

        private DatabaseContext Context { get; }

        public StatisticsService(DatabaseContext db)
        {
            Context = db;
        }

        #endregion

        #region Pobieranie danych statystycznych

        /// <summary>
        /// Statystyki popularności dostępnych gier.
        /// Statystyka dla użytkownika i ewwentualnie dla konkretnego języka.
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="langId">Id języka (null - dla wszystkich języków)</param>
        /// <returns>Lista gier wraz z liczbą sesji dla danej gry</returns>
        public IList<GameStatistic> GetGamesStatisticsForUser(int userId, int? langId)
        {
            var data = Context.GameSessions
                .Include(gs => gs.Game)
                .Where(gs => gs.UserId == userId)
                .Where(gs => langId == null || langId == gs.Dictionary.SecondLanguageId)
                .ToList();
            var groups = data.GroupBy(x => x.GameId, (key, list) =>
                {
                    var dataList = list as IList<GameSession> ?? list.ToList();
                    var firstEl = dataList.First();
                    return new GameStatistic
                    {
                        GameName = firstEl.Game.Name,
                        NumberOfGames = dataList.Count
                    };
                })
                .OrderByDescending(x => x.NumberOfGames)
                .ToList();
            return groups;
        }

        /// <summary>
        /// Statystyki postępów udzielanaych odpowiedzi dla podanego zakresu.
        /// Statystyka dla użytkownika i ewentualnie dla konkretnego języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="startDate">Data początkowa</param>
        /// <param name="endDate">Data końcowa</param>
        /// <param name="langId">Id języka (null - dla wszystkich języków)</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Lista statystyk na konkretny dzień z wybranego zakresu</returns>
        public IList<Statistics> GetStatisticsForUserPeriod(int userId, DateTime startDate, DateTime endDate, int? langId, int? gameId)
        {
            var data = GetPeriodData(userId, startDate, endDate, langId, gameId).ToList();
            var groups = data.GroupBy(gst => gst.GameSession.DateStart.Date, (key, list) =>
                {
                    var dataList = list as IList<GameSessionTranslation> ?? list.ToList();
                    var firstEl = dataList.First();
                    return new Statistics
                    {
                        Date = firstEl.GameSession.DateStart.Date,
                        Game = firstEl.GameSession.Game,
                        CorrectAnswers = dataList.Count(d => d.Correct),
                        WrongAnswers = dataList.Count(d => !d.Correct),
                        AverageTime = Math.Round(dataList.Average(d => d.Duration), 1)
                    };
                })
                .OrderBy(x => x.Date)
                .ToList();
            return groups;
        }

        /// <summary>
        /// Statystyki czasu dla podanego zakresu.
        /// Statystyka dla użytkownika i ewentualnie dla konkretnego języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="startDate">Data początkowa</param>
        /// <param name="endDate">Data końcowa</param>
        /// <param name="langId">Id języka (null - dla wszystkich języków)</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Lista statystyk na konkretny dzień z wybranego zakresu</returns>
        public IList<TimeStatistics> GetTimeStatisticsForUserPeriod(int userId, DateTime startDate, DateTime endDate, int? langId, int? gameId)
        {
            var data = GetPeriodData(userId, startDate, endDate, langId, gameId).ToList();
            var groups = data.GroupBy(gst => gst.GameSession.DateStart.Date, (key, list) =>
                {
                    var dataList = list as IList<GameSessionTranslation> ?? list.ToList();
                    var firstEl = dataList.First();
                    return new TimeStatistics
                    {
                        Date = firstEl.GameSession.DateStart.Date,
                        Game = firstEl.GameSession.Game,
                        MinDuration = Math.Round(dataList.Min(d => d.Duration), 1),
                        MaxDuration = Math.Round(dataList.Max(d => d.Duration), 1),
                        AvgDuration = Math.Round(dataList.Average(d => d.Duration), 1)
                    };
                })
                .OrderBy(x => x.Date)
                .ToList();
            return groups;
        }

        /// <summary>
        /// Statystyki dla konkretnej sesji
        /// </summary>
        /// <param name="gameSessionId">Id sesji</param>
        /// <returns>Statystyka sesji</returns>
        public Statistics GetStatisticsForGameSession(int gameSessionId)
        {
            var data = Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.Id == gameSessionId)
                .ToList();
            return ConvertGameSessionsToStatistics(data);
        }

        /// <summary>
        /// Statystyki dla konkretnej sesji (ostatniej użytkownika)
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Statystyka sesji</returns>
        public Statistics GetStatisticsForLastUserGameSession(int userId)
        {
            var lastSession = Context.GameSessions
                .OrderByDescending(gs => gs.Id)
                .FirstOrDefault(gs => gs.UserId == userId);

            if (lastSession == null) return new Statistics();

            var data = Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.Id == lastSession.Id)
                .ToList();
            return ConvertGameSessionsToStatistics(data);
        }

        /// <summary>
        /// Statystyki dla słownika (dla użytkownika) - podsumowanie odpowiedzi
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Statystyka słownika</returns>
        public Statistics GetSummaryStatisticsForDictionary(int dictionaryId, int userId, int? gameId)
        {
            var data = Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.Dictionary.Id == dictionaryId)
                .Where(gst => gameId == null || gameId == gst.GameSession.GameId)
                .ToList();
            return ConvertGameSessionsToStatistics(data);
        }

        /// <summary>
        /// Szczegółowe statystyki słownika (dla użytkownika)
        /// Procent poprawnych odpowiedzi dla każdego słówka w słowniku
        /// Dotyczy słówek, na które została udzielona przynajmniej jedna odpowiedź
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Statystyki dla słówek</returns>
        public IList<TranslationStatistics> GetDetailsStatisticsForDictionary(int dictionaryId, int userId, int? gameId)
        {
            var data = GetGameSessionDataForDictionary(dictionaryId, userId, gameId);
            var groups = data.GroupBy(x => x.TranslationId, (key, list) =>
                {
                    var dataList = list as IList<GameSessionTranslation> ?? list.ToList();
                    var firstEl = dataList.First();

                    return new TranslationStatistics
                    {
                        Word = firstEl.Translation.SecondLangWord,
                        ProcentOfCorrectAnswers = dataList.Count(x => x.Correct) / dataList.Count
                    };
                })
                .ToList();
            return groups;
        }

        /// <summary>
        /// Dzienne statystyki użytkownika
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="day">Dzień (null - dzisiejszy dzień)</param>
        /// <returns>Statystyki podsumowaujące dany dzień</returns>
        public DailyStatistics GetDailyStatisticsForUser(int userId, DateTime? day)
        {
            var date = day ?? DateTime.Today;

            var data = Context.GameSessionTranslations
                .Where(x => x.GameSession.UserId == userId)
                .Where(x => x.GameSession.DateStart.Date == date.Date)
                .ToList();

            if (data.Count == 0) return new DailyStatistics();

            var result = new DailyStatistics
            {
                GamesCount = data.GroupBy(x => x.GameSessionId).Count(),
                AnswersRate = Math.Round((decimal) data.Count(x => x.Correct) / data.Count() * 100, 0),
                AverageTime = Math.Round(data.Average(x => x.Duration), 1),
                TotalTime = Math.Round(data.Sum(x => x.Duration))
            };

            return result;
        }

        #endregion

        #region Helpery

        /// <summary>
        /// Pobranie danych dla słownika i konkretnego użytkownika
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Lista danych ze wszystkich sesji dla danego słownika</returns>
        private IList<GameSessionTranslation> GetGameSessionDataForDictionary(int dictionaryId, int userId, int? gameId)
        {
            return Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.UserId == userId)
                .Where(gst => gst.GameSession.Dictionary.Id == dictionaryId)
                .Where(gst => gameId == null || gameId == gst.GameSession.GameId)
                .OrderBy(gst => gst.Translation.SecondLangWord)
                .ToList();
        }

        /// <summary>
        /// Przekonwerowanie listy (danych) na statystykę podsumowującą
        /// </summary>
        /// <param name="data">Lista danych z sesji</param>
        /// <returns>Statystyka podsumowująca</returns>
        private Statistics ConvertGameSessionsToStatistics(IList<GameSessionTranslation> data)
        {
            if (data.Count == 0) return new Statistics();

            return new Statistics
            {
                Date = data.First().GameSession.DateStart,
                Game = data.First().GameSession.Game,
                CorrectAnswers = data.Count(x => x.Correct),
                WrongAnswers = data.Count(x => !x.Correct),
                AverageTime = (decimal)data.Average(x => x.Duration)
            };
        }

        /// <summary>
        /// Bazowe dane do statystyk z zadanego zakresu
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="startDate">Data początkowa</param>
        /// <param name="endDate">Data końcowa</param>
        /// <param name="langId">Id języka (null - dla wszystkich języków)</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Dane ze wszystkich pasujących gier</returns>
        private IList<GameSessionTranslation> GetPeriodData(int userId, DateTime startDate, DateTime endDate, int? langId, int? gameId)
        {
            var data = Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.UserId == userId)
                .Where(gst => langId == null || langId == gst.GameSession.Dictionary.SecondLanguageId)
                .Where(gst => gameId == null || gameId == gst.GameSession.GameId)
                .Where(gst => gst.GameSession.DateStart.Date >= startDate.Date)
                .Where(gst => gst.GameSession.DateStart.Date <= endDate.Date)
                .ToList();
            return data;
        }

        #endregion
    }
}
