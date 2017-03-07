export class AnswerUpdateModel {

    constructor(
        public gameSessionId = 0,
        public translationId = 0,
        public correct = false,
        public duration = 0
    ) { }

}