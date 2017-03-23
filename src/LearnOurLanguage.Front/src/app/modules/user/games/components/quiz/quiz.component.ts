import {
    DictionaryModel, QuizModel, QuizParameters,
    AnswerUpdateModel, PieChartData, KeysEnum
} from 'lol/models';
import { GamesService, DictionariesService, ChartsService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PieChartColors } from 'lol/consts';
import { GamesHelper } from 'lol/helpers';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

declare let $: any;
declare let store: any;

@Component({
    selector: 'game-quiz',
    templateUrl: 'quiz.component.html',
    styleUrls: ['./quiz.scss'],
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

    /**
     * Settings / Parameters for play
     *
     * @type {QuizParameters}
     * @memberOf QuizComponent
     */
    parameters: QuizParameters = new QuizParameters();

    /**
     * Id of logged user
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    userId: number;

    /**
     * Id of started game
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    gameSessionId: number;

    /**
     * Array with question for current play
     *
     * @type {Array<QuizModel>}
     * @memberOf QuizComponent
     */
    questions: Array<QuizModel> = [];

    /**
     * Array with questions answers
     *
     * @type {Array<AnswerUpdateModel>}
     * @memberOf QuizComponent
     */
    answers: Array<AnswerUpdateModel> = [];

    /**
     * Model with current question
     *
     * @type {QuizModel}
     * @memberOf QuizComponent
     */
    model: QuizModel;

    /**
     * Property describing that current play was saved
     *
     * @type {boolean}
     * @memberOf QuizComponent
     */
    saved: boolean = false;

    /**
     * Model with sum-up statistics for current play
     *
     * @type {PieChartData}
     * @memberOf QuizComponent
     */
    stats: PieChartData = null;

    /**
     * Property describing that navigation panel should be visible
     *
     * @type {boolean}
     * @memberOf QuizComponent
     */
    showNav: boolean = false;

    /**
     * Index of current question
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    questionIndex: number = 0;

    /**
     * Total count of questions
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    questionsCount: number = 0;

    /**
     * List of dictionaries to chose
     *
     * @type {*}
     * @memberOf QuizComponent
     */
    selDictionaryList: any;

    /**
     * Selected dictionary
     *
     * @type {DictionaryModel}
     * @memberOf QuizComponent
     */
    selectedDictionary: DictionaryModel;

    /**
     * Property describing support of speech (TTS) in browser
     *
     * @type {boolean}
     * @memberOf QuizComponent
     */
    speechSupport: boolean;

    /**
     * Timestamp of start game
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    startTime: number = 0;

    /**
     * Timestamp of endgame
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    endTime: number = 0;

    /**
     * Different between timestamp
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    diffTime: number = 0;

    /**
     * Property holding the time counting interval
     *
     * @type {*}
     * @memberOf QuizComponent
     */
    interval: any = null;

    /**
     * Time of animation (in milliseconds)
     *
     * @type {number}
     * @memberOf QuizComponent
     */
    animationTime: number = 300;

    /**
     * Array with colors for chart
     *
     * @memberOf QuizComponent
     */
    chartColors = PieChartColors;

    /**
     * Creates an instance of QuizComponent
     * @param {GamesHelper} gamesHelper
     * @param {DictionariesService} dictionariesService
     * @param {ChartsService} chartsService
     * @param {GamesService} gamesService
     * @param {ToastsManager} _toast
     * @param {Router} router
     *
     * @memberOf QuizComponent
     */
    constructor(
        private gamesHelper: GamesHelper,
        private dictionariesService: DictionariesService,
        private chartsService: ChartsService,
        private gamesService: GamesService,
        private _toast: ToastsManager,
        public router: Router) { }

    /**
     * Method called when component is starting
     * Getting id of user and checking speech support
     *
     * @memberOf QuizComponent
     */
    ngOnInit() {
        let me = this;
        me.userId = +store('userId');
        me.speechSupport = me.gamesHelper.speechSupport;
    }

    /**
     * Method called when component is destroying
     * If the game is not finished then session will be ended automatically
     *
     * @memberOf QuizComponent
     */
    ngOnDestroy() {
        let me = this;
        if (!me.saved && me.model) {
            me.endSession(false);
        }
    }

    /**
     * Method listening on key press
     *
     * @param {KeyboardEvent} event Key pressed
     * @returns
     *
     * @memberOf QuizComponent
     */
    handleKeyboardEvents(event: KeyboardEvent) {
        let me = this;
        let key = event.which || event.keyCode;

        if (!me.model) return;

        if (key >= KeysEnum.NUM1 && key <= KeysEnum.NUM9) {
            let index = key - KeysEnum.NUM1;
            if (!me.showNav && index < me.model.answers.length) {
                $('.answers button')[index].click();
            }
        }

        if (key == KeysEnum.ENTER && me.showNav && me.isNextQuestion()) {
            me.nextQuestion();
        }

        if (key == KeysEnum.SPACE && me.showNav) {
            me.ttsPlay();
        }
    }

    /**
     * Method which getting basic session parameters from server and start game
     *
     * @memberOf QuizComponent
     */
    startGame() {
        let me = this;
        me.parameters.userId = me.userId;
        me.gamesService.initializeGameQuiz(me.parameters, me.initializeGame, me);
    }

    /**
     * Initialization of game. Prepare questions and show first question.
     *
     * @param {*} data Response from server
     *
     * @memberOf QuizComponent
     */
    initializeGame(data: any) {
        let me = this;
        me.questions = data;

        if (me.questions.length > 0) {
            me.gameSessionId = me.questions[0].gameSessionId;
            me.prepareQuestions();
            me.nextQuestion();
        } else {
            me._toast.warning('Wybrany słownik nie zawiera słówek. Wybierz inny słownik.');
        }
    }

    /**
     * Confirmation of answer. Checking correctness and save answer.
     *
     * @param {string} answer User's answer
     * @param {Event} event
     *
     * @memberOf QuizComponent
     */
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

    /**
     * Method which preparing question and preparing shuffle answers for them.
     *
     * @memberOf QuizComponent
     */
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

    /**
     * Method checking if it is the next question.
     *
     * @returns
     *
     * @memberOf QuizComponent
     */
    isNextQuestion() {
        let me = this;
        return me.questions.length > 0;
    }

    /**
     * Method serving way of change question. Animation and change question.
     *
     * @memberOf QuizComponent
     */
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

            me.diffTime = me.liveTime();
            me.interval = setInterval(() => {
                me.diffTime = me.liveTime();
            }, 1000);

        }, me.animationTime);
    }

    /**
     * Method which ending session. Update session and save answers.
     *
     * @param {boolean} loadStats
     *
     * @memberOf QuizComponent
     */
    endSession(loadStats: boolean) {
        let me = this;

        me.gamesService.finishGameSession(me.gameSessionId, () => { }, me);
        me.gamesService.insertAnswers(me.answers, me.getStatistics, me);
    }

    /**
     * Getting statistics for session
     *
     * @param {*} data Response from server
     * @returns
     *
     * @memberOf QuizComponent
     */
    getStatistics(data: any) {
        let me = this;
        if (!data) {
            return;
        }

        // załadowanie statystyk
        me.chartsService.getStatisticsForGameSession(me.gameSessionId, me.showCharts, me);
    }

    /**
     * Binding received statistics for template
     *
     * @param {*} data
     *
     * @memberOf QuizComponent
     */
    showCharts(data: any) {
        let me = this;
        me.model = null;
        me.stats = data;
    }

    /**
     * Live update of answer's duration
     *
     * @returns
     *
     * @memberOf QuizComponent
     */
    liveTime() {
        let me = this;
        me.endTime = new Date().getTime();
        return me.calculateDuration();
    }

    /**
     * Calculate duration for current question
     *
     * @returns
     *
     * @memberOf QuizComponent
     */
    calculateDuration() {
        let me = this;
        return me.gamesHelper.calculateDuration(me.startTime, me.endTime);
    }

    /**
     * Playing answer by TTS
     *
     * @memberOf QuizComponent
     */
    ttsPlay() {
        let me = this;
        me.gamesHelper.ttsPlay(
            me.model.translation.secondLangWord,
            me.selectedDictionary.secondLanguage.code);
    }

    /**
     * Event for dictionary change
     *
     * @param {*} value
     *
     * @memberOf QuizComponent
     */
    dictionaryChange(value: any) {
        let me = this;
        me.selectedDictionary = value;
    }
}
