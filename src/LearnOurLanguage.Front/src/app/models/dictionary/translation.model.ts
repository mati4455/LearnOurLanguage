import { DictionaryModel } from './dictionary.model';
export class TranslationModel {

    constructor(
        public id = 0,
        public dictionaryId = 0,
        public dictionary: DictionaryModel = null,
        public firstLangWord = '',
        public secondLangWord = ''
    ) { }

}