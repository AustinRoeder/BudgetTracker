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
using ARBudgetTracker.Models.DashModels;

namespace ARBudgetTracker.Controllers
{
    [Authorize]
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public class Options
        {
            public string Name { get; set; }
            public decimal? Balance { get; set; }
        }

        // GET: api/Accounts/HouseAccounts
        [HttpPost, Route("HouseAccounts")]
        public IQueryable<Account> GetAccounts()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var accounts = user.Household.Accounts.Where(a => a.IsArchived == false).AsQueryable();
            return accounts;
        }

        // GET: api/Accounts/Find
        [HttpPost, Route("Find")]
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

        // PUT: api/Accounts/Edit?id&name
        [HttpPost, Route("Edit")]
        [ResponseType(typeof(Account))]
        public async Task<IHttpActionResult> EditAccount(int id, Options options)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var account = db.Accounts.Where(a => a.HouseholdId == user.HouseholdId &&
                                                                                a.Id == id).FirstOrDefault();
            if ((options.Name == account.Name || String.IsNullOrWhiteSpace(options.Name)) && (options.Balance == account.Balance || options.Balance == null))
                return Ok("No property to edit was given.");
            if (options.Name != account.Name)
            {
                account.Name = options.Name;
            }

            var editCategory = db.Categories.Where(c => c.Name == "Edited Balance").FirstOrDefault();

            if(editCategory == null)
            {
                return InternalServerError(new Exception("The edited balance category was not found"));
            }

            if (options.Balance != account.Balance)
            {
                var diff = (decimal)(options.Balance - account.Balance);
                db.Transactions.Add(new Transaction()
                {
                    Description = "Balance Adjusted",
                    Amount = diff,
                    CategoryId = editCategory.Id,
                    AccountId = id,
                    IsReconciled = true,
                    Created = DateTimeOffset.Now
                });
                
                account.Balance += diff;
                account.RecBalance += diff;
            }
            await db.SaveChangesAsync();
            return Ok(account);
        }

        // PUT: api/Accounts/Archive?id
        [HttpPost, Route("Archive")]
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


        // POST: api/Accounts/Create
        [HttpPost, Route("Create")]
        [ResponseType(typeof(AccountVM))]
        public async Task<IHttpActionResult> CreateAccount(Options options)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var bal = options.Balance ?? 0;
            var Account = new Account()
            {
                Name = options.Name,
                RecBalance = bal,
                Balance = bal,
                HouseholdId = (int)user.HouseholdId
            };

            var createCategory = db.Categories.Where(c=>c.Name == "Account Created").FirstOrDefault();

            if(createCategory == null)
            {
                return InternalServerError(new Exception("The Account Created category was not found"));
            }

            Account.Transactions.Add(new Transaction
            {
                AccountId = Account.Id,
                Amount = bal,
                CategoryId = createCategory.Id,
                Created = DateTimeOffset.Now,
                Description = "Start-up Balance",
                IsReconciled = true
            });
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