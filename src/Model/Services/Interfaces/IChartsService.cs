using System.Collections.Generic;
using Model.Models.Statistics;
using Model.Models.Statistics.Charts;

namespace Model.Services.Interfaces
{
    public interface IChartsService
    {
        PieChartData GetGamesStatisticPieChartForUser(IList<GameStatistic> data);
        LineChartData GetSummaryStatistics(IList<Statistics> data);
        PieChartData GetBasicStatistics(Statistics data);
        LineChartData GetDetailsStatisticsForDictionary(IList<TranslationStatistics> data);
    }
}
