import { DictionaryModel } from './dictionary.model';
export class TranslationModel {

    constructor(
        public dictionary: DictionaryModel = null,
        public firstLangWord = '',
        public secondLangWord = ''
    ) { }

}