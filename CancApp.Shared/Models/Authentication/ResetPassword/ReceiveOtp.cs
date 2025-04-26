namespace CancApp.Shared.Models.Authentication.ResetPassword
{
    public record ReceiveOtp(
        string Email,
        string Otp
    );
}
