using System.Collections.Generic;

namespace LetsDo.DTO.ViewModels
{
    public class IndexViewModel
    {
        public IList<Issue> Issues { get; set; }
        public IList<UnderIssue> UnderIssues { get; set; }
        public IList<Category> CategoriesList { get; set; }
        public int? ActiveCategoryId { get; set; }
    }
}