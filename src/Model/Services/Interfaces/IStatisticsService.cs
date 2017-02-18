﻿using System;
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
        IList<GameStatistic> GetGamesStatisticsForUser(int userId, int? langId);
        IList<Statistics> GetStatisticsForUserPeriod(int userId, DateTime startDate, DateTime endDate, int? langId, int? gameId);
        IList<TimeStatistics> GetTimeStatisticsForUserPeriod(int userId, DateTime startDate, DateTime endDate, int? langId, int? gameId);
        Statistics GetStatisticsForGameSession(int gameSessionId);
        Statistics GetStatisticsForLastUserGameSession(int userId);
        Statistics GetSummaryStatisticsForDictionary(int dictionaryId, int userId, int? gameId);
        IList<TranslationStatistics> GetDetailsStatisticsForDictionary(int dictionaryId, int userId, int? gameId);
        DailyStatistics GetDailyStatisticsForUser(int userId, DateTime? day);
    }
}
