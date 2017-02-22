using System.Collections.Generic;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IHangmanService
    {
        IList<HangmanModel> InitializeQuestions(HangmanParameters param);
    }
}
