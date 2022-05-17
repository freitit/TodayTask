namespace TodayTask.ModelsAppSettings
{
    public class AzureDevOpsServiceSetting
    {
        public const string SectionName = "AzureDevOpsServiceSetting";
        public string OrganizationUrl { get; set; }
        public string Username { get; set; }
        public string PersonalAccessToken { get; set; }
    }
}
