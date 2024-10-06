using ELearningApp.Core.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class Content
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Body { get; set; }
        public int OrderNumber { get; set; }
        public ContentType ContentType { get; set; }
        public double Duration { get; set; }
        public bool HasQuiz { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public Quiz? Quiz { get; set; }

        public virtual ICollection<UserProgress>? UserProgresses { get; set; }

        [NotMapped]
        public bool IsWatched { get; set; }
    }
}
