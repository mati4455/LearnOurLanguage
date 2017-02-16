namespace Model.Models.Games
{
    public class AnswerUpdateModel
    {
        public int GameSessionId { get; set; }
        public int TranslationId { get; set; }
        public bool Correct { get; set; }
        public decimal Duration { get; set; }
    }
}
