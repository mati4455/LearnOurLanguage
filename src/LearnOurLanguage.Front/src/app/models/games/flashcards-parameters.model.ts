export class FlashcardsParameters {

    constructor(
        public userId = 0,
        public dictionaryId = 0,
        public reverseLangs = false,
        public maxNumberOfQuestions = 10
    ) {
    }

}