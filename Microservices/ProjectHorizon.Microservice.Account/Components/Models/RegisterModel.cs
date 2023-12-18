namespace ProjectHorizon.Microservice.Account.Components.Models
{
    public sealed record RegisterModel
        (
        string Username,
        string Password,
        string Email
        );
}
