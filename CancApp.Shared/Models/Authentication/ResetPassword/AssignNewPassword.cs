namespace CancApp.Shared.Models.Authentication.ResetPassword
{
    public record AssignNewPassword(
        string Email,
        string NewPassword
        );
}
