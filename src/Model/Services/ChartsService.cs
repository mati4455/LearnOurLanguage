using System.Collections.Generic;
using System.Linq;
using Model.Helpers;
using Model.Models;
using Model.Models.Statistics;
using Model.Models.Statistics.Charts;
using Model.Services.Interfaces;

namespace Model.Services
{
    public class ChartsService : IChartsService
    {
        #region Stałe, Etykiety
        const string NumberOfCorrectAnswers = "Poprawne odpowiedzi";
        const string NumberOfWrongAnswers = "Błędne odpowiedzi";
        const string MinTime = "Minimalny czas odpowiedzi";
        const string MaxTime = "Maksymalny czas odpowiedzi";
        const string AverageTime = "Średni czas odpowiedzi";
        const string WordName = "Słówka";
        #endregion

        #region Context, Serwisy i konstruktor

        private DatabaseContext Context { get; }
        private IStatisticsService StatisticsService { get; set; }

        public ChartsService(DatabaseContext db, IStatisticsService service)
        {
            Context = db;
            StatisticsService = service;
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
            var result = new PieChartData
            {
                Data = new List<decimal>(),
                Labels = new List<string>()
            };
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
                    }
                }
            };
            return result;
        }

        /// <summary>
        /// Przekształcenie danych o statystykach użytkownika na format wykresów
        /// </summary>
        /// <param name="data">Dane statystyczne</param>
        /// <returns>Dane w formacie dla wykresu Line Bar Chart</returns>
        public LineChartData GetSummaryTimeStatistics(IList<TimeStatistics> data)
        {
            var result = new LineChartData
            {
                Labels = data.Select(x => FormatHelper.DateFormat(x.Date)).ToList(),
                Data = new List<SingleSerieData>
                {
                    new SingleSerieData
                    {
                       Label = MinTime,
                       Data = data.Select(x => x.MinDuration).ToList()
                    },
                    new SingleSerieData
                    {
                       Label = AverageTime,
                       Data = data.Select(x => x.AvgDuration).ToList()
                    },
                    new SingleSerieData
                    {
                       Label = MaxTime,
                       Data = data.Select(x => x.MaxDuration).ToList()
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
                Data = new List<decimal> { data.CorrectAnswers, data.WrongAnswers }
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
    }
}
