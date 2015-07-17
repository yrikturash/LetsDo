using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsDo.DataAccess.Entities
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        // Navigation property 
        //public virtual ICollection<Issue> Issues { get; set; } 

    }
}
