import { Injectable } from '@angular/core';

@Injectable()
export class GamesHelper {

    public speechSupport = 'speechSynthesis' in window && 'SpeechSynthesisUtterance' in window;
    private window = (<any>window);

    ttsPlay(word: string, lang: string) {
        let me = this;
        if (!me.speechSupport) {
            return;
        }

        let voice = new me.window.SpeechSynthesisUtterance();
        voice.text = word;
        voice.lang = lang;
        voice.rate = 0.8;

        me.window.speechSynthesis.speak(voice);
    }

    shuffle(array: any) {
        let currentIndex = array.length,
            temporaryValue: any,
            randomIndex: any;

        while (0 !== currentIndex) {
            randomIndex = Math.floor(Math.random() * currentIndex);
            currentIndex -= 1;

            temporaryValue = array[currentIndex];
            array[currentIndex] = array[randomIndex];
            array[randomIndex] = temporaryValue;
        }

        return array;
    }

    equalsWords(s1: string, s2: string) {
        let me = this;
        return s1.toUpperCase() == s2.toUpperCase();
    }

    calculateDuration(d1: number, d2: number) {
        let me = this;
        return Math.round((d2 - d1) / 1000 * 100) / 100;
    }

}