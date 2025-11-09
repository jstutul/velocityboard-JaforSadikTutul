using Microsoft.AspNetCore.Mvc;
using VelocityBoard.Web.Models;
using VelocityBoard.Web.Services;

namespace VelocityBoard.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;
        string apiBaseUrl = null;
        public AccountController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
            apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                if (model == null) return BadRequest("No data received");
                var loginData = new { model.Email, model.Password };

                var url = apiBaseUrl + "Auth/login";
                var response = await _apiService.PostAsync<LoginResponse>(url, loginData);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    HttpContext.Session.SetString("JWToken", response.Token);
                    return Ok(response);
                }

                ViewBag.Error = "Invalid login";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Login failed: " + ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Login");
        }

        public class LoginResponse
        {
            public string Token { get; set; }
        }
    }
}
