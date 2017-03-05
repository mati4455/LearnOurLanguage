using System.Collections.Generic;
using Model.Models.Database;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IGamesService
    {
        IList<Translation> GetDictionaryTranslations(int dictionaryId);
        IList<Translation> GetTranslationsById(IEnumerable<int> ids, bool reverse);
        IList<QuestionPair> InitializeGame(int dictionaryId, int userId, GamesEnum gameId, int count);
        GameSession InitializeBaseGame(int dictionaryId, int userId, GamesEnum gameId);
        bool InsertAnswers(IList<AnswerUpdateModel> answers);
        void FinishGameSession(int gameSessionId);
        IList<Translation> ReverseTranslations(IEnumerable<Translation> translations);
    }
}
