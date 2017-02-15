import { QuizParameters } from './../../../../../models/games/quiz-parameters.model';
import { GamesService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import './quiz.scss';

@Component({
    selector: 'game-quiz',
    templateUrl: 'quiz.component.html',
    providers: [
        GamesService
    ]
})

export class QuizComponent {

    parameters: QuizParameters = new QuizParameters();
    model: any;
    stats: any;

    constructor(private service: GamesService, public router: Router) { }

    ngOnInit() {
        let me = this;
    }

    startGame() {
        let me = this;
        console.log(me.parameters);
    }
}
