using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Appcode.Application.QuestionModelu
{
    public class QuestionsViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Questions { get; set; }
        public string Answer { get; set; }
    }
}
