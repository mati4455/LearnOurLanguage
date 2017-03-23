import { Component, OnInit, ViewChildren, SimpleChanges, QueryList } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts/ng2-charts';
import { ChartsService, DictionariesService, LanguageService } from 'lol/services';
import { LineChartData, PieChartData, DailyStatistics, DictionaryModel, LanguageModel, GameModel } from 'lol/models';
import { BarChartColors, BasicChartOptions, PieChartColors } from 'lol/consts';

declare let store: any;

@Component({
    selector: 'statistics',
    templateUrl: 'statistics.component.html',
    styleUrls: ['./statistics.scss'],
    providers: [
        ChartsService,
        DictionariesService,
        LanguageService
    ]
})

export class StatisticsComponent {

    @ViewChildren(BaseChartDirective) charts: QueryList<BaseChartDirective>;
    chartsArray: Array<BaseChartDirective>;

    periodStatistics: LineChartData;
    periodTimeStatistics: LineChartData;
    dictionariesStatistics: PieChartData;
    dictionariesDetailsStatistics: LineChartData;

    dictionaries: Array<DictionaryModel> = null;
    dictionariesFiltered: Array<DictionaryModel> = null;
    languages: Array<LanguageModel> = null;
    games: Array<GameModel> = null;

    languageId: number = 0;
    dictionaryId: number = 0;
    gameId: number = 0;
    startDate: Date;
    endDate: Date;
    gameDicId: number = 0;
    dicId: number = 0;

    barChartColors = BarChartColors;
    pieChartColors = PieChartColors;
    chartOptions = BasicChartOptions;

    userId: number;

    constructor(private chartsService: ChartsService, private dictionariesService: DictionariesService, private languageService: LanguageService) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        me.userId = +store('userId');

        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
        me.languageService.getAll(me.loadLanguages, me);
        me.prepareGames();
    }

    generate() {
        let me = this;
        me.endDate = new Date();
        me.startDate = new Date();
        me.startDate.setDate(me.endDate.getDate() - 100);
        let params = {
            userId: me.userId,
            langId: me.languageId,
            gameId: me.gameId,
            startDate: me.dateFormat(me.startDate),
            endDate: me.dateFormat(me.endDate)
        };
        me.chartsService.getChartForUserByPeriod(params, me.loadPeriodUser, me);
        me.chartsService.getTimeChartForUserByPeriod(params, me.loadTimePeriod, me);
    }

    generateForDictionaries() {
        let me = this;
        let params = {
            userId: me.userId,
            dictionaryId: me.dicId,
            gameId: me.gameDicId
        };
        me.chartsService.getStatisticsForDictionary(params, me.loadDictionariesStats, me);
        me.chartsService.getDetailsStatisticsForDictionary(params, me.loadDictionariesDetailsStats, me);
    }

    loadPeriodUser(data: any) {
        let me = this;
        me.periodStatistics = data;
        me.updateChart();
    }

    loadTimePeriod(data: any) {
        let me = this;
        me.periodTimeStatistics = data;
        me.updateChart();
    }

    loadDictionariesStats(data: any) {
        let me = this;
        me.dictionariesStatistics = data;
        me.updateChart();
    }

    loadDictionariesDetailsStats(data: any) {
        let me = this;
        me.dictionariesDetailsStatistics = data;
        me.updateChart();
    }

    updateChart() {
        let me = this;
        if (me.charts) {
            setTimeout(() => {
                me.charts.forEach((chart) => {
                    chart.ngOnChanges({} as SimpleChanges);
                });
            }, 100);
        }
    }

    filterDictionaries(event: any) {
        let me = this;
        me.dictionariesFiltered = me.dictionaries.filter((element) => element.firstLanguage.id == event.target.value || element.secondLanguage.id == event.target.value);

    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
        me.dictionariesFiltered = new Array<DictionaryModel>();
    }

    prepareGames() {
        let me = this;
        me.games = [
            new GameModel(1, "Flashcards"),
            new GameModel(2, "Quiz"),
            new GameModel(3, "Memo"),
            new GameModel(4, "Hangman")
        ];
    }

    loadLanguages(data: any) {
        let me = this;
        me.languages = data;
    }

    dateFormat(date: Date) {
        let mm = date.getMonth() + 1; // getMonth() is zero-based
        let dd = date.getDate();
        return [date.getFullYear(), (mm > 9 ? '' : '0') + mm, (dd > 9 ? '' : '0') + dd].join('-');
    }
}
