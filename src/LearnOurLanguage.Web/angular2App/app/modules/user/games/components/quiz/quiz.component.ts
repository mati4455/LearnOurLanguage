import { DictionaryModel, QuizModel, QuizParameters, 
         AnswerUpdateModel, PieChartData } from 'lol/models';
import { GamesService, DictionariesService, ChartsService } from 'lol/services';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import './quiz.scss';

var $ = require('jquery');

@Component({
    selector: 'game-quiz',
    templateUrl: 'quiz.component.html',
    providers: [
        GamesService,
        DictionariesService,
        ChartsService
    ]
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

    startTime: number = 0;
    endTime: number = 0;
    diffTime: number = 0;
    interval: any = null;

    animationTime: number = 300;

    constructor(
        private dictionariesService: DictionariesService, 
        private chartsService: ChartsService, 
        private gamesService: GamesService, 
        public router: Router) { }

    ngOnInit() {
        let me = this;
        me.userId = +localStorage.getItem('userId');
        me.dictionariesService.getForUser(me.userId, me.loadDictionaries, me);
    }

    ngOnDestroy() {
        let me = this;
        if (!me.saved) {
            me.endSession(false);
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

        me.nextQuestion();
    }

    confirmAnswer(answer: string, event: Event) {
        let me = this;
        me.endTime = new Date().getTime();
        if (me.interval) clearInterval(me.interval);

        let correct = me.equalsWords(me.model.translation.secondLangWord, answer);
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
        me.shuffle(me.questions);

        $('.animation').css({
            'opacity': '0',
            'transform': 'scale(0)'
        });

        setTimeout(function(){ 
            
            $('.answers button').removeClass('correct wrong');
            me.showNav = false;
            me.questionIndex++;
            me.model = me.questions.shift();        
            me.shuffle(me.model.answers);
            me.startTime = new Date().getTime();

            $('.animation').css({
                'opacity': '1',
                'transform': 'scale(1)'
            });

            me.interval = setInterval(() => {
                me.diffTime = me.liveTime();
            }, 100);

        }, me.animationTime);

    }

    endSession(loadStats: boolean) {
        let me = this;
        me.model = null;
        
        if (loadStats) {
            me.gamesService.insertAnswers(me.answers, me.getStatistics, me);
            me.gamesService.finishGameSession(me.gameSessionId, () => { }, me);
        }
    }

    getStatistics(data: any) {
        let me = this;
        if (!data) return;       

        // załadowanie statystyk
        me.chartsService.getStatisticsForGameSession(me.gameSessionId, me.showCharts, me);
    }

    showCharts(data: any) {
        let me = this;
        me.stats = data;
    }

    equalsWords(s1: string, s2: string) {
        let me = this;
        return s1.toUpperCase() == s2.toUpperCase();
    }

    calculateDuration() {
        let me = this;
        return Math.round((me.endTime - me.startTime) / 1000 * 100) / 100;
    }

    liveTime() {
        let me = this;
        me.endTime = new Date().getTime();
        return me.calculateDuration();
    }

    shuffle(a: any) {
        let j: number;
        let x: any;
        let i: number;
        for (i = a.length; i; i--) {
            j = Math.floor(Math.random() * i);
            x = a[i - 1];
            a[i - 1] = a[j];
            a[j] = x;
        }
    }

    ttsPlay() {
        let me = this;
        let voice = new SpeechSynthesisUtterance(me.model.translation.secondLangWord);
        voice.lang = me.model.translation.dictionary.secondLanguage.code;
        voice.rate = 0.7;
        window.speechSynthesis.speak(voice);
    }
}
