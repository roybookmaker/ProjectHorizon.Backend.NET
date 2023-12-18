namespace ProjectHorizon.Shared.Library.Base
{
    public interface IAuthInfo
    {
        public Guid UserIdentifier { get; set; }
        public Guid? OrganizationIdentifier { get; set; }
        public string Username { get; set; }
        public string? Language { get; set; }
        public string? Roles { get; set; }
        public string Token { get; set; }
    }
}
