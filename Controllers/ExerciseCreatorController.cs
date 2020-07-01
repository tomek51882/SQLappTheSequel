using Microsoft.AspNet.Identity;
using SQLappTheSequel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace SQLappTheSequel.Controllers
{
    public class ExerciseCreatorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ExerciseCreator
        public ActionResult Index()
        {
            int max;
            try
            {
                max = db.AnswerModelNames.Max(p => p.Id) + 1;
            }
            catch (Exception e)
            {
                max = 1;
            }
            ViewBag.modelID = max;
            ViewBag.error = "";
            return View();
        }
        // POST: ExerciseCreator -- Ta część będzie tą brzydką częścią
        [HttpPost]
        public ActionResult Index(string SQLheader, string SQLcode, string modelID)
        {
            try
            {
                string test = Convert.ToString(SQLheader) + Convert.ToString(SQLcode);

                if (db.Database.ExecuteSqlCommand(test) == 0)
                {
                    return View();
                }
                db.AnswerModelNames.Add(new AnswerModelName { ModelName = "Model_" + modelID });
                db.SaveChangesAsync();
                return RedirectToAction("Continue", new { @modelID = modelID });
            }
            catch (Exception e)
            {
                ViewBag.modelID = int.Parse(modelID);
                ViewBag.error = "0001";
                ViewBag.errorMsg = e.Message + "\n" + e.InnerException;
                return View();
            }
        }
        // GET: TaskCreator/Continue/modelID
        public async Task<ActionResult> Continue(int? modelID)
        {
            int taskID;
            try
            {
                taskID = db.Exercises.Max(p => p.Id) + 1;
            }
            catch
            {
                taskID = 1;
            }
            ViewBag.modelID = modelID;
            ViewBag.taskID = taskID;
            ViewBag.userID = User.Identity.GetUserId();
            ViewBag.error = "";
            ViewBag.Subcategories = await db.Subcategories.ToListAsync();
            return View();
        }
        // POST: TaskCreator/Continue
        [HttpPost]
        public async Task<ActionResult> Continue([Form] Exercise ex, int modelID, int taskID, int Subcategory_Id)
        {
            try
            {
                ex.Subcategory = await db.Subcategories.FirstOrDefaultAsync(t => t.Id == Subcategory_Id);
                ex.AnswerModelName = await db.AnswerModelNames.FirstOrDefaultAsync(t => t.Id == modelID);
                db.Exercises.Add(ex);
                await db.SaveChangesAsync();
                return RedirectToAction("Done");
            }
            catch (Exception e)
            {
                ViewBag.modelID = modelID;
                ViewBag.taskID = taskID;
                ViewBag.userID = User.Identity.GetUserId();
                ViewBag.error = "0002";
                ViewBag.errorMsg = e.Message + "\n" + e.InnerException;
                var subcategories = await db.Subcategories.ToListAsync();
                ViewBag.subcategories = subcategories;
                return View();
            }
        }
        // GET: TaskCreator/Done
        public ActionResult Done()
        {
            return View();
        }
        // GET: TaskCreator/Debug
        public ActionResult Debug()
        {
            return RedirectToAction("Continue", new { @modelID = 5 });
        }
    }
}