﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARBudgetTracker.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public bool IsDebit { get; set; }
        public bool IsArchived { get; set; }
        public bool IsReconciled { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual Account Account { get; set; }
    }
}