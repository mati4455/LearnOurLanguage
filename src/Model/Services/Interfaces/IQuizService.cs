using System.Collections.Generic;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IQuizService
    {
        IList<QuizModel> InitializeQuestions(QuizParameters param);
    }
}
