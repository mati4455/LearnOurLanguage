using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.Models.Games;

namespace Model.Services.Interfaces
{
    public interface IMemoService 
    {
        IList<MemoModel> InitializeQuestions(MemoParameters param);
    }
}
