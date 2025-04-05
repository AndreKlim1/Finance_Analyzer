namespace CategoryAccountService.Messaging.DTO
{
    public class DeletionEvent
    {
        public DeletionEvent(string eventType, long id)
        {
            EventType = eventType;
            Id = id;
        }
        public string EventType { get; set; }
        public long Id { get; set; }
    }
}
