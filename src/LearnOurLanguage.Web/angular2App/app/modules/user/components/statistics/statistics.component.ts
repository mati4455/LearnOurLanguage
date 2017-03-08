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
    languages: Array<LanguageModel> = null;
    games: Array<GamesModel> = null;

    languageId: number = 0;
    dictionaryId: number = 0;
    gameId: number = 0;
    startDate: Date = null;
    endDate: Date = null;


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

        me.initDailyStatistics();
        me.initLastSessionChart();
        me.initGamesRankChart();
        me.initPeriodChart();
        me.initPeriodTimeChart();
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
        me.languageService.getAll(me.loadLanguages, me);
        me.prepareGames();
    }

    generate() {

    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    prepareGames() {
        let me = this;
        me.games = [
            new GamesModel(1, "Flashcards"),
            new GamesModel(2, "Quiz"),
            new GamesModel(3, "Memo"),
            new GamesModel(4, "Hangman")
        ];
        console.log(me.games);
    }

    loadLanguages(data: any) {
        let me = this;
        me.languages = data;
    }

    initDailyStatistics() {
        let me = this;
        me.chartsService.getDailyStatistics({
            userId: me.userId
        }, me.loadDailyStatistics, me);
    }

    loadDailyStatistics(data: any) {
        let me = this;
        me.dailyStatistics = new DailyStatistics(data.gamesCount, data.answersRate, data.averageTime, data.totalTime);
    }

    initLastSessionChart() {
        let me = this;
        me.chartsService.getLastSessionStatistics({
            userId: me.userId
        }, me.loadLastSessionChart, me);
    }

    loadLastSessionChart(data: any) {
        let me = this;
        me.lastSessionStatistics = data;
    }

    initGamesRankChart() {
        let me = this;
        me.chartsService.getGamesStatisticsForUser({
            userId: me.userId,
            langId: null
        }, me.loadGamesRankChart, me);
    }

    loadGamesRankChart(data: any) {
        let me = this;
        me.gamesRankStatistics = data;
    }

    initPeriodChart() {
        let me = this;
        let start = new Date();
        start.setDate(start.getDate() - 6);
        let end = new Date();

        me.chartsService.getChartForUserByPeriod({
            userId: me.userId,
            langId: null,
            gameId: null,
            startDate: me.dateFormat(start),
            endDate: me.dateFormat(end)
        }, me.loadPeriodChart, me);
    }

    loadPeriodChart(data: any) {
        let me = this;
        me.periodStatistics = data;
    }

    initPeriodTimeChart() {
        let me = this;
        let start = new Date();
        start.setDate(start.getDate() - 6);
        let end = new Date();

        me.chartsService.getTimeChartForUserByPeriod({
            userId: me.userId,
            langId: null,
            gameId: null,
            startDate: me.dateFormat(start),
            endDate: me.dateFormat(end)
        }, me.loadPeriodTimeChart, me);
    }

    loadPeriodTimeChart(data: any) {
        let me = this;
        me.periodTimeStatistics = data;
    }

    dateFormat(date: Date) {
        let mm = date.getMonth() + 1; // getMonth() is zero-based
        let dd = date.getDate();
        return [date.getFullYear(), (mm > 9 ? '' : '0') + mm, (dd > 9 ? '' : '0') + dd].join('-');
    }
}
