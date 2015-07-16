using ASP.NET_MVC5_Bootstrap3_3_1_LESS.Models;

namespace LetsDo.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASP.NET_MVC5_Bootstrap3_3_1_LESS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ASP.NET_MVC5_Bootstrap3_3_1_LESS.Models.ApplicationDbContext";
        }

        protected override void Seed(ASP.NET_MVC5_Bootstrap3_3_1_LESS.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Categories.AddOrUpdate(n => n.Name,
                new Category() {Name = "Everythhik"});
        }
    }
}
