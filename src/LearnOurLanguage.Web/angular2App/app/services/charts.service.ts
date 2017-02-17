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
}