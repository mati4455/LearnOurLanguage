import './quiz.scss';
import { DictionaryModel, QuizModel, QuizParameters,
    AnswerUpdateModel, PieChartData } from 'lol/models';
import { GamesService, DictionariesService, ChartsService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PieChartColors } from 'lol/consts';
import { GamesHelper } from 'lol/helpers';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

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
    answers: Array<AnswerUpdateModel> = [];
    model: QuizModel;
    saved: boolean = false;
    stats: PieChartData = null;
    showNav: boolean = false;
    questionIndex: number = 0;
    questionsCount: number = 0;
    selDictionaryList: any;
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
        private _toast: ToastsManager,
        public router: Router) { }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
        me.speechSupport = me.gamesHelper.speechSupport;
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

        if (key == 13 && me.showNav && me.isNextQuestion()) {
            me.nextQuestion();
        }

        if (key == 32 && me.showNav) {
            me.ttsPlay();
        }
    }

    startGame() {
        let me = this;
        me.parameters.userId = me.userId;
        // me.parameters.dictionaryId = me.selDictionaryList[0].id;
        me.gamesService.initializeGameQuiz(me.parameters, me.initializeGame, me);
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    initializeGame(data: any) {
        let me = this;
        me.questions = data;

        if (me.questions.length > 0) {
            me.gameSessionId = me.questions[0].gameSessionId;
            me.selectedDictionary = me.dictionaries.find((item) => item.id == me.parameters.dictionaryId);

            me.prepareQuestions();
            me.nextQuestion();
        } else {
            me._toast.warning('Wybrany słownik nie zawiera słówek. Wybierz inny słownik.');
        }
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

        me.showNav = true;
        me.ttsPlay();

        $(event.target).addClass(correct ? 'correct' : 'wrong');
        $(event.target).blur();
    }

    prepareQuestions() {
        let me = this;
        let size = me.questions.length;
        for (let i = 0; i < size; i++) {
            for (let j = 1; j < me.parameters.maxNumberOfRepeats; j++) {
                me.questions.push(me.questions[i]);
            }
        }
        me.questionsCount = me.questions.length;
        me.questions = me.gamesHelper.shuffle(me.questions);
    }

    isNextQuestion() {
        let me = this;
        return me.questions.length > 0;
    }

    nextQuestion() {
        let me = this;
        $('.animation').removeClass('up').addClass('down');

        setTimeout(function () {

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
