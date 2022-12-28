using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sd.Auth.Domain;
using sd.Auth.Web.Models;

namespace sd.Auth.Web.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IHttpClientFactory clientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
        }

        [Route("Register")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                Path = "/"
            };

            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    Response.Cookies.Append("UserName", model.Username, cookieOptions);
                    Response.Cookies.Append("UserId", user.Id, cookieOptions);
                    return RedirectToAction("StartGame", "Game");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [Route("Login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                Path = "/"
            };

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    var loggedinUser = await _userManager.FindByNameAsync(user.Username);
                    Response.Cookies.Append("UserName", user.Username, cookieOptions);
                    Response.Cookies.Append("UserId", loggedinUser.Id, cookieOptions);
                    return RedirectToAction("StartGame", "Game");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("UserName");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var httpClient = _clientFactory.CreateClient();
            Uri siteUri = new("https://localhost:7187/Statistics/GetStatisticsById?id=" + Request.Cookies["UserId"]);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, siteUri) { };

            StatisticViewModel viewModel = new();

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                viewModel = JsonConvert.DeserializeObject<StatisticViewModel>(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Either this feature is currently not working or there is no saved statistic");
            }
            viewModel.User = Request.Cookies["UserName"];

            return View(viewModel);
        }
    }
}
