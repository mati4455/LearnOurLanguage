import { Component, OnInit } from '@angular/core';
import { ChartsService } from 'lol/services';
import { LineChartData, PieChartData } from 'lol/models';
import { BarChartColors, BasicChartOptions, PieChartColors } from 'lol/consts';

@Component({
    selector: 'my-app',
    templateUrl: 'dashboard.component.html',
    providers: [
        ChartsService
    ]
})

export class DashboardComponent {

    periodStatistics: LineChartData;
    lastSessionStatistics: PieChartData;
    gamesRankStatistics: PieChartData;

    barChartColors = BarChartColors;
    pieChartColors = PieChartColors;
    chartOptions = BasicChartOptions;

    userId: number;

    constructor(private chartsService: ChartsService) {
        let me = this;
    }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');

        me.initLastSessionChart();
        me.initGamesRankChart();
        me.initPeriodChart();
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
        start.setDate(start.getDate() - 7);
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
    
    dateFormat(date: Date) {
        var mm = date.getMonth() + 1; // getMonth() is zero-based
        var dd = date.getDate();

        return [date.getFullYear(),
                (mm>9 ? '' : '0') + mm,
                (dd>9 ? '' : '0') + dd
                ].join('-');
    }
}
