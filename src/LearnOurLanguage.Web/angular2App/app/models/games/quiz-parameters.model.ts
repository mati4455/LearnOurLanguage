import { BaseGameParameters } from './base-game-parameters.model';

export class QuizParameters {

    constructor(
        public userId = 0,
        public dictionaryId = 0,
        public maxNumberOfAnswers = 5,
        public maxNumberOfQuestions = 5,
        public maxNumberOfRepeats = 3
    ) {
    }

}