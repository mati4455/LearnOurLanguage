import { Component, OnInit } from '@angular/core';
import { ChartsService } from 'lol/services';
import { LineChartData, PieChartData, DailyStatistics } from 'lol/models';
import { BarChartColors, BasicChartOptions, PieChartColors } from 'lol/consts';

declare let store: any;

@Component({
    selector: 'my-app',
    templateUrl: 'dashboard.component.html',
    styleUrls: ['./dashboard.scss'],
    providers: [
        ChartsService
    ]
})

export class DashboardComponent {

    periodStatistics: LineChartData;
    periodTimeStatistics: LineChartData;
    lastSessionStatistics: PieChartData;
    gamesRankStatistics: PieChartData;
    dailyStatistics: DailyStatistics;

    barChartColors = BarChartColors;
    pieChartColors = PieChartColors;
    chartOptions = BasicChartOptions;

    userId: number;

    constructor(private chartsService: ChartsService) {
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
