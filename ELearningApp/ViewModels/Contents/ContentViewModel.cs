using ELearningApp.Core.Models;

namespace ELearningApp.ViewModels.Contents
{
    public class ContentViewModel
    {
        public Content? Content { get; set; }
        public List<Content>? Contents { get; set; }

        public int? NextContentId { get; set; }
        public int? PreviousContentId { get; set; }

        public int UserCourseId { get; set; }
    }
}
