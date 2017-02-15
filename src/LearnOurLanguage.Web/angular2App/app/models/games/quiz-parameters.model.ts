import { BaseGameParameters } from './base-game-parameters.model';

export class QuizParameters extends BaseGameParameters {

    constructor(
        public userId = 0,
        public dictionaryId = 0,
        public maxNumberOfAnswers = 0,
        public maxNumberOfQuestions = 0,
        public maxNumberOfRepeats = 0
    ) {
        super(userId, dictionaryId);
    }

}