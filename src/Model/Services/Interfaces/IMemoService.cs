using System.Collections.Generic;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IMemoService
    {
        IList<MemoModel> InitializeQuestions(MemoParameters param);
    }
}
