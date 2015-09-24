namespace ARBudgetTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ARBudgetTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ARBudgetTracker.Models.ApplicationDbContext context)
        {
            if (!context.Categories.Any(c => c.Name == "Account Created"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Account Created",
                });
            }
            if (!context.Categories.Any(c => c.Name == "Edited Balance"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Edited Balance",
                });
            }
            if (!context.Categories.Any(c => c.Name == "Salary"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Salary",
                });
            }
            if (!context.Categories.Any(c => c.Name == "Food"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Food",
                });
            }
            if (!context.Categories.Any(c => c.Name == "Rent | Mortgage"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Rent | Mortgage",
                });
            }

            if (!context.Categories.Any(c => c.Name == "Utilities"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Utilities",
                });
            }
            if (!context.Categories.Any(c => c.Name == "Gas"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Gas",
                });
            }
            if (!context.Categories.Any(c => c.Name == "Misc"))
            {
                context.Categories.Add(new Models.Category
                {
                    Name = "Misc",
                });
            }
        }
    }
}
