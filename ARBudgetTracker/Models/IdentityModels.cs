﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ARBudgetTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string DisplayName { get; set; }

        public int HouseholdId { get; set; }

        public virtual Household Household { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public System.Data.Entity.DbSet<ARBudgetTracker.Models.BudgetItem> BudgetItems { get; set; }

        public System.Data.Entity.DbSet<ARBudgetTracker.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<ARBudgetTracker.Models.Household> Households { get; set; }

        public System.Data.Entity.DbSet<ARBudgetTracker.Models.Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<ARBudgetTracker.Models.HouseholdAccount> HouseholdAccounts { get; set; }
    }
}