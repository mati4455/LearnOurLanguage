import { MemoVoModel } from './memo-vo.model';
export class MemoQuestionModel {

    constructor(
        public gameSessionId: number = 0,
        public answers: Array<MemoVoModel> = []
    ) { }

}