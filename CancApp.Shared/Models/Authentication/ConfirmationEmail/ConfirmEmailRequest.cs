namespace CancApp.Shared.Models.Authentication.ConfirmationEmail
{
    public record ConfirmEmailRequest(
    string Email,
    string Otp
);
}
