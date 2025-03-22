namespace CategoryAccountService.Models.DTO
{
    public record DateRange(DateTime StartDate, DateTime EndDate)
    {
        public DateRange() : this(DateTime.MinValue, DateTime.MaxValue) { }

        public bool IsValid() => StartDate <= EndDate;
    }
}
