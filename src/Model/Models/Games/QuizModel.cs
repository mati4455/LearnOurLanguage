using System.Collections.Generic;
using Model.Models.Database;

namespace Model.Models.Games
{
    public class QuizModel
    {
        public int GameSessionTranslationId { get; set; }
        public Translation Translation { get; set; }
        public IList<string> Answers { get; set; }
    }
}
