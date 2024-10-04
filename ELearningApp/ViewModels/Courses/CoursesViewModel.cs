﻿using ELearningApp.Core.Assists;
using ELearningApp.Core.Models;

namespace ELearningApp.ViewModels.Courses
{
    public class CoursesViewModel
    {
        public List<Category> Categories { get; set; } = [];

        public PaginatedList<Course>? Courses { get; set; }
    }
}