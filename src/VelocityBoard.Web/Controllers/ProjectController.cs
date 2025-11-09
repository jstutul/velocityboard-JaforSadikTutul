using Microsoft.AspNetCore.Mvc;
using VelocityBoard.Web.Models;
using VelocityBoard.Web.Services;
using static VelocityBoard.Web.Controllers.AccountController;

namespace VelocityBoard.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;
        string apiBaseUrl = null;

        public ProjectController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
            apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
        }


        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var url = apiBaseUrl + "Project";
            var response = await _apiService.GetAsync<List<ProjectViewModel>>(url, new{});
            return Json(response);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateProject([FromBody] ProjectDto project)
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    var created = await _apiService.PostAsync<ProjectDto>("https://localhost:7212/api/Project", project, token);
        //    return Json(created);
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateProject([FromBody] ProjectDto project)
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    var updated = await _apiService.PutAsync<ProjectDto>($"https://localhost:7212/api/Project/{project.Id}", project, token);
        //    return Json(updated);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProject(int id)
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    var result = await _apiService.DeleteAsync($"https://localhost:7212/api/Project/{id}", token);
        //    return Json(result);
        //}
    }
}
