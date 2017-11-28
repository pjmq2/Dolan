using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IngeDolan3._0.Models;
using System.Linq.Dynamic;
using IngeDolan3._0.Generator;

namespace IngeDolan3._0.Controllers
{
    public class ScenariosController : Controller{
        private NewDolan2Entities db = new NewDolan2Entities();

        // GET: Displays the scenarios list that belong to a specific project
        public async Task<ActionResult> Index(string projectId, string storyId, int page = 1, string sort = "ScenarioNumber", string sortdir = "asc", string search = "")
        {
            if (!IDGenerator.CanDo("Consultar Lista de Escenarios"))
            {
                return RedirectToAction("Denied", "Others");
            }
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetScenarios(search, sort, sortdir, skip, pageSize, out totalRecord, projectId, storyId);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            ViewBag.ProjectId = projectId;
            ViewBag.StoryId = storyId;
            return View(data);
        }

        public List<Scenario> GetScenarios(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord, string Idproject, string Idstory)
        {
            IQueryable<Scenario> v = (from a in db.Scenarios
                     where
                        a.ProjectID.Equals(Idproject) && a.StoryID.Equals(Idstory) && (
                            a.AcceptanceCriteria.Contains(search) ||
                            a.Resultado.Contains(search)
                        )
                     select a
                        );
            totalRecord = v.Count();
            v = v.OrderBy(sort + " " + sortdir);
            if (pageSize > 0)
            {
                v = v.Skip(skip).Take(pageSize);
            }
            return v.ToList();
        }

        // GET: Displays a list of the details for an specific scenario
        public async Task<ActionResult> Details(String projectId, string storyId, string ScenarioId)
        {
            if (!IDGenerator.CanDo("Consultar Detalles de Escenarios"))
            {
                return RedirectToAction("Denied", "Others");
            }
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // GET: Displays a screen that allows the user to create an scenario
        public ActionResult Create(string projectId, string storyId)
        {
            if (!IDGenerator.CanDo("Crear Escenarios"))
            {
                return RedirectToAction("Denied", "Others");
            }
            Scenario pl = new Scenario();
            int sprintId = db.UserStories.Where(x => x.StoryID == storyId).ToList().FirstOrDefault().SprintID;
            pl.ProjectID = projectId;
            pl.StoryID = storyId;
            pl.SprintID = sprintId;
            pl.ScenarioNumber = Convert.ToInt32(DateTime.Now.ToString("MMddyyyyhhmmssffffff"));
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo");
            return View(pl);
        }

        // Stores the new scenario on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                db.Scenarios.Add(scenario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // Displays a screen that lets the user edit an scenario
        public async Task<ActionResult> Edit(String projectId, string storyId, string ScenarioId)
        {
            if (!IDGenerator.CanDo("Editar Escenarios"))
            {
                return RedirectToAction("Denied", "Others");
            }
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // POST: Stores the edited scenario information on the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scenario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.UserStories, "ProjectID", "Modulo", scenario.ProjectID);
            return View(scenario);
        }

        // GET: Displays a screen that allows an user to delete an scenario
        public async Task<ActionResult> Delete(String projectId, string storyId, string ScenarioId)
        {
            if (!IDGenerator.CanDo("Borrar Escenarios"))
            {
                return RedirectToAction("Denied", "Others");
            }
            if (ScenarioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            return View(scenario);
        }

        // Deletes the specified scenario from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String projectId, string storyId, string ScenarioId)
        {
            Scenario scenario = await db.Scenarios.FindAsync(ScenarioId);
            db.Scenarios.Remove(scenario);
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
