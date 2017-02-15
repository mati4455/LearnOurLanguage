import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class GamesService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/games'); }

    initializeGameQuiz(callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/InitializeQuizGame', callback, scope);
    }

    updateAnswers(callback: Function, scope: any) {
        let me = this;
        me.service.put(me.api() + '/UpdateAnswers', callback, scope);
    }

    finishGameSession(gameSessionId: number, callback: Function, scope: any) {
        let me = this;
        me.service.put(me.api() + '/FinishGameSession/' + gameSessionId, callback, scope);
    }
}