using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARBudgetTracker.Models
{
    public class Household
    {
        public Household()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        [JsonIgnore]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}