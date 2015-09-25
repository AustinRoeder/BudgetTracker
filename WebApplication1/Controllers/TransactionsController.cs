using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ARBudgetTracker.Models;
using Microsoft.AspNet.Identity;

namespace ARBudgetTracker.Controllers
{
    [Authorize]
    [RoutePrefix("api/Transactions")]
    public class TransactionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public class Options
        {
            public string Description { get; set; }
            public decimal? Amount { get; set; }
            public bool IsReconciled { get; set; }
            public int? CategoryId { get; set; }
            public bool IsDebit { get; set; }
        }

        [HttpPost, Route("RecentTransactions")]
        [ResponseType(typeof(List<Transaction>))]
        public async  Task<IHttpActionResult> RecentTransactions()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            string[] arr = {"Edited Balance", "Account Created"};
            var trans = await db.Transactions.Where(t=>t.Account.HouseholdId == user.HouseholdId && !t.IsArchived && !arr.Contains(t.Category.Name)).OrderByDescending(t=>t.Created).Take(5).ToListAsync();
            var models = new List<object>();
            foreach (var item in trans)
            {
                models.Add(new
                {
                    Created = item.Created,
                    AccountName = item.Account.Name,
                    Amount = item.Amount,
                    Desc = item.Description
                });
            }
                        
            return Ok(models);
        }

        // GET: api/Transactions/AccountTransactions
        [HttpPost, Route("AccountTransactions")]
        public IQueryable<Transaction> AccountTransactions(int aId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var trans = db.Accounts.Where(a=>a.HouseholdId == user.HouseholdId && a.Id == aId).FirstOrDefault().Transactions.AsQueryable();
            return trans;
        }

        // GET: api/Transactions/Find/5
        [HttpPost, Route("Find")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> GetTransaction(int id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            if (transaction.IsArchived)
            {
                return Ok("This transaction has been archived.");
            }
            return Ok(transaction);
        }

        // GET: api/Transactions/FindByCategory?categoryName=""
        [HttpPost, Route("FindByCategory")]
        [ResponseType(typeof(List<Transaction>))]
        public IHttpActionResult FindTransaction(string categoryName)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return Ok(db.Transactions.Where(t => t.Category.Name == categoryName).ToList());
        }

        // PUT: api/Transactions/Edit/5
        [HttpPost, Route("Edit")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> EditTransaction(int id, Options options)
        {
            var transaction = await db.Transactions.FindAsync(id);
            transaction.Created = DateTimeOffset.Now;
            options.IsReconciled = !options.IsReconciled;
            if (options.IsDebit)
                options.Amount *= -1;
            if (options.CategoryId == null && options.Amount == null && String.IsNullOrWhiteSpace(options.Description) && options.IsReconciled == transaction.IsReconciled)
            {
                return Ok("No property to edit was given.");
            }
            else
            {
                FindEdited(options.Description, options.Amount, options.CategoryId, options.IsReconciled, transaction);
                transaction.Updated = DateTimeOffset.Now;
            }
            transaction.Category = null;
            await db.SaveChangesAsync();
            return Ok(transaction);
        }

        private static void FindEdited(string desc, decimal? a, int? catId, bool rec, Transaction transaction)
        {
            if (!String.IsNullOrWhiteSpace(desc))
                transaction.Description = desc;
            if (a != null)
            {
                transaction.Account.Balance -= transaction.Amount;
                transaction.Account.RecBalance -= transaction.Amount;
                transaction.Amount = (decimal)a;
                transaction.Account.Balance += (decimal)a;
                transaction.Account.RecBalance += (decimal)a;
            }
            if (catId != null)
                transaction.CategoryId = (int)catId;
            if (transaction.IsReconciled != rec)
            {
                transaction.IsReconciled = rec;
                if (!rec)
                    transaction.Account.RecBalance -= (decimal)a;
                else
                    transaction.Account.RecBalance += (decimal)a;
            }
        }

        // POST: api/Transactions/Create
        [HttpPost, Route("Create")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> CreateTransaction(Transaction transaction)
        {
            transaction.Created = DateTimeOffset.Now;
           // transaction.CategoryId = transaction.Category.Id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (transaction.IsDebit)
                transaction.Amount *= -1;
            transaction.CategoryId = transaction.Category.Id;
            transaction.Category = null;
            var account = db.Accounts.Find(transaction.AccountId);
            account.Balance += transaction.Amount;
            if(transaction.IsReconciled)
                account.RecBalance += transaction.Amount;
            account.Transactions.Add(transaction);

            await db.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Transactions/Delete/5
        [HttpPost, Route("Delete")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> DeleteTransaction(int id)
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            var account = transaction.Account;
            if (transaction == null)
            {
                return NotFound();
            }
            account.Balance -= transaction.Amount;
            if (transaction.IsReconciled)
                account.RecBalance -= transaction.Amount;
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Id == id) > 0;
        }
    }
}