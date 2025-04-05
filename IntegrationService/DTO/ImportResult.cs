namespace IntegrationService.DTO
{
    public class ImportResult
    {
        public int SuccessfulCount { get; set; }
        public int FailedCount { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
