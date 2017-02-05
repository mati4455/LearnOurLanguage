using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Models.Database;
using Model.Models.Statistics;
using Model.Models.Statistics.Charts;

namespace Model.Services.Interfaces
{
    public interface IStatisticsService
    {
        PieChartData GetGamesStatisticPieChartForUser(IList<GameStatistic> data);
        LineChartData GetSummaryStatistics(IList<Statistics> data);
        PieChartData GetBasicStatistics(Statistics data);
        LineChartData GetDetailsStatisticsForDictionary(IList<TranslationStatistics> data);
        IList<GameStatistic> GetGamesStatisticsForUser(int userId, int? langId);
        IList<Statistics> GetStatisticsForUserPeriod(int userId, DateTime startDate, DateTime endDate, int? langId);
        Statistics GetStatisticsForGameSession(int gameSessionId);
        Statistics GetSummaryStatisticsForDictionary(int dictionaryId, int userId);
        IList<TranslationStatistics> GetDetailsStatisticsForDictionary(int dictionaryId, int userId);
    }
}
