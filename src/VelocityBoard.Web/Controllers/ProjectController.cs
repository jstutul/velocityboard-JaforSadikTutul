using Microsoft.AspNetCore.Mvc;
using VelocityBoard.Web.Models;
using VelocityBoard.Web.Services;

namespace VelocityBoard.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApiService _apiService;

        public ProjectController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            return View();
        }

        //// API endpoints for AJAX
        //[HttpGet]
        //public async Task<IActionResult> GetProjects()
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    var projects = await _apiService.GetAsync<List<ProjectViewModel>>("https://localhost:7212/api/Project", token);
        //    return Json(projects);
        //}

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
