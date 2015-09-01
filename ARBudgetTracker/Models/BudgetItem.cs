using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARBudgetTracker.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int Frequency { get; set; }
        public int CategoryId { get; set; }
        public int HouseholdId { get; set; }
        public decimal Amount { get; set; }

        public virtual Category Category { get; set; }
        public virtual Household Household { get; set; }
    }
}