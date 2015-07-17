using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetsDo.DataAccess.Entities
{
    [Table("Issue")]
    public class Issue
    {
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedTime { get; set; }

        public string Text { get; set; }

        public bool IsFinished { get; set; }

        // Navigation property 
        public int? CategoryId { get; set; }
        //public virtual ICollection<UnderIssue> UnderIssues { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}