namespace ProjectHorizon.Microservice.Account.Components.Models
{
    public sealed record LoginModel
        (
        string Username,
        string? Password,
        string? Token
        );
}
