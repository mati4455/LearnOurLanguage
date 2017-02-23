import { LanguageModel } from './language.model';
import { TranslationModel } from './translation.model';

export class DictionaryVoModel {

    constructor(
        public id = 0,
        public name = '',
        public description = '',
        public Public = false,
        public Date: number = null,
        public userId = 0,
        public firstLanguage: LanguageModel = null,
        public secondLanguage: LanguageModel = null,
        public translationsList: Array<TranslationModel> = []
    ) { }

}