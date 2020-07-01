using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SQLappTheSequel.Models;
using System.Web.ModelBinding;

namespace SQLappTheSequel.Controllers
{
    public class SubcategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subcategories
        public async Task<ActionResult> Index()
        {
            return View(await db.Subcategories.ToListAsync());
        }

        // GET: Subcategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        // GET: Subcategories/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Categories = await db.Categories.ToListAsync();
            return View();
        }

        // POST: Subcategories/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Form] Subcategory subcategory, int Category_Id)
        {
            if (ModelState.IsValid)
            {
                subcategory.Category = await db.Categories.FirstOrDefaultAsync(t => t.Id == Category_Id);
                db.Subcategories.Add(subcategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subcategory);
        }

        // GET: Subcategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Categories = await db.Categories.ToListAsync();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        // POST: Subcategories/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Form] Subcategory subcategory, int Category_Id)
        {
            if (ModelState.IsValid)
            {
                subcategory.Category = await db.Categories.FirstOrDefaultAsync(t => t.Id == Category_Id);
                db.Entry(subcategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subcategory);
        }

        // GET: Subcategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }

        // POST: Subcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Subcategory subcategory = await db.Subcategories.FindAsync(id);
            db.Subcategories.Remove(subcategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
