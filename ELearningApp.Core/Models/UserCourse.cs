using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
      
        public DateTime EnrollDate { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public List<UserProgress>? Progresses { get; set; }

        [NotMapped]
        public double Progress
        {
            get
            {
                return Progresses == null ? 0 : Progresses.Sum(m => m.Content?.Duration ?? 0);
            }
        }
    }
}
