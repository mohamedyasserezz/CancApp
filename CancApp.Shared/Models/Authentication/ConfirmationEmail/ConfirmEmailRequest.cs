namespace CancApp.Shared.Models.Authentication.ConfirmationEmail
{
    public record ConfirmEmailRequest(
    string UserId,
    string Code
);
}
