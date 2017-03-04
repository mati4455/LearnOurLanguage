import { Injectable } from '@angular/core';
import { BaseHttpService } from './http.service';
import { BaseService } from './base.service';


@Injectable()
export class GamesService extends BaseService {

    constructor(private serv: BaseHttpService) { super(serv, '/api/games'); }

    initializeGameQuiz(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/InitializeQuizGame', params, callback, scope);
    }

    initializeGameHangman(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/InitializeHangmanGame', params, callback, scope);
    }

    initializeGameFlashcards(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/InitializeFlashcardsGame', params, callback, scope);
    }

    initializeGameMemo(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/InitializeMemoGame', params, callback, scope);
    }

    insertAnswers(params: any, callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/InsertAnswers', params, callback, scope);
    }

    finishGameSession(gameSessionId: number, callback: Function, scope: any) {
        let me = this;
        me.service.post(me.api() + '/FinishGameSession/' + gameSessionId, callback, scope);
    }
}