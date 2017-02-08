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
        /// <param name="startDate">Data początkowa statystyk</param>
        /// <param name="endDate">Data końcowa statystyk</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetChartForUserByPeriod/{userId}/{langId}/{startDate}/{endDate}")]
        public ActionResult GetChartForUserByPeriod(int userId, int? langId, DateTime startDate, DateTime endDate)
        {
            AccessGuardian(Roles.AccessUser, userId);

            var data = StatisticsService.GetStatisticsForUserPeriod(userId, startDate, endDate, langId);
            var chartData = ChartsService.GetSummaryStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Dane do wykresu dla użytkownika (statystyka popularności gier), dla wybranego (lub wszystkich) języka
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="langId">Id języka</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetGamesStatisticsForUser/{userId}/{langId}")]
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
        [HttpGet("GetStatisticsForGameSession/{gameSessionId}")]
        public ActionResult GetStatisticsForGameSession(int gameSessionId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetStatisticsForGameSession(gameSessionId);
            var chartData = ChartsService.GetBasicStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Podstawowe dane do wykresu dla słownika
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetStatisticsForDictionary/{dictionaryId}/{userId}")]
        public ActionResult GetStatisticsForDictionary(int dictionaryId, int userId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetSummaryStatisticsForDictionary(dictionaryId, userId);
            var chartData = ChartsService.GetBasicStatistics(data);
            return JsonHelper.Success(chartData);
        }

        /// <summary>
        /// Szczegółowe dane do wykresu dla słownika
        /// </summary>
        /// <param name="dictionaryId">Id słownika</param>
        /// <param name="userId">Id użytkownika</param>
        /// <returns>Obiekt opisujący wykres</returns>
        [HttpGet("GetDetailsStatisticsForDictionary/{dictionaryId}/{userId}")]
        public ActionResult GetDetailsStatisticsForDictionary(int dictionaryId, int userId)
        {
            AccessGuardian(Roles.AccessEveryone);

            var data = StatisticsService.GetDetailsStatisticsForDictionary(dictionaryId, userId);
            var chartData = ChartsService.GetDetailsStatisticsForDictionary(data);
            return JsonHelper.Success(chartData);
        }
    }
}
