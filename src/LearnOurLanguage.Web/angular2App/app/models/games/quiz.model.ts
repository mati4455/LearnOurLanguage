import { TranslationModel } from '../dictionary/translation.model';
export class QuizModel {

    constructor(
        public gameSessionId: number = 0,
        public translation: TranslationModel = null,
        public answers: Array<string> = null
    ) { }

}