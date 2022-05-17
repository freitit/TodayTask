using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;

namespace TodayTask.Models
{
    /// <summary>
    /// WorkItem model.
    /// </summary>
    public class WorkItemViewModel
    {
        /// <summary>
        /// WorkItem ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// WorkItem Title.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// WorkItem Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// WorkItem State.
        /// </summary>
        public State State { get; set; }

        public WorkItemViewModel(){}

        public WorkItemViewModel(WorkItem workItem)
        {
            Id = workItem.Id ?? 0;
            Title = (string) workItem.Fields["System.Title"];
            Type = (string) workItem.Fields["System.WorkItemType"];
            State = ((string) workItem.Fields["System.State"]) switch
            {
                "Active" => GetStateFromActiveWorkItem(workItem),
                "Resolved" => State.Resolved,
                "Closed" => State.Completed,
                _ => State.InProgress
            };
        }

        private static State GetStateFromActiveWorkItem(WorkItem workItem)
        {
            foreach (var workItemRelation in workItem.Relations)
            {
                if (workItemRelation.Rel.Equals("ArtifactLink", StringComparison.OrdinalIgnoreCase))
                {
                    var canGetRelationName = workItemRelation.Attributes.TryGetValue("name", out var relationName);
                    if (canGetRelationName && (relationName as string).Equals("Pull Request", StringComparison.OrdinalIgnoreCase))
                    {
                        return State.UnderReview;
                    }
                }
            }

            return State.InProgress;
        }
    }
}
