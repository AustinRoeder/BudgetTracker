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

        // GET: api/BudgetItems/HouseBudgetItems
        [Route("HouseBudgetItems")]
        public IQueryable<BudgetItem> GetBudgetItems(int hhid)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return db.Households.Where(h => h.Id == user.HouseholdId && h.Id == hhid).FirstOrDefault().BudgetItems.AsQueryable();
        }

        // GET: api/BudgetItems/Find/1
        [Route("Find")]
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
        [Route("Edit")]
        [ResponseType(typeof(BudgetItem))]
        public async Task<IHttpActionResult> EditBudgetItem(int id, decimal? a, int? catId, int? freq, string name)
        {
            var budgetItem = db.BudgetItems.Find(id);

            if (a != null)
                budgetItem.Amount = (decimal)a;
            if (catId != null)
                budgetItem.CategoryId = (int)catId;
            if (freq != null)
                budgetItem.Frequency = (int)freq;
            if (!String.IsNullOrWhiteSpace(name))
                budgetItem.Name = name;

            await db.SaveChangesAsync();
            return Ok(budgetItem);
        }

        // POST: api/BudgetItems/Create
        [Route("Create")]
        [ResponseType(typeof(BudgetItem))]
        public async Task<IHttpActionResult> CreateBudgetItem(BudgetItem budgetItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BudgetItems.Add(budgetItem);
            await db.SaveChangesAsync();

            return Ok(budgetItem);
        }

        // DELETE: api/BudgetItems/Delete?id=5
        [Route("Delete")]
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