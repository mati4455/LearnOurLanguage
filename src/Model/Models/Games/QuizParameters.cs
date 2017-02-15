namespace Model.Models.Games
{
    public class QuizParameters : BaseGameParameters
    {
        public int MaxNumberOfAnswers { get; set; }
        public int MaxNumberOfQuestions { get; set; }
        public int MaxNumberOfRepeats { get; set; }
    }
}
