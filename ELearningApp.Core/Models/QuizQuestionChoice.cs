using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class QuizQuestionChoice
    {
        public int Id { get; set; }
        public string Choice { get; set; } = string.Empty;
        public bool IsAnswer { get; set; } = false;

        public int QuizQuestionId { get; set; }
        public QuizQuestion? Question { get; set; }
    }
}
