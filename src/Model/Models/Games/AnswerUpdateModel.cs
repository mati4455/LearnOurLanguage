namespace Model.Models.Games
{
    public class AnswerUpdateModel
    {
        public int GameSessionTranslationId { get; set; }
        public bool Correct { get; set; }
        public int Duration { get; set; }
    }
}
