namespace ProjectHorizon.Microservice.Account.Components.DTO
{
    public class RecoveryDTO
    {
        public Guid Id { get; set; }
        public Guid Userid { get; set; }
        public string? RecoveryKey { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
