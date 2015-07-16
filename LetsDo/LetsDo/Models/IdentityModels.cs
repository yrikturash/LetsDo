using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASP.NET_MVC5_Bootstrap3_3_1_LESS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cellphone { get; set; }
        public string Password { get; set; }
        public Boolean IsEnabled { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<UnderIssue> UnderIssues { get; set; }
        public DbSet<Category> Categories { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        

    }

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

    [Table("UnderIssue")]
    public class UnderIssue
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }
        public bool IsFinished { get; set; }

        public int IssueId { get; set; }
        // Navigation properties 
        [ForeignKey("IssueId")]
        public virtual Issue Issue { get; set; } 

    }

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