﻿using ELearningApp.Core.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningApp.Core.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public CourseStatus Status { get; set; } = CourseStatus.Pending;
        public string? ImagePath { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string InstructorId { get; set; } = string.Empty;
        public ApplicationUser? Instructor { get; set; }

        public List<Content>? Contents { get; set; }
        public ICollection<UserCourse>? Students { get; set; }

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
