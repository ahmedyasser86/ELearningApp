using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public int ContentId { get; set; }
        public Content? Content { get; set; }

        public ICollection<QuizQuestion>? Questions { get; set; }
    }
}
