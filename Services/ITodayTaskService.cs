using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodayTask.Models;

namespace TodayTask.Services
{
    public interface ITodayTaskService
    {
        public Task<IEnumerable<WorkItemViewModel>> GetTodayTasks(string user, DateTime? dateTime);
    }
}
