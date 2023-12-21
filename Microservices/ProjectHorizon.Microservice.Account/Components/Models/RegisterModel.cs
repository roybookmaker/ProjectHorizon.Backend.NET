namespace ProjectHorizon.Microservice.Account.Components.Models
{
    public sealed record RegisterModel
        (
        string Fullname,
        string Username,
        string Password,
        string Email
        );
}
