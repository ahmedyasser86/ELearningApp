
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Course>? CoursesAsInstructor  { get; set; }

        public virtual ICollection<UserCourse>? Courses { get; set; }

        public virtual ICollection<UserProgress>? Progresses { get; set; }
    }
}
