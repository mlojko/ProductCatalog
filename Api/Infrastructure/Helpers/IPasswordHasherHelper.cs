namespace Api.Infrastructure.Helpers
{
    public interface IPasswordHasherHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}