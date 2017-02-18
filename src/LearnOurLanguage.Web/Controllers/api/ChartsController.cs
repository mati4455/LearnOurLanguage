using System;
using LearnOurLanguage.Web.Base;
using Microsoft.AspNetCore.Mvc;
using Model.Const;
using Model.Services.Interfaces;

namespace LearnOurLanguage.Web.Controllers.api
{
    /// <summary>
    /// Pobieranie danych do wykresów
    /// </summary>
    [Route("api/[controller]")]
    public class ChartsController : BaseController
    {
        private IStatisticsService StatisticsService { get; set; }
        private IChartsService ChartsService { get; set; }

        /// <summary>
        /// Konstruktor wstrzykujący serwisy
        /// </summary>
        /// <param name="statisticsService">Serwis odpowiedzialny za zbieranie danych statystycznych</param>
        /// <param name="chartsService">Serwis odpowiedzialny za przygotowanie danych pod wykresy</param>
        public ChartsController(IStatisticsService statisticsService, IChartsService chartsService)
        {
            StatisticsService = statisticsService;
            ChartsService = chartsService;
        }

        /// <summary>
        /// Dane do wykresu dla użytkownika, w podanym zakresie, dla wybranego (lub wszystkich) języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="langId">Id języka (null - wszystkie języki)</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <param name="startDate">Data początkowa statystyk</param>
        /// <param name="endDate">Data końcowa statystyk</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetChartForUserByPeriod")]
        public ActionResult GetChartForUserByPeriod(int userId, int? langId, int? gameId, DateTime startDate, DateTime endDate)
        {
            AccessGuardian(Roles.AccessUser, userId);

            var data = StatisticsService.GetStatisticsForUserPeriod(userId, startDate, endDate, langId, gameId);
            var chartData = ChartsService.GetSummaryStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Dane do wykresu dla użytkownika, w podanym zakresie, dla wybranego (lub wszystkich) języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="langId">Id języka (null - wszystkie języki)</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <param name="startDate">Data początkowa statystyk</param>
        /// <param name="endDate">Data końcowa statystyk</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetTimeChartForUserByPeriod")]
        public ActionResult GetTimeChartForUserByPeriod(int userId, int? langId, int? gameId, DateTime startDate, DateTime endDate)
        {
            AccessGuardian(Roles.AccessUser, userId);

            var data = StatisticsService.GetTimeStatisticsForUserPeriod(userId, startDate, endDate, langId, gameId);
            var chartData = ChartsService.GetSummaryTimeStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Dane do wykresu dla użytkownika (statystyka popularności gier), dla wybranego (lub wszystkich) języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="langId">Id języka</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetGamesStatisticsForUser")]
        public ActionResult GetGamesStatisticsForUser(int userId, int? langId)
        {
            AccessGuardian(Roles.AccessUser, userId);

            var data = StatisticsService.GetGamesStatisticsForUser(userId, langId);
            var chartData = ChartsService.GetGamesStatisticPieChartForUser(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Dane do wykresu dla pojedynczej sesji
        /// </summary>
        /// <param name="gameSessionId">Id sesji</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetStatisticsForGameSession")]
        public ActionResult GetStatisticsForGameSession(int gameSessionId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetStatisticsForGameSession(gameSessionId);
            var chartData = ChartsService.GetBasicStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Dane do wykresu dla pojedynczej sesji (ostatniej użytkownika)
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetLastSessionStatistics")]
        public ActionResult GetLastSessionStatistics(int userId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetStatisticsForLastUserGameSession(userId);
            var chartData = ChartsService.GetBasicStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Podstawowe dane do wykresu dla słownika
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetStatisticsForDictionary")]
        public ActionResult GetStatisticsForDictionary(int dictionaryId, int userId, int? gameId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetSummaryStatisticsForDictionary(dictionaryId, userId, gameId);
            var chartData = ChartsService.GetBasicStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Szczegółowe dane do wykresu dla słownika
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="gameId">Id gry (null - wszystkie gry)</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetDetailsStatisticsForDictionary")]
        public ActionResult GetDetailsStatisticsForDictionary(int dictionaryId, int userId, int? gameId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetDetailsStatisticsForDictionary(dictionaryId, userId, gameId);
            var chartData = ChartsService.GetDetailsStatisticsForDictionary(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Statystyki użytkownika z jednego dnia
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="date">Dzień (null - dziejszy dzień)</param>
        /// <returns>Statystyki z zadanego dnia</returns>
        [HttpGet("GetDailyStatisticsForUser")]
        public ActionResult GetDailyStatisticsForUser(int userId, DateTime? date)
        {
            AccessGuardian(Roles.AccessUser, userId);

            var data = StatisticsService.GetDailyStatisticsForUser(userId, date);
            return JsonHelper.Success(data);
        }
    }
}
