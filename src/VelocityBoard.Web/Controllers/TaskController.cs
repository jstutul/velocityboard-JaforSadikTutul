using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelocityBoard.Web.Models;
using VelocityBoard.Web.Services;

namespace VelocityBoard.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IConfiguration _configuration;
        string apiBaseUrl = null;

        public TaskController(ApiService apiService, IConfiguration configuration)
        {
            _apiService = apiService;
            _configuration = configuration;
            apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetTasks([FromBody] TaskViewModel model)
        {
            var url = apiBaseUrl + $"Task/ProjectWiseTask/{model.ProjectId}";
            var tasks = await _apiService.GetAsync<List<TaskViewModel>>(url, new {});
            return Json(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            var url = apiBaseUrl + $"Auth/GetAll";
            var tasks = await _apiService.GetAsync<List<UserList>>(url, new { });
            return Json(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> SaveTask([FromBody] TaskViewModel taskDto)
        {
            try
            {
                var url = apiBaseUrl + $"Task";
                if (taskDto == null) return BadRequest("No data received");
                var data = new { taskDto.Title, taskDto.Description, taskDto.AssignedToUserId, taskDto.ProjectId, taskDto.DueDate,taskDto.Id };

                var tasks = await _apiService.PostAsync<TaskViewModel>(url, data);
                return Json(tasks);
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask([FromBody] TaskViewModel taskDto)
        {
            try
            {
                var url = apiBaseUrl + $"Task/" + taskDto.Id;
                if (taskDto == null) return BadRequest("No data received");
                var data = new { taskDto.Title, taskDto.Description, taskDto.AssignedToUserId, taskDto.ProjectId, taskDto.DueDate, taskDto.Id };

                var tasks = await _apiService.PutAsync<TaskViewModel>(url, data);
                return Json(tasks);
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }
    }
}
