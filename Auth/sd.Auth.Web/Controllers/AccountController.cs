using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sd.Auth.Domain;
using sd.Auth.Web.Models;

namespace sd.Auth.Web.Controllers;

[Route("[controller]")]
public class AccountController(UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IHttpClientFactory clientFactory,
    ILogger<AccountController> logger) : Controller
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet("Register")]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("Register")]
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

                _logger.LogInformation("new user and cookies were created with username: {username} and userid: {userId}",
                    model.Username,
                    user.Id);

                return RedirectToAction("StartGame", "Game");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            _logger.LogInformation("Invalid Login Attempt with username: {username} and userid: {userId}",
                model.Username,
                user.Id);
        }

        return View(model);
    }

    [HttpGet("Login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("Login")]
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
            var result = await _signInManager
                .PasswordSignInAsync(user.Username, user.Password, user.RememberMe, false);

            if (result.Succeeded)
            {
                var loggedinUser = await _userManager.FindByNameAsync(user.Username);

                Response.Cookies.Append("UserName", user.Username, cookieOptions);
                Response.Cookies.Append("UserId", loggedinUser?.Id ?? "", cookieOptions);

                _logger.LogInformation("Logged in with username: {username} and userid: {userId}",
                    user.Username,
                    loggedinUser?.Id ?? "");

                return RedirectToAction("StartGame", "Game");
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            _logger.LogInformation("Invalid Login Attempt username: {username}",
                user.Username);
        }
        return View(user);
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        _logger.LogInformation("User logged out username: {username}, userId: {userid}",
            Request.Cookies["UserName"],
            Request.Cookies["UserId"]);

        Response.Cookies.Delete("UserId");
        Response.Cookies.Delete("UserName");

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
        var httpClient = _clientFactory.CreateClient();
        Uri siteUri = new("http://192.168.18.105/Statistics/GetStatisticsById?id=" + Request.Cookies["UserId"]);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, siteUri) { };

        StatisticViewModel viewModel = new();

        _logger.LogDebug("Asking for user: {user} statistics", Request.Cookies["UserName"]);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
            viewModel = JsonConvert.DeserializeObject<StatisticViewModel>(result) ?? new();
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Either this feature is currently not working or there is no saved statistic");
        }
        viewModel.User = Request.Cookies["UserName"] ?? "";

        _logger.LogDebug("Recieved user: {user} statistics: {@stats}", Request.Cookies["UserName"], viewModel);

        return View(viewModel);
    }
}
