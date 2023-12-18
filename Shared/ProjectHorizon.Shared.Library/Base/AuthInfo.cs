namespace ProjectHorizon.Shared.Library.Base
{
    public class AuthInfo : IAuthInfo
    {
        public AuthInfo() { }

        public Guid UserIdentifier { get; set; }
        public Guid? OrganizationIdentifier { get; set; }
        public string Username { get; set; }
        public string? Language { get; set; }
        public string? Roles { get; set; }
        public string Token { get; set; }

        public AuthInfo(IAuthInfo authInfo)
        {
            UserIdentifier = authInfo.UserIdentifier;
            OrganizationIdentifier = authInfo.OrganizationIdentifier;
            Username = authInfo.Username;
            Language = authInfo.Language;
            Roles = authInfo.Roles;
            Token = authInfo.Token;
        }
    }
}
