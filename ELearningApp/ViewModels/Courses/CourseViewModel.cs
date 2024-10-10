using ELearningApp.Core.Models;

namespace ELearningApp.ViewModels.Courses
{
    public class CourseViewModel
    {
        public Course? Course { get; set; }
        public List<Category>? Categories { get; set; }
        public object ImageFile { get; internal set; }
    }
}
