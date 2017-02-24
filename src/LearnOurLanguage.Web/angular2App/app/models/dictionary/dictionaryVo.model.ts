import { LanguageModel } from './language.model';
import { TranslationModel } from './translation.model';

export class DictionaryVoModel {

    constructor(
        public id = 0,
        public name = '',
        public description = '',
        public Public = false,
        public date: Date = null,
        public userId = 0,
        public firstLanguageId: number = null,
        public secondLanguageId: number = null,
        public translationsList: Array<TranslationModel> = []
    ) { }

}