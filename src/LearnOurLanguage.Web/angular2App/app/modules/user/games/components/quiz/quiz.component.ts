import { DictionaryModel } from 'lol/models/dictionary';
import { QuizParameters } from 'lol/models/games';
import { GamesService, DictionariesService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import './quiz.scss';

@Component({
    selector: 'game-quiz',
    templateUrl: 'quiz.component.html',
    providers: [
        GamesService,
        DictionariesService
    ]
})

export class QuizComponent {

    parameters: QuizParameters = new QuizParameters();
    dictionaries: Array<DictionaryModel> = [];
    userId: number;
    model: any;
    stats: any;

    constructor(
        private dictionariesService: DictionariesService, 
        private service: GamesService, 
        public router: Router) { }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
    }

    startGame() {
        let me = this;
        me.parameters.userId = me.userId;

        console.log(me.parameters);
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }
}
