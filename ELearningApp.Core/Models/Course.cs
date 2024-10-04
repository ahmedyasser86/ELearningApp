using ELearningApp.Core.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CourseStatus Status { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string InstructorId { get; set; } = string.Empty;
        public ApplicationUser? Instructor { get; set; }

        public ICollection<Content>? Contents { get; set; }

        [NotMapped]
        public double Duration
        {
            get
            {
                return Contents == null ? 0 : Contents.Sum(m => m.Duration);
            }
        }
    }
}
