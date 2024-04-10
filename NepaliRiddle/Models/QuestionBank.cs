namespace NepaliRiddle.Models
{
    public class QuestionBank
    {
        public string Question { get; set; } =null!;
        public List<string> Options { get; set; }
        public string Answer { get; set; } = null!;
        public string SelectedOption { get; set; } = "";
        public bool AnswerSelected { get; set; }
        public bool CorrectAnswerSelected { get; set; }
    }
}
