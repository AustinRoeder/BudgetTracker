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
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Categories
        [HttpPost]
        public IQueryable<Category> GetCategories()
        {
            string[] names = { "Edited Balance", "Account Created"};
            var user = db.Users.Find(User.Identity.GetUserId());

            List<Category> array = db.Categories.Where(c => !names.Contains(c.Name) && c.HouseholdId == null).ToList();
            array.AddRange(user.Household.Categories);

            return array.AsQueryable();
        }

        // GET: api/Categories/5
        [HttpPost, ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [HttpPost, ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, string name)
        {
            Category category = await db.Categories.FindAsync(id);
            category.Name = name;
            await db.SaveChangesAsync();
            return Ok(category);
        }

        // POST: api/Categories
        [HttpPost, ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(string name)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            if (!db.Categories.Any(c => c.Name == name && c.HouseholdId == user.HouseholdId))
            {
                Category category = new Category()
                {
                    Name = name,
                };
                db.Categories.Add(category);
                await db.SaveChangesAsync();
            }

            return Ok();
        }

        // DELETE: api/Categories/5
        [HttpPost, ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}