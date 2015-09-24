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
using Microsoft.AspNet.Identity;

namespace ARBudgetTracker.Models
{
    [Authorize]
    [RoutePrefix("api/BudgetItems")]
    public class BudgetItemsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public class Options
        {
            public int Id { get; set; }
            public int? Frequency { get; set; }
            public int? CategoryId { get; set; }
            public string Name { get; set; }
            public decimal? Amount { get; set; }
            public bool isExpense { get; set; }
        }

        // GET: api/BudgetItems/HouseBudgetItems
        [HttpPost, Route("HouseBudgetItems")]
        public object GetBudgetItems()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var budgets = db.Households.Where(h => h.Id == user.HouseholdId).FirstOrDefault().BudgetItems.AsQueryable();
            var model = new { 
                expenses = budgets.Where(b=>b.isExpense),
                incomes = budgets.Where(b=>!b.isExpense)
            };
            return model;
        }

        // GET: api/BudgetItems/Find/1
        [HttpPost, Route("Find")]
        [ResponseType(typeof(BudgetItem))]
        public async Task<IHttpActionResult> FindBudgetItem(int id)
        {
            BudgetItem budgetItem = await db.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return NotFound();
            }

            return Ok(budgetItem);
        }

        // PUT: api/BudgetItems/Edit
        [HttpPost, Route("Edit")]
        [ResponseType(typeof(BudgetItem))]
        public async Task<IHttpActionResult> EditBudgetItem(int id, Options options)
        {
            var budgetItem = db.BudgetItems.Find(options.Id);
            if (options.isExpense != budgetItem.isExpense)
                budgetItem.isExpense = options.isExpense;
            if (options.Amount != budgetItem.Amount)
                budgetItem.Amount = (decimal)options.Amount;
            if (options.CategoryId != budgetItem.CategoryId)
                budgetItem.CategoryId = (int)options.CategoryId;
            if (options.Frequency != budgetItem.Frequency)
                budgetItem.Frequency = (int)options.Frequency;
            if (options.Name != budgetItem.Name)
                budgetItem.Name = options.Name;
            budgetItem.Category = null;
            await db.SaveChangesAsync();
            return Ok(budgetItem);
        }

        // POST: api/BudgetItems/Create
        [HttpPost, Route("Create")]
        [ResponseType(typeof(BudgetItem))]
        public async Task<IHttpActionResult> CreateBudgetItem(BudgetItem budgetItem)
        {
            budgetItem.HouseholdId = (int)db.Users.Find(User.Identity.GetUserId()).HouseholdId;
            var defCats = new List<string>() {"Salary","Rent | Mortgage","Utilities", "Gas", "Misc"};
            var cats = db.Categories.Where(c => defCats.Contains(c.Name)  || c.HouseholdId == budgetItem.HouseholdId);
            if (!cats.Any(c => c.Name == budgetItem.Category.Name))
            {
                var newCat = new Category()
                {
                    HouseholdId = budgetItem.HouseholdId,
                    Name = budgetItem.Category.Name
                };

                db.Categories.Add(newCat);
                await db.SaveChangesAsync();
                budgetItem.Category.Id = newCat.Id;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            budgetItem.CategoryId = budgetItem.Category.Id;
            budgetItem.Category = null;
            db.BudgetItems.Add(budgetItem);
            await db.SaveChangesAsync();

            return Ok(budgetItem);
        }

        // DELETE: api/BudgetItems/Delete?id=5
        [HttpPost, Route("Delete")]
        [ResponseType(typeof(BudgetItem))]
        public async Task<IHttpActionResult> DeleteBudgetItem(int id)
        {
            BudgetItem budgetItem = await db.BudgetItems.FindAsync(id);
            var hh = budgetItem.Household;
            if (budgetItem == null)
            {
                return NotFound();
            }

            db.BudgetItems.Remove(budgetItem);
            await db.SaveChangesAsync();

            return Ok(hh);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BudgetItemExists(int id)
        {
            return db.BudgetItems.Count(e => e.Id == id) > 0;
        }
    }
}