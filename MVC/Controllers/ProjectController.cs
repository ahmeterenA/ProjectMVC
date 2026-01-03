#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;
using Microsoft.AspNetCore.Authorization;
using CORE.APP.Services.Session;

namespace MVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IService<ProjectRequest, ProjectResponse> _projectService;
        private readonly SessionServiceBase _sessionService;

        public ProjectController(
            IService<ProjectRequest, ProjectResponse> projectService,
            SessionServiceBase sessionService
        )
        {
            _projectService = projectService;
            _sessionService = sessionService;
        }

        // GET: Project
        public IActionResult Index()
        {
            var list = _projectService.List();
            return View(list);
        }

        // GET: Project/Details/5
        public IActionResult Details(int id)
        {
            var item = _projectService.Item(id);
            
            if (item != null)
            {
                var recentlyViewed = _sessionService.GetSession<List<ProjectResponse>>("RecentlyViewedProjects") ?? new List<ProjectResponse>();
                
                // Remove if exists to re-add at the top (or just avoid duplicates)
                var existing = recentlyViewed.FirstOrDefault(p => p.Id == item.Id);
                if (existing != null)
                {
                    recentlyViewed.Remove(existing);
                }
                
                recentlyViewed.Insert(0, item);
                
                // Keep only last 5
                if (recentlyViewed.Count > 5)
                {
                    recentlyViewed = recentlyViewed.Take(5).ToList();
                }
                
                _sessionService.SetSession("RecentlyViewedProjects", recentlyViewed);
                ViewBag.RecentlyViewedProjects = recentlyViewed;
            }

            return View(item);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProjectRequest project)
        {
            if (ModelState.IsValid)
            {
                var response = _projectService.Create(project);
                if (response.IsSuccessful)
                {
                    TempData["Message"] = response.Message;
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(project);
        }

        // GET: Project/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _projectService.Edit(id);
            return View(item);
        }

        // POST: Project/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectRequest project)
        {
            if (ModelState.IsValid)
            {
                var response = _projectService.Update(project);
                if (response.IsSuccessful)
                {
                    TempData["Message"] = response.Message;
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            return View(project);
        }

        // GET: Project/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _projectService.Item(id);
            return View(item);
        }

        // POST: Project/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _projectService.Delete(id);
            if (response.IsSuccessful)
            {
                var recentlyViewed = _sessionService.GetSession<List<ProjectResponse>>("RecentlyViewedProjects");
                if (recentlyViewed != null)
                {
                    var itemToRemove = recentlyViewed.FirstOrDefault(p => p.Id == id);
                    if (itemToRemove != null)
                    {
                        recentlyViewed.Remove(itemToRemove);
                        _sessionService.SetSession("RecentlyViewedProjects", recentlyViewed);
                    }
                }
            }
            TempData["Message"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
