import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class ChartsService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/charts'); }

    getStatisticsForGameSession(gameSessionId: number, callback: Function, scope: any) {
        let me = this;
        let params = {
            gameSessionId: gameSessionId
        };
        me.service.get(me.api() + '/GetStatisticsForGameSession', params, callback, scope);
    }
    
    getChartForUserByPeriod(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.get(me.api() + '/GetChartForUserByPeriod', params, callback, scope);
    }

    getLastSessionStatistics(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.get(me.api() + '/GetLastSessionStatistics', params, callback, scope);
    }

    getGamesStatisticsForUser(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.get(me.api() + '/GetGamesStatisticsForUser', params, callback, scope);
    }
}