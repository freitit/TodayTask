namespace TodayTask.Models
{
    public enum State
    {
        /// <summary>
        /// The task is being done.
        /// </summary>
        InProgress,

        /// <summary>
        /// The task is temporarily done, waiting for the approval from the team leader.
        /// </summary>
        UnderReview,

        /// <summary>
        /// The task is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The bug is generally completed, but need furthur check from QA.
        /// </summary>
        Resolved
    }
}
