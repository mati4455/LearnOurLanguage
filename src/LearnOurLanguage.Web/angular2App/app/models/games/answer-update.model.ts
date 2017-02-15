export class AnswerUpdateModel {

    constructor(
        public gameSessionTranslationId = 0,
        public correct = false,
        public duration = 0
    ) { }

}