#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IService<ProjectRequest, ProjectResponse> _projectService;

        public ProjectController(
            IService<ProjectRequest, ProjectResponse> projectService
        )
        {
            _projectService = projectService;
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
            TempData["Message"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
