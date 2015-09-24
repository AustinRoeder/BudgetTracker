using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARBudgetTracker.Models.DashModels
{
    public class HouseholdVM
    {
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Account> Accounts { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }
    }
    public class AccountVM
    {
        public List<Transaction> Transactions { get; set; }
    }
}