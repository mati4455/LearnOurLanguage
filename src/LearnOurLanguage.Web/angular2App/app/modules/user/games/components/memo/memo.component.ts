import './memo.scss';
import {
    DictionaryModel, MemoModel, MemoParametersModel, MemoQuestionModel, MemoVoModel,
    AnswerUpdateModel, PieChartData, KeysEnum
} from 'lol/models';
import { trigger, state, style, transition, animate } from '@angular/core';
import { GamesService, DictionariesService, ChartsService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { PieChartColors } from 'lol/consts';
import { GamesHelper } from 'lol/helpers';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

let $ = require('jquery');
let store = require('store2');

@Component({
    selector: 'game-memo',
    templateUrl: 'memo.component.html',
    providers: [
        GamesService,
        DictionariesService,
        ChartsService
    ],
    host: {
        '(document:keydown)': 'handleKeyboardEvents($event)'
    }
})

export class MemoComponent {

    parameters: MemoParametersModel = new MemoParametersModel();
    dictionaries: Array<DictionaryModel> = [];
    userId: number;
    gameSessionId: number;

    questions: Array<MemoQuestionModel> = [];
    answers: Array<AnswerUpdateModel> = [];
    model: MemoQuestionModel;
    saved: boolean = false;
    stats: PieChartData = null;
    showNav: boolean = false;
    questionIndex: number = 0;
    questionsCount: number = 0;
    selDictionaryList: any;
    selectedDictionary: DictionaryModel;
    chosenAnswer: number = 0;
    chosenTarget: any = null;
    gridSize: number = 1; //1 2 3 4 6
    correctCount: number = 0;
    attemptsCount: number = 0;
    currentSessionTime: number = 0;
    numberOfAsnwers: number = 0;
    correctIds: Array<number> = [];
    wrongIds: Array<number> = [];

    startTime: number = 0;
    elapsedSeconds: number = 0;
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
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
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

        if (key == KeysEnum.ENTER && me.showNav && me.isNextQuestion()) {
            me.nextQuestion();
        }
        else if (key == KeysEnum.ENTER && me.showNav && !me.isNextQuestion()) {
            me.endSession(true);
        }
    }

    startGame() {
        let me = this;
        me.parameters.userId = me.userId;
        me.gamesService.initializeGameMemo(me.parameters, me.initializeGame, me);
    }

    loadDictionaries(data: any) {
        let me = this;
        me.dictionaries = data;
    }

    confirmAnswer(translationId: number, event: any) {

        let me = this;
        me.attemptsCount++;
        $(event.target).parent('.answerButton').addClass('chosen');

        if (me.chosenAnswer > 0) {

            let correct = me.chosenAnswer == translationId;
            if (correct) {
                $(me.chosenTarget).parent('.answerButton').addClass('correct');
                $(event.target).parent('.answerButton').addClass('correct');
                me.correctIds.push(me.chosenAnswer);
                me.correctCount = me.correctCount - 2;
                if (me.correctCount == 0) {
                    me.showNav = true;
                    me.correctCount = me.numberOfAsnwers;
                    clearInterval(me.interval);
                    me.generateAnswers();
                }
            }
            me.chosenAnswer = 0;
            me.chosenTarget = null;
            setTimeout(function () {
                $('.chosen').removeClass('chosen');
            }, 700);
        } else {
            me.chosenAnswer = translationId;
            me.chosenTarget = event.target;
        }

    }

    prepareGridSize() {
        let me = this;
        let temp = me.questions[0].answers.length;
        me.correctCount = me.questions[0].answers.length;
        let availableSize = [3, 4, 6, 2, 1];
        let found = false;
        for (let i = 0; i < availableSize.length && !found; i++) {
            if (temp % (12 / availableSize[i]) == 0) {
                me.gridSize = availableSize[i];
                found = true;
            }
        }
    }

    initializeGame(data: any) {
        let me = this;
        me.answers = new Array<AnswerUpdateModel>();
        me.prepareQuestions(data);
        me.prepareGridSize();
        if (me.questions.length > 0) {
            me.gameSessionId = me.questions[0].gameSessionId;
            me.selectedDictionary = me.dictionaries.find((item) => item.id == me.parameters.dictionaryId);
            me.questions = me.gamesHelper.shuffle(me.questions);
            me.questionsCount = me.questions.length;
            me.nextQuestion();
        } else {
            me._toast.warning('Wybrany słownik nie zawiera słówek. Wybierz inny słownik.');
        }
    }

    prepareQuestions(data: any) {
        let me = this;
        for (let i = 0; i < data.length; i++) {
            let question = new MemoQuestionModel();
            question.gameSessionId = data[i].gameSessionId;
            for (let j = 0; j < data[i].translations.length; j++) {
                let translation = data[i].translations[j];
                question.answers.push(new MemoVoModel(translation.id, translation.firstLangWord));
                question.answers.push(new MemoVoModel(translation.id, translation.secondLangWord));
            }
            me.gamesHelper.shuffle(question.answers);
            me.questions.push(question);
        }
        me.numberOfAsnwers = me.questions[0].answers.length;
    }

    nextQuestion() {
        let me = this;
        me.correctIds = new Array<number>();
        me.wrongIds = new Array<number>();
        me.correctCount = me.questions[0].answers.length;
        me.showNav = false;
        me.questionIndex++;
        me.model = me.questions.shift();
        me.elapsedSeconds = me.model.answers.length * 1;
        me.startTime = me.elapsedSeconds;
        me.interval = setInterval(() => {
            me.elapsedSeconds--;
            if (me.elapsedSeconds <= 0) {
                clearInterval(me.interval);
                me.generateAnswers();
                me.showNav = true;
            }
        }, me.updateTimeInterval);
    }


    isNextQuestion() {
        let me = this;
        return me.questions.length > 0;
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

    generateAnswers() {
        let me = this;
        if (!me.model.answers) {
            return;
        }
        for (let i = 0; i < me.model.answers.length; i++) {
            if (!me.correctIds.find((item) => item == me.model.answers[i].translationId)) {
                me.wrongIds.push(me.model.answers[i].translationId);
            }
        }
        me.wrongIds = me.gamesHelper.uniqueArray(me.wrongIds);
        me.correctIds = me.gamesHelper.uniqueArray(me.correctIds);
        let avg = (me.startTime - me.elapsedSeconds) / (me.model.answers.length / 2);
        for (let i = 0; i < me.correctIds.length; i++) {
            me.answers.push(new AnswerUpdateModel(me.gameSessionId, me.correctIds[i], true, avg));
        }
        for (let i = 0; i < me.wrongIds.length; i++) {
            me.answers.push(new AnswerUpdateModel(me.gameSessionId, me.wrongIds[i], false, avg));
            $('.trans-' + me.wrongIds[i]).addClass('wrong');
        }
    }

}
