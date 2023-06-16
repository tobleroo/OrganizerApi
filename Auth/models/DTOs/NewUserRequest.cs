namespace OrganizerApi.Auth.models.DTOs
{
    public record NewUserRequest(string Name, string EmailAddress, string Password);

}
