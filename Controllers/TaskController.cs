using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodayTask.Services;

namespace TodayTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITodayTaskService _todayTaskService;

        public TaskController(ITodayTaskService todayTaskService)
        {
            _todayTaskService = todayTaskService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string user, [FromQuery] DateTime? date)
        {
            var workItems = await _todayTaskService.GetTodayTasks(user, date);
            return Ok(workItems);
        }
    }
}
