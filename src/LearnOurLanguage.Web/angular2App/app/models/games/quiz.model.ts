import { TranslationModel } from '../dictionary/translation.model';
export class QuizModel {

    constructor(
        public gameSessionTranslationId = 0,
        public translation: TranslationModel = null,
        public answers: Array<string> = null
    ) { }

}