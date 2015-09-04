using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARBudgetTracker.Models
{
    public class Account
    {
        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool IsArchived { get; set; }

        public virtual Household Household { get; set; }

        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}