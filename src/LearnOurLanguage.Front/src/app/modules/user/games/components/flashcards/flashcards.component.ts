import {
    DictionaryModel, FlashcardsModel, FlashcardsParameters,
    AnswerUpdateModel, PieChartData, KeysEnum
} from 'lol/models';
import { trigger, state, style, transition, animate } from '@angular/core';
import { GamesService, DictionariesService, ChartsService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PieChartColors } from 'lol/consts';
import { GamesHelper } from 'lol/helpers';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

declare let $: any;
declare let store: any;

@Component({
    selector: 'game-flashcards',
    templateUrl: 'flashcards.component.html',
    styleUrls: ['./flashcards.scss'],
    providers: [
        GamesService,
        DictionariesService,
        ChartsService
    ],
    host: {
        '(document:keydown)': 'handleKeyboardEvents($event)'
    }
})

export class FlashcardsComponent {

    parameters: FlashcardsParameters = new FlashcardsParameters();
    userId: number;
    gameSessionId: number;

    questions: Array<FlashcardsModel> = [];
    answers: Array<AnswerUpdateModel> = [];
    model: FlashcardsModel;
    saved: boolean = false;
    stats: PieChartData = null;
    showNav: boolean = false;
    questionIndex: number = 0;
    questionsCount: number = 0;
    selDictionaryList: any;
    selectedDictionary: DictionaryModel;
    speechSupport: boolean;
    answerChecked: boolean = false;
    answerValue: string = '';
    answerClass: string = '';
    flip: string = 'inactive';

    startTime: number = 0;
    endTime: number = 0;
    diffTime: number = 0;
    updateTimeInterval: number = 1000;
    interval: any = null;

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
        me.userId = +store('userId');
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
        let index = -1;

        if (key == KeysEnum.ENTER && !me.showNav) {
            me.confirmAnswer();
        }

        else if (key == KeysEnum.ENTER && me.showNav && me.isNextQuestion()) {
            me.nextQuestion();
        }

        else if (key == KeysEnum.ENTER && me.showNav && !me.isNextQuestion()) {
            me.endSession(true);
        }

        else if (key == KeysEnum.SPACE && me.showNav) {
            me.ttsPlay();
        }
    }

    startGame() {
        let me = this;
        me.parameters.userId = me.userId;
        me.gamesService.initializeGameFlashcards(me.parameters, me.initializeGame, me);
    }

    confirmAnswer() {
        let me = this;
        if (me.answerValue.length == 0) return;

        me.answerChecked = true;

        me.endTime = new Date().getTime();
        if (me.interval) {
            clearInterval(me.interval);
        }

        let correct = me.gamesHelper.equalsWords(me.model.translation.secondLangWord, me.answerValue);
        me.answers.push(new AnswerUpdateModel(
            me.model.gameSessionId,
            me.model.translation.id,
            correct,
            me.calculateDuration()
        ));
        me.showNav = true;
        me.answerClass = (correct ? 'correct' : 'wrong');
        me.flip = 'active';
        me.ttsPlay();
        $('#answerButton').blur();
        $('#answerInput').blur();
    }

    initializeGame(data: any) {
        let me = this;
        me.questions = data;

        if (me.questions.length > 0) {
            me.gameSessionId = me.questions[0].gameSessionId;
            me.questions = me.gamesHelper.shuffle(me.questions);
            me.questionsCount = me.questions.length;
            me.nextQuestion();
        } else {
            me._toast.warning('Wybrany słownik nie zawiera słówek. Wybierz inny słownik.');
        }
    }

    nextQuestion() {
        let me = this;

        me.showNav = false;
        me.questionIndex++;

        me.model = me.questions.shift();
        me.startTime = new Date().getTime();
        me.flip = 'inactive';
        me.answerChecked = false;
        me.answerValue = '';
        me.answerClass = '';

        me.diffTime = me.liveTime();
        me.interval = setInterval(() => {
            me.diffTime = me.liveTime();
        }, me.updateTimeInterval);

        setTimeout(() => {
            $('#answerInput').focus();
        }, 100);
    }


    isNextQuestion() {
        let me = this;
        return me.questions.length > 0;
    }

    isChecked() {
        let me = this;
        return me.answerChecked;
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

    dictionaryChange(value: any) {
        let me = this;
        me.selectedDictionary = value;
    }
}
