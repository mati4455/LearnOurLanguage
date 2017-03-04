using System.Collections.Generic;
using Model.Models.Database;

namespace Model.Models.Games
{
    public class MemoModel
    {
        public int GameSessionId { get; set; }
        public IList<Translation> Translations { get; set; }
    }
}
