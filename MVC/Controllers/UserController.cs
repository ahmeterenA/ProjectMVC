#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;
using APP.Services;
using Microsoft.AspNetCore.Authorization;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<RoleRequest, RoleResponse> _roleService;
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        public UserController(
            IService<UserRequest, UserResponse> userService,
            IService<RoleRequest, RoleResponse> roleService,
            IService<GroupRequest, GroupResponse> groupService
        )
        {
            _userService = userService;
            _roleService = roleService;
            _groupService = groupService;
        }

        [AllowAnonymous]
        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("/Login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;
                var response = await userService.Login(request);
                if (response.IsSuccessful)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", response.Message);
            }
            return View();
        }

        [Authorize]
        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            var userService = _userService as UserService;
            await userService.Logout();
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [Route("/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        [Route("/Register")]
        public IActionResult Register(UserRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var userService = _userService as UserService;
                var response = userService.Register(request);
                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Login));
                ModelState.AddModelError("", response.Message);
            }
            return View();
        }

        private void SetViewData()
        {
            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title");
            ViewBag.RoleIds = new MultiSelectList(_roleService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            TempData[key] = message;
        }

        [Authorize]
        public IActionResult Index()
        {
            var list = _userService.List();
            return View(list);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var item = _userService.Item(id);
            return View(item);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Create(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(user);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var item = _userService.Edit(id);
            SetViewData();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(UserRequest user)
        {
            if (ModelState.IsValid)
            {
                var response = _userService.Update(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message);
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError("", response.Message);
            }
            SetViewData();
            return View(user);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var item = _userService.Item(id);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _userService.Delete(id);
            SetTempData(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}

