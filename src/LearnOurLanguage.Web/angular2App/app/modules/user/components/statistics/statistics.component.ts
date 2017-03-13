import { Component, OnInit } from '@angular/core';
import { ChartsService, DictionariesService, LanguageService } from 'lol/services';
import { LineChartData, PieChartData, DailyStatistics } from 'lol/models';
import { BarChartColors, BasicChartOptions, PieChartColors } from 'lol/consts';
import { DictionaryModel, LanguageModel } from 'lol/models/dictionary';
import { GamesModel } from '../../../../models/games/game.model';

let store = require('store2');

@Component({
    selector: 'statistics',
    templateUrl: 'statistics.component.html',
    providers: [
        ChartsService,
        DictionariesService,
        LanguageService
    ]
})

export class StatisticsComponent {

    periodStatistics: LineChartData;
    periodTimeStatistics: LineChartData;
    lastSessionStatistics: PieChartData;
    gamesRankStatistics: PieChartData;
    dailyStatistics: DailyStatistics;
    dictionaries: Array<DictionaryModel> = null;
    dictionariesFiltered: Array<DictionaryModel> = null;
    languages: Array<LanguageModel> = null;
    games: Array<GamesModel> = null;

    languageId: number = 0;
    dictionaryId: number = 0;
    gameId: number = 0;
    startDate: Date;
    endDate: Date;


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

    loadPeriodUser(data: any) {
        let me = this;
        me.periodStatistics = data;
    }

    loadTimePeriod(data: any) {
        let me = this;
        me.periodTimeStatistics = data;
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
            new GamesModel(1, "Flashcards"),
            new GamesModel(2, "Quiz"),
            new GamesModel(3, "Memo"),
            new GamesModel(4, "Hangman")
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
