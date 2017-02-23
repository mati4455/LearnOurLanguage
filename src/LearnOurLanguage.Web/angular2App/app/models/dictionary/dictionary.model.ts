import { LanguageModel } from './language.model';

export class DictionaryModel {

    constructor(
        public id = 0,
        public name = '',
        public description = '',
        public Public = false, // public zarezerwowane - pomyślimy później
        public Date: number = null,
        public userId = 0,
        public firstLanguage: LanguageModel = null,
        public secondLanguage: LanguageModel = null
    ) { }

}