import { BaseGameParameters } from './base-game-parameters.model';

export class MemoParametersModel {

    constructor(
        public userId = 0,
        public dictionaryId = 0,
        public numberOfGames = 5,
        public maxNumberOfQuestions = 8
    ) {
    }

}