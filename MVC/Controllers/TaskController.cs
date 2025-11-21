#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

namespace MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly IService<TaskRequest, TaskResponse> _taskService;
        private readonly IService<ProjectRequest, ProjectResponse> _projectService;
        private readonly IService<UserRequest, UserResponse> _userService;

        public TaskController(
            IService<TaskRequest, TaskResponse> taskService,
            IService<ProjectRequest, ProjectResponse> projectService,
            IService<UserRequest, UserResponse> userService
        )
        {
            _taskService = taskService;
            _projectService = projectService;
            _userService = userService;
        }

        private void SetViewData()
        {
            ViewData["ProjectId"] = new SelectList(_projectService.List(), "Id", "Name");
            ViewData["UserId"] = new SelectList(_userService.List(), "Id", "UserName");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        // GET: Task
        public IActionResult Index()
        {
            var list = _taskService.List();
            return View(list);
        }

        // GET: Task/Details/5
        public IActionResult Details(int id)
        {
            var item = _taskService.Item(id);
            return View(item);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Task/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(TaskRequest task)
        {
            if (ModelState.IsValid)
            {
                var response = _taskService.Create(task);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(task);
        }

        // GET: Task/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _taskService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Task/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(TaskRequest task)
        {
            if (ModelState.IsValid)
            {
                var response = _taskService.Update(task);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(task);
        }

        // GET: Task/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _taskService.Item(id);
            return View(item);
        }

        // POST: Task/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _taskService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
