using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodayTask.Models;
using TodayTask.ModelsAppSettings;

namespace TodayTask.Services
{
    public class TodayTaskService : ITodayTaskService
    {
        private readonly VssConnection _connection;

        private const string WORK_DAY_FINISH_AT = "18:00";
        private const string WORK_ITEMS_QUERY_STRING = 
            "SELECT [Id] " +
            "FROM WorkItems " +
            "WHERE [Work Item Type] IN ('Task', 'Bug') AND " +
                "([State] = 'Active' AND [Assigned To] = {0}) " +
                "OR " +
                "(" +
                    "(" +
                        "[State] = 'Resolved' " +
                        "OR " +
                        "([State] = 'Closed' AND [Closed Date] = {1})" +
                    ") " +
                    "AND [Resolved Date] = {1} AND [Assigned To] EVER {0} AND [Activated By] = {0}" +
                ") " +
            "ASOF {2}";
        
        public TodayTaskService(IOptions<AzureDevOpsServiceSetting> connectionSetting)
        {
            var setting = connectionSetting.Value; 
            _connection = new(new Uri(setting.OrganizationUrl), new VssBasicCredential(setting.Username, setting.PersonalAccessToken));
        }

        public async Task<IEnumerable<WorkItemViewModel>> GetTodayTasks(string user, DateTime? queryDate)
        {
            user = !string.IsNullOrEmpty(user) ? $"'{user}'" : "@Me";

            if (!queryDate.HasValue)
            {
                queryDate = DateTime.Now;
            }

            var workItems = await GetWorkItems(user, $"{queryDate.Value:yyyy-MM-dd}", WORK_DAY_FINISH_AT);

            return workItems != null && workItems.Any() ?
                workItems.Select(workItem => new WorkItemViewModel(workItem)) :
                Enumerable.Empty<WorkItemViewModel>();
        }

        private async Task<IEnumerable<WorkItem>> GetWorkItems(string user, string date, string time)
        {
            WorkItemTrackingHttpClient witClient = await _connection.GetClientAsync<WorkItemTrackingHttpClient>();

            Wiql query = new() { Query = string.Format(WORK_ITEMS_QUERY_STRING, user, $"'{date}'", $"'{date} {time}'") };
            WorkItemQueryResult queryResults = await witClient.QueryByWiqlAsync(query);

            if (queryResults == null || !queryResults.WorkItems.Any())
            {
                return null;
            }

            var workItems = await witClient.GetWorkItemsAsync(queryResults.WorkItems.Select(x => x.Id).Take(200), expand: WorkItemExpand.All);
            return workItems;
        }
    }
}
