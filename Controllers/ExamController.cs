using Microsoft.AspNet.Identity;
using SQLappTheSequel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SQLappTheSequel.Controllers
{
    public class ExamController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Exam
        public async Task<ActionResult> Index(int? id)
        {
            Exercise exercise = await db.Exercises.Where(e => e.Id == id).FirstOrDefaultAsync();
            var results = await db.Results.Where(r=>r.ExerciseId.Id == id).ToListAsync();
            ViewBag.Command = exercise.Command;
            return View(results);
        }
        // GET: Exam/Start
        public ActionResult Start(int? id)
        {
            ViewBag.id = id;
            return View();
        }

        // GET: Exam/Write
        public async Task<ActionResult> Write(int? id)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.ExeTable = "ExeTable_" + userId.Replace("-","_");
            ViewBag.TaskID = id;
            Exercise exercise = await db.Exercises.Where(e => e.Id == id).FirstOrDefaultAsync();
            return View(exercise);
        }
        [HttpPost]
        // POST: Exam/Write
        public async Task<ActionResult> Write(string SQLcode, int taskID)
        {
            string userId = User.Identity.GetUserId();
            string ExeTable = "ExeTable_" + userId.Replace("-", "_");
            Exercise exercise = await db.Exercises.Where(e => e.Id == taskID).Include(e => e.AnswerModelName).FirstOrDefaultAsync();
            int modelId = exercise.AnswerModelName.Id;

            try
            {
                //Przetwarzanie kodu studenta
                db.Database.ExecuteSqlCommand(SQLcode);
            }
            catch(Exception e)
            {
                ViewBag.ExeTable = ExeTable;
                ViewBag.TaskID = taskID;
                ViewBag.error = "0003";
                ViewBag.errorMsg = e.Message + "\n" + e.InnerException;
                return View(exercise);
            }

            //Sprawdzenie tabel
            var returnCode = new SqlParameter();
            returnCode.ParameterName = "@ResultOut";
            returnCode.SqlDbType = SqlDbType.Int;
            returnCode.Direction = ParameterDirection.Output;
            var model = new SqlParameter("@Model", "Model_"+modelId);
            var Answer = new SqlParameter("@Answer", ExeTable);

            var testCode = new SqlParameter();
            testCode.ParameterName = "@testCode";
            testCode.SqlDbType = SqlDbType.Int;
            testCode.Direction = ParameterDirection.Output;

            var data = db.Database.SqlQuery<List<int>>("EXEC @testCode = CheckTables @Model, @Answer, @ResultOut OUT", testCode, model, Answer, returnCode);
            var test = data.ToList();
            try
            {
                //Wynik sprawdzenia
                int returnProc = (int)testCode.Value;
                Result result = new Result();
                result.Score = returnProc;
                result.ExerciseId = exercise;
                result.UserId = userId;
                result.UserAnswerSql = SQLcode;
                result.CompletionDate = DateTime.Now;
                db.Results.Add(result);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ViewBag.ExeTable = ExeTable;
                ViewBag.TaskID = taskID;
                ViewBag.error = "0004";
                ViewBag.errorMsg = e.Message + "\n" + e.InnerException;
                return View(exercise);
            }
            try
            {
                db.Database.ExecuteSqlCommand("DROP TABLE " + ExeTable);
            }
            catch{}
            return RedirectToAction("Finish");
            //return "error";
        }
        // GET: Exam/Finish
        public ActionResult Finish()
        {
            return View();
        }
    }
}