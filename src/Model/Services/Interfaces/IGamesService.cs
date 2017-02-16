using System.Collections.Generic;
using Model.Models.Database;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IGamesService
    {
        IList<Translation> GetDictionaryTranslations(int dictionaryId);
        IList<QuestionPair> InitializeGame(int dictionaryId, int userId, GamesEnum gameId, int count);
        bool InsertAnswers(IList<AnswerUpdateModel> answers);
        void FinishGameSession(int gameSessionId);
    }
}
