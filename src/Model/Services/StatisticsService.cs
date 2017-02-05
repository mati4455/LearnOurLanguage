using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Helpers;
using Model.Models;
using Model.Models.Database;
using Model.Models.Statistics;
using Model.Models.Statistics.Charts;
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

        #region Stałe, Etykiety
        const string NumberOfCorrectAnswers = "Poprawne odpowiedzi";
        const string NumberOfWrongAnswers = "Błędne odpowiedzi";
        const string AverageTime = "Średni czas odpowiedzi";
        const string WordName = "Słówka";
        #endregion

        #region Context i konstruktor

        private DatabaseContext Context { get; }

        public StatisticsService(DatabaseContext db)
        {
            Context = db;
        }

        #endregion

        #region Przekształcenie danych na format wykresów (wg. ng2-chart)

        /// <summary>
        /// Przekształcenie danych o popularności gier na format wykresów
        /// </summary>
        /// <param name="data">Dane statystyczne</param>
        /// <returns>Dane w formacie dla wykresu Pie Chart</returns>
        public PieChartData GetGamesStatisticPieChartForUser(IList<GameStatistic> data)
        {
            var result = new PieChartData();
            foreach (var row in data)
            {
                result.Labels.Add(row.GameName);
                result.Data.Add(row.NumberOfGames);
            }
            return result;
        }

        /// <summary>
        /// Przekształcenie danych o statystykach użytkownika na format wykresów
        /// </summary>
        /// <param name="data">Dane statystyczne</param>
        /// <returns>Dane w formacie dla wykresu Line Bar Chart</returns>
        public LineChartData GetSummaryStatistics(IList<Statistics> data)
        {
            var result = new LineChartData
            {
                Labels = data.Select(x => FormatHelper.DateFormat(x.Date)).ToList(),
                Data = new List<SingleSerieData>
                {
                    new SingleSerieData
                    {
                       Label = NumberOfCorrectAnswers,
                       Data = data.Select(x => x.CorrectAnswers).ToList()
                    },
                    new SingleSerieData
                    {
                       Label = NumberOfWrongAnswers,
                       Data = data.Select(x => x.WrongAnswers).ToList()
                    },
                    new SingleSerieData
                    {
                       Label = AverageTime,
                       Data = data.Select(x => x.AverageTime).ToList()
                    }
                }
            };
            return result;
        }

        /// <summary>
        /// Przekształcenie danych o statystykach w formacie podstawowym
        /// </summary>
        /// <param name="data">Statystyka podstawowa</param>
        /// <returns>Dane w formacie dla wykresu Pie Chart</returns>
        public PieChartData GetBasicStatistics(Statistics data)
        {
            return new PieChartData
            {
                Labels = new List<string> { NumberOfCorrectAnswers, NumberOfWrongAnswers },
                Data = new List<decimal> { data.CorrectAnswers, data.WrongAnswers}
            };
        }

        /// <summary>
        /// Przekształcenie szczegółowych statystyk słownika
        /// </summary>
        /// <param name="data">Szczegółowe statystyki słownika</param>
        /// <returns>Dane w formacie dla wykresu Line Chart</returns>
        public LineChartData GetDetailsStatisticsForDictionary(IList<TranslationStatistics> data)
        {
            var result = new LineChartData
            {
                Labels = data.Select(x => x.Word).ToList(),
                Data = new List<SingleSerieData>
                {
                    new SingleSerieData
                    {
                        Label = WordName,
                        Data = data.Select(x => x.ProcentOfCorrectAnswers).ToList()
                    }
                } 
            };
            return result;
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
        /// Statystyki postępów udzielanaych odpowiedzi i czasu dla podanego zakresu.
        /// Statystyka dla użytkownika i ewentualnie dla konkretnego języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="startDate">Data początkowa</param>
        /// <param name="endDate">Data końcowa</param>
        /// <param name="langId">Id języka (null - dla wszystkich języków)</param>
        /// <returns>Lista statystyka na konkretny dzień z wybranego zakresu</returns>
        public IList<Statistics> GetStatisticsForUserPeriod(int userId, DateTime startDate, DateTime endDate, int? langId)
        {
            var data = Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.UserId == userId)
                .Where(gst => langId == null || langId == gst.GameSession.Dictionary.SecondLanguageId)
                .Where(gst => gst.GameSession.DateStart.Date >= startDate.Date)
                .Where(gst => gst.GameSession.DateStart.Date <= endDate.Date)
                .ToList();
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
                        AverageTime = (decimal) dataList.Average(d => d.Duration)
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
        /// Statystyki dla słownika (dla użytkownika) - podsumowanie odpowiedzi
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Statystyka słownika</returns>
        public Statistics GetSummaryStatisticsForDictionary(int dictionaryId, int userId)
        {
            var data = Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.Dictionary.Id == dictionaryId)
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
        /// <returns>Statystyki dla słówek</returns>
        public IList<TranslationStatistics> GetDetailsStatisticsForDictionary(int dictionaryId, int userId)
        {
            var data = GetGameSessionDataForDictionary(dictionaryId, userId);
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

        #endregion

        #region Helpery

        /// <summary>
        /// Pobranie danych dla słownika i konkretnego użytkownika
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Lista danych ze wszystkich sesji dla danego słownika</returns>
        private IList<GameSessionTranslation> GetGameSessionDataForDictionary(int dictionaryId, int userId)
        {
            return Context.GameSessionTranslations
                .Include(gst => gst.GameSession.Game)
                .Where(gst => gst.GameSession.UserId == userId)
                .Where(gst => gst.GameSession.Dictionary.Id == dictionaryId)
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

        #endregion
    }
}
