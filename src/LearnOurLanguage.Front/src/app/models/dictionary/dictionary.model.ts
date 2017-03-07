import { LanguageModel } from './language.model';

export class DictionaryModel {

    constructor(
        public id = 0,
        public parentDictionaryId = 0,
        public name = '',
        public description = '',
        public isPublic = false,
        public date: Date = null,
        public userId = 0,
        public firstLanguage: LanguageModel = null,
        public secondLanguage: LanguageModel = null
    ) { }

}