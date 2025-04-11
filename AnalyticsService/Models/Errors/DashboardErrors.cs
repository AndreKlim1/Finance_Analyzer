namespace AnalyticsService.Models.Errors
{
    public static class DashboardErrors
    {
        public static readonly Error DashboardItemNotFound = new("DashboardItemNotFound", "Dashboard Item not found");
        public static readonly Error InvalidCredentials = new("InvalidCredentials", "Invalid credentials");
    }
}
