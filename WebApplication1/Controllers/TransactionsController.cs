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

        // GET: api/Transactions/AccountTransactions
        [Route("AccountTransactions")]
        public IQueryable<Transaction> AccountTransactions(int aId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var trans = db.Accounts.Where(a=>a.HouseholdId == user.HouseholdId && a.Id == aId).FirstOrDefault().Transactions.AsQueryable();
            return trans;
        }

        // GET: api/Transactions/Find/5
        [Route("Find")]
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

        // GET: api/Transactions/FindByCategory/Salary
        [Route("FindByCategory")]
        [ResponseType(typeof(List<Transaction>))]
        public IHttpActionResult FindTransaction(string categoryName)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return Ok(db.Transactions.Where(t => t.Category.Name == categoryName && user.Household.Accounts.Contains(t.Account)).ToList());
        }

        // PUT: api/Transactions/Edit/5
        [Route("Edit")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> EditTransaction(int id, string desc, decimal? a, int? catId, bool rec)
        {
            var transaction = await db.Transactions.FindAsync(id);
            if (catId == null && a == null && String.IsNullOrWhiteSpace(desc) && rec == transaction.IsReconciled)
            {
                return Ok("No property to edit was given.");
            }
            else
            {
                FindEdited(desc, a, catId, rec, transaction);
                transaction.Updated = DateTimeOffset.Now;
            }
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
                transaction.Amount = (decimal)a;
                transaction.Account.Balance += (decimal)a;
            }
            if (catId != null)
                transaction.CategoryId = (int)catId;
            if (transaction.IsReconciled != rec)
                transaction.IsReconciled = rec;
        }

        // POST: api/Transactions/Create
        [Route("Create")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> CreateTransaction(Transaction transaction)
        {
            transaction.Created = DateTimeOffset.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Transactions.Add(transaction);
            var account = db.Accounts.Find(transaction.AccountId);
            account.Balance += transaction.Amount;

            await db.SaveChangesAsync();
            return Ok(transaction);
        }

        // DELETE: api/Transactions/Delete/5
        [Route("Delete")]
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