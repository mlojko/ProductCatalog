namespace Api.Models.Auth
{
    public class AuthResponse(User user, string token)
    {
        public int Id { get; set; } = user.Id;
        public string Username { get; set; } = user.Username;
        public string Token { get; set; } = token;
    }
}
