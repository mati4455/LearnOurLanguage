import { TranslationModel } from '../dictionary/translation.model';
export class HangmanModel {

    constructor(
        public gameSessionId: number = 0,
        public translation: TranslationModel = null
    ) { }

}