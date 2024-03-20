using System.Security.Cryptography;
using System.Text;
using tongDe.Services.Interfaces;

namespace tongDe.Services;

public class TokenService : ITokenService
{
    private readonly byte[] _key;

    public TokenService(IConfiguration configuration)
    {
        _key = Encoding.UTF8.GetBytes(configuration["TokenService:Key"]);
    }

    public string Encrypt(Guid shopToken, Guid userId)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.GenerateIV();
            var iv = aes.IV;

            var encryptor = aes.CreateEncryptor(aes.Key, iv);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write($"{userId}|{shopToken}");
                }
                var encrypted = msEncrypt.ToArray();
                var result = new byte[iv.Length + encrypted.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

                return Convert.ToBase64String(result);
            }
        }
    }

    public (Guid shopToken, Guid userId) Decrypt(string token)
    {
        var fullCipher = Convert.FromBase64String(token);

        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            var iv = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var msDecrypt = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                var decryptedToken = srDecrypt.ReadToEnd();
                var splittedDecryptedToken = decryptedToken.Split("|");
                return (Guid.Parse(splittedDecryptedToken[1]), Guid.Parse(splittedDecryptedToken[0]));
            }
        }
    }
}
