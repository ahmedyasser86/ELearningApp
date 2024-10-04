using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class UserQuiz
    {
        public int Id { get; set; }
        public double Degree { get; set; }
        public double FullMark { get; set; }
        public DateTime StartDate { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }
    }
}
