using Microsoft.Extensions.Configuration;
using Moq;
using tongDe.Services;

namespace tongDe.Tests.UnitTest;

public class TokenServiceTest
{
    private readonly TokenService _tokenService;
    private readonly Mock<IConfiguration> _mockConfig = new Mock<IConfiguration>();

    public TokenServiceTest()
    {
        _mockConfig.SetupGet(x => x["TokenService:Key"]).Returns("12345678901234567890123456789012");
        var config = _mockConfig.Object;

        _tokenService = new TokenService(config);
    }

    [Fact]
    public void EncryptAndDecrypt_ShouldReturnOriginalValues()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var shopToken = Guid.NewGuid();

        // Act
        var encryptedToken = _tokenService.Encrypt(shopToken, userId);
        var (decryptedShopToken, decryptedUserId) = _tokenService.Decrypt(encryptedToken);

        // Assert
        Assert.Equal(shopToken, decryptedShopToken);
        Assert.Equal(userId, decryptedUserId);
    }
}