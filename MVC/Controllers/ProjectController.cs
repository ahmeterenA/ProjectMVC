#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

namespace MVC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IService<ProjectRequest, ProjectResponse> _projectService;
        private readonly IService<UserRequest, UserResponse> _userService;

        public ProjectController(
            IService<ProjectRequest, ProjectResponse> projectService,
            IService<UserRequest, UserResponse> userService
        )
        {
            _projectService = projectService;
            _userService = userService;
        }

        private void SetViewData()
        {
            ViewBag.UserIds = new MultiSelectList(_userService.List(), "Id", "UserName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
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
            SetViewData();
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
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(project);
        }

        // GET: Project/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _projectService.Edit(id);
            SetViewData();
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
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
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
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
