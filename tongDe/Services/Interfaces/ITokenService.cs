namespace tongDe.Services.Interfaces;

public interface ITokenService
{
    public string Encrypt(Guid shopToken, Guid userId);
    public (Guid shopToken, Guid userId) Decrypt(string token);
}
