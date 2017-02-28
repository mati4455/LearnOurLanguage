import { TranslationModel } from '../dictionary/translation.model';
export class MemoModel {

    constructor(
        public gameSessionId: number = 0,
        public translations: Array<TranslationModel> = null
    ) { }

}