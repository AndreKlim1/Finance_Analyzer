namespace AnalyticsService.Models.DTO.Response.SpendingPatternsReport
{
    public class ValueDistributionResponse
    {
        public List<ValueDistributionBinDto> Bins { get; set; } = new();
    }

    public class ValueDistributionBinDto
    {
        public string Label { get; set; } = default!;
        public int Count { get; set; }
    }
}
