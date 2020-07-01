using SQLappTheSequel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SQLappTheSequel.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Index
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }
        // GET: Cat/id
        public async Task<ActionResult> Cat(int? id)
        {
            /*
            var cats = Context.transaction_category
              .Where(p => p.account_portfolio_id == portfolioId && p.deleted == null)
              .Include(p => p.SubCategories);*/

            //Category category = await db.Categories.FindAsync(id);
            var subcategories = await db.Subcategories.Where(s => s.Category.Id == id).Include(s=> s.Category).ToListAsync();
            return View(subcategories);
        }
        // GET: SubCat/id
        public async Task<ActionResult> SubCat(int? id)
        {
            var Exercises = await db.Exercises.Where(e => e.Subcategory.Id == id).ToListAsync();
            return View(Exercises);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}