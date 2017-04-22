import {
    DictionaryModel, HangmanModel, HangmanParameters,
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
    selector: 'game-hangman',
    templateUrl: 'hangman.component.html',
    styleUrls: ['./hangman.scss'],
    providers: [
        GamesService,
        DictionariesService,
        ChartsService
    ],
    host: {
        '(document:keydown)': 'handleKeyboardEvents($event)'
    }
})

export class HangmanComponent {

    parameters: HangmanParameters = new HangmanParameters();
    userId: number;
    gameSessionId: number;

    questions: Array<HangmanModel> = [];
    answers: Array<AnswerUpdateModel> = [];
    model: HangmanModel;
    saved: boolean = false;
    stats: PieChartData = null;
    showNav: boolean = false;
    questionIndex: number = 0;
    questionsCount: number = 0;
    selDictionaryList: any;
    selectedDictionary: DictionaryModel;
    speechSupport: boolean;

    availableCharacters: Array<string> = [];
    usedCharacters: any = [];
    answerMask: Array<string> = [];
    letterMask: string = '_';
    incrementSize: number = 0;
    wordLengthLimit: number = 5;
    hangmanParts: number = 9;
    wrongAnswers: number = 0;

    startTime: number = 0;
    endTime: number = 0;
    diffTime: number = 0;
    interval: any = null;

    animationTime: number = 300;
    updateTimeInterval: number = 1000;

    chartColors: any = PieChartColors;

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

        if (key >= KeysEnum.CHAR_A && key <= KeysEnum.CHAR_Z) {
            index = key - KeysEnum.CHAR_A;
        } else if (key >= KeysEnum.CHAR_A_SMALL && key <= KeysEnum.CHAR_Z_SMALL) {
            index = key - KeysEnum.CHAR_A_SMALL;
        }

        if (index >= 0 && !me.showNav) {
            index = index + KeysEnum.CHAR_A;
            var el = $('.availableChars #char-' + key);
            if (el && !$(el).hasClass('selected')) {
                $(el).click();
            }
        }

        if (key == KeysEnum.ENTER && me.showNav && me.isNextQuestion()) {
            me.nextQuestion();
        }

        if (key == KeysEnum.SPACE && me.showNav) {
            me.ttsPlay();
        }
    }

    startGame() {
        let me = this;
        me.parameters.userId = me.userId;
        me.gamesService.initializeGameHangman(me.parameters, me.initializeGame, me);
    }

    initializeGame(data: any) {
        let me = this;
        me.questions = data;

        if (me.questions.length > 0) {
            me.gameSessionId = me.questions[0].gameSessionId;
            me.questions = me.gamesHelper.shuffle(me.questions);
            me.questionsCount = me.questions.length;
            me.prepareCharacters();

            me.nextQuestion();
        } else {
            me._toast.warning('Wybrany słownik nie zawiera słówek. Wybierz inny słownik.');
        }
    }

    isNextQuestion() {
        let me = this;
        return me.questions.length > 0;
    }

    nextQuestion() {
        let me = this;
        $('.animation').removeClass('up').addClass('down');

        setTimeout(function () {
            me.showNav = false;
            me.questionIndex++;

            me.model = me.questions.shift();
            me.incrementSize = 1; // przy wiekszych slownikach nie da sie odgadnac slowek. zawsze zabiera po jednym zyciu
            // me.incrementSize = me.model.translation.secondLangWord.length > me.wordLengthLimit ? 1 : 2;
            me.prepareAnswerMask();
            me.startTime = new Date().getTime();
            me.usedCharacters = [];
            me.wrongAnswers = 0;

            $('.availableChars label').removeClass('selected');
            $('.animation').removeClass('down').addClass('up');
            $('.hangmanArea').removeClass('wrong correct');

            me.interval = setInterval(() => {
                me.diffTime = me.liveTime();
            }, me.updateTimeInterval);

        }, me.animationTime);
    }

    prepareCharacters(): void {
        let me = this;
        let tempChars = me.gamesHelper.getAlphabet();

        for (let q = 0; q < me.questions.length; q++) {
            let upperWord = me.questions[q].translation.secondLangWord.toUpperCase();
            for (let i = 0; i < upperWord.length; i++) {
                if (me.gamesHelper.isLetter(upperWord[i])) {
                    tempChars.push(upperWord[i]);
                }
            }
        }
        me.availableCharacters = me.gamesHelper.uniqueArray(tempChars).sort();
    }

    prepareAnswerMask(): void {
        let me = this;
        let upperWord = me.model.translation.secondLangWord.toUpperCase();
        me.answerMask = [];
        for (let i = 0; i < upperWord.length; i++) {
            me.answerMask.push(me.gamesHelper.isLetter(upperWord[i]) ? me.letterMask : upperWord[i]);
        }
    }

    confirmAnswer(char: string, event: Event) {
        let me = this;

        if (!me.usedCharacters[char] || me.usedCharacters[char] == false) {
            var check = me.model.translation.secondLangWord.toUpperCase().indexOf(char) >= 0;
            if (check) {
                me.fillAnswer(char);
            } else {
                me.usedCharacters[char] = true;
                me.wrongAnswers += me.incrementSize;
            }
        }

        $(event.target).blur();
        $(event.target).addClass('selected');

        let checkAnswer = me.model.translation.secondLangWord.toUpperCase() == me.answerMask.join('').toUpperCase();

        if (me.wrongAnswers >= me.hangmanParts || checkAnswer) {
            me.completeAnswer(checkAnswer);
        }
    }

    completeAnswer(correct: boolean) {
        let me = this;
        me.endTime = new Date().getTime();
        if (me.interval) {
            clearInterval(me.interval);
        }
        me.answers.push(new AnswerUpdateModel(
            me.model.gameSessionId,
            me.model.translation.id,
            correct,
            me.calculateDuration()
        ));

        $('.hangmanArea').addClass(correct ? 'correct' : 'wrong');

        me.showNav = true;
        me.ttsPlay();
    }

    fillAnswer(char: string) {
        let me = this;
        let upperWord = me.model.translation.secondLangWord.toUpperCase();
        for (let i = 0; i < upperWord.length; i++) {
            if (upperWord[i] == char) {
                me.answerMask[i] = char;
            }
        }
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
