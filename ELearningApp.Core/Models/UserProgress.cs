using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class UserProgress
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int ContentId { get; set; }
        public virtual Content? Content { get; set; }
    }
}
