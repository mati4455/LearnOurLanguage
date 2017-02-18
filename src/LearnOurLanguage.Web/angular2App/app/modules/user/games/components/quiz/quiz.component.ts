import { DictionaryModel, QuizModel, QuizParameters,
         AnswerUpdateModel, PieChartData } from 'lol/models';
import { GamesService, DictionariesService, ChartsService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PieChartColors } from 'lol/consts';
import { GamesHelper } from 'lol/helpers';

import './quiz.scss';

let $ = require('jquery');

@Component({
    selector: 'game-quiz',
    templateUrl: 'quiz.component.html',
    providers: [
        GamesService,
        DictionariesService,
        ChartsService
    ],
    host: {
        '(document:keydown)': 'handleKeyboardEvents($event)'
    }
})

export class QuizComponent {

    parameters: QuizParameters = new QuizParameters();
    dictionaries: Array<DictionaryModel> = [];
    userId: number;
    gameSessionId: number;

    questions: Array<QuizModel> = [];
    countAnswers: Array<number> = [];
    answers: Array<AnswerUpdateModel> = [];
    model: QuizModel;
    saved: boolean = false;
    stats: PieChartData = null;
    showNav: boolean = false;
    questionIndex: number = 0;
    selectedDictionary: DictionaryModel;
    speechSupport: boolean;

    startTime: number = 0;
    endTime: number = 0;
    diffTime: number = 0;
    interval: any = null;

    animationTime: number = 300;

    chartColors = PieChartColors;

    constructor(
        private gamesHelper: GamesHelper,
        private dictionariesService: DictionariesService,
        private chartsService: ChartsService,
        private gamesService: GamesService,
        public router: Router) { }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
        me.speechSupport = me.gamesHelper.speechSupport;

        setTimeout( () => {
            $('.selectpicker').selectpicker();
        }, 100);
    }

    ngOnDestroy() {
        let me = this;
        if (!me.saved && me.model) {
            me.endSession(false);
        }
    }

    handleKeyboardEvents(event: KeyboardEvent) {
        let me = this;
        let key = event.which || event.keyCode;
        let one = 49;
        let nine = 57;

        if (key >= one && key <= nine) {
            let index = key - one;
            if (index < me.model.answers.length) {
                $('.answers button')[index].click();
            }
        }

        if (key == 13 && me.isNextQuestion()) {
            me.nextQuestion();
        }

        if (key == 32 && me.showNav) {
            me.ttsPlay();
        }
    }

    startGame() {
        let me = this;
        me.parameters.userId = me.userId;

        me.gamesService.initializeGameQuiz(me.parameters, me.initializeGame, me);
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    initializeGame(data: any) {
        let me = this;
        me.questions = data;
        me.questions.forEach((item) => me.countAnswers[item.translation.id] = 0 );
        me.gameSessionId = me.questions[0].gameSessionId;
        me.selectedDictionary = me.dictionaries.find( (item) => item.id == me.parameters.dictionaryId);

        me.nextQuestion();
    }

    confirmAnswer(answer: string, event: Event) {
        let me = this;
        me.endTime = new Date().getTime();
        if (me.interval) {
            clearInterval(me.interval);
        }

        let correct = me.gamesHelper.equalsWords(me.model.translation.secondLangWord, answer);
        me.answers.push(new AnswerUpdateModel(
            me.model.gameSessionId,
            me.model.translation.id,
            correct,
            me.calculateDuration()
        ));

        me.checkToAddNewQuestion();
        me.showNav = true;
        me.ttsPlay();

        $(event.target).addClass(correct ? 'correct' : 'wrong');
    }

    checkToAddNewQuestion() {
        let me = this;
        me.countAnswers[me.model.translation.id]++;
        if (me.countAnswers[me.model.translation.id] < me.parameters.maxNumberOfRepeats) {
            me.repeatQuestion();
        }
    }

    repeatQuestion() {
        let me = this;
        me.questions.push(me.model);
    }

    isNextQuestion() {
        let me = this;
        return me.questions.length > 0;
    }

    nextQuestion() {
        let me = this;
        $('.animation').removeClass('up').addClass('down');

        setTimeout(function() {

            $('.answers button').removeClass('correct wrong');
            me.showNav = false;
            me.questionIndex++;
            me.model = me.questions.shift();
            me.model.answers = me.gamesHelper.shuffle(me.model.answers);
            me.startTime = new Date().getTime();

            $('.animation').removeClass('down').addClass('up');

            me.interval = setInterval(() => {
                me.diffTime = me.liveTime();
            }, 100);

            me.questions = me.gamesHelper.shuffle(me.questions);

        }, me.animationTime);
    }

    endSession(loadStats: boolean) {
        let me = this;

        me.gamesService.finishGameSession(me.gameSessionId, () => { }, me);
        me.gamesService.insertAnswers(me.answers, me.getStatistics, me);
    }

    getStatistics(data: any) {
        let me = this;
        if (!data) {
            return;
        }

        // załadowanie statystyk
        me.chartsService.getStatisticsForGameSession(me.gameSessionId, me.showCharts, me);
    }

    showCharts(data: any) {
        let me = this;
        me.model = null;
        me.stats = data;
    }

    liveTime() {
        let me = this;
        me.endTime = new Date().getTime();
        return me.calculateDuration();
    }

    calculateDuration() {
        let me = this;
        return me.gamesHelper.calculateDuration(me.startTime, me.endTime);
    }

    ttsPlay() {
        let me = this;
        me.gamesHelper.ttsPlay(
            me.model.translation.secondLangWord,
            me.selectedDictionary.secondLanguage.code);
    }
}
