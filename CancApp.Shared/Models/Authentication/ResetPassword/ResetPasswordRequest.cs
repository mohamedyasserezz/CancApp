namespace CancApp.Shared.Models.Authentication.ResetPassword
{
    public record ResetPasswordRequest(
    string Email,
    string Otp
);
}
