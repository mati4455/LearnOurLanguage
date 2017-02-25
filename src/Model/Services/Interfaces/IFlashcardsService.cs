using System.Collections.Generic;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IFlashcardsService
    {
        IList<FlashcardsModel> InitializeQuestions(FlashcardsParameters param);
    }
}