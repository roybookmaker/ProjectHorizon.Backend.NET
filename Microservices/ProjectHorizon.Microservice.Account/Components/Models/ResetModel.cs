namespace ProjectHorizon.Microservice.Account.Components.Models
{
    public sealed record ResetModel
        (
        Guid Id,
        string RecoveryKey,
        string NewPassword
        );
}
