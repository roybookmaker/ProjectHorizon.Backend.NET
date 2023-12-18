namespace ProjectHorizon.Microservice.Account.Components.DTO
{
    public class AccountDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
