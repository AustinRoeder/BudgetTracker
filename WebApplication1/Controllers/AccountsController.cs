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
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Accounts/HouseAccounts
        [Route("HouseAccounts")]
        public IQueryable<Account> GetAccounts(int hId)
        {
            var accounts = db.Accounts.Where(a=>a.IsArchived == false &&
                                                                a.HouseholdId == hId);
            return accounts;
        }

        // GET: api/Accounts/Find
        [Route("Find")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> GetAccount(int id)
        {
            Account Account = await db.Accounts.FindAsync(id);
            if (Account == null)
            {
                return NotFound();
            }
            if (Account.IsArchived)
                return Ok("This account has been archived.");
            return Ok(Account);
        }

        // api/Accounts/Adjust
        [Route("Adjust")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> AdjustAccount(int id, decimal newBalance)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.Accounts.Where(a => a.HouseholdId == user.HouseholdId &&
                                                                                a.Id == id).FirstOrDefault();
            var diff = newBalance - account.Balance;
            if (diff != 0)
            {
                db.Transactions.Add(new Transaction()
                {
                    Description = "Balance Adjusted",
                    Amount = diff,
                    CategoryId = 3,
                    AccountId = id
                });
            }
            account.Balance += diff;
            await db.SaveChangesAsync();
            return Ok(account);
        }

        // PUT: api/Accounts/Archive?id
        [Route("Archive")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> ArchiveAccount(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.Accounts.Find(id);
            account.IsArchived = true;
            foreach (var trans in account.Transactions)
            {
                trans.IsArchived = true;
            }
            await db.SaveChangesAsync();
            return Ok(user.Household);
        }

        // PUT: api/Accounts/Edit?id&name
        [Route("Edit")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> EditAccount(int id, string name)
        {
            var Account = db.Accounts.Find(id);
            if (!String.IsNullOrWhiteSpace(name))
            {
                Account.Name = name;
            }
            else
                return Ok("No property to edit was given.");
            await db.SaveChangesAsync();
            return Ok(Account);
        }

        // POST: api/Accounts/Create
        [Route("Create")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> CreateAccount(string name)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var Account = new Account()
            {
                Name = name,
                Balance = 0,
                HouseholdId = (int)user.HouseholdId
            };

            db.Accounts.Add(Account);
            await db.SaveChangesAsync();

            return Ok(Account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.Id == id) > 0;
        }
    }
}