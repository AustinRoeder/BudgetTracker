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

namespace ARBudgetTracker.Controllers
{
    public class HouseholdAccountsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/HouseholdAccounts
        public IQueryable<HouseholdAccount> GetHouseholdAccounts()
        {
            return db.HouseholdAccounts;
        }

        // GET: api/HouseholdAccounts/5
        [ResponseType(typeof(HouseholdAccount))]
        public async Task<IHttpActionResult> GetHouseholdAccount(int id)
        {
            HouseholdAccount householdAccount = await db.HouseholdAccounts.FindAsync(id);
            if (householdAccount == null)
            {
                return NotFound();
            }

            return Ok(householdAccount);
        }

        // PUT: api/HouseholdAccounts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHouseholdAccount(int id, HouseholdAccount householdAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != householdAccount.Id)
            {
                return BadRequest();
            }

            db.Entry(householdAccount).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseholdAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/HouseholdAccounts
        [ResponseType(typeof(HouseholdAccount))]
        public async Task<IHttpActionResult> PostHouseholdAccount(HouseholdAccount householdAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HouseholdAccounts.Add(householdAccount);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = householdAccount.Id }, householdAccount);
        }

        // DELETE: api/HouseholdAccounts/5
        [ResponseType(typeof(HouseholdAccount))]
        public async Task<IHttpActionResult> DeleteHouseholdAccount(int id)
        {
            HouseholdAccount householdAccount = await db.HouseholdAccounts.FindAsync(id);
            if (householdAccount == null)
            {
                return NotFound();
            }

            db.HouseholdAccounts.Remove(householdAccount);
            await db.SaveChangesAsync();

            return Ok(householdAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HouseholdAccountExists(int id)
        {
            return db.HouseholdAccounts.Count(e => e.Id == id) > 0;
        }
    }
}