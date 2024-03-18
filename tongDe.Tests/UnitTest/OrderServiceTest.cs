using tongDe.Services;

namespace tongDe.Tests.UnitTest;

public class OrderServiceTest
{
    [Theory]
    [InlineData("客戶A\n商品A 2件\n商品B 3斤", 2, "商品A", 2, "件", "商品B", 3, "斤")]
    [InlineData("客戶A\n商品A 2 件\n商品B 3 斤", 2, "商品A", 2, "件", "商品B", 3, "斤")]
    [InlineData("客戶A\n商品A2件\n商品B3斤", 2, "商品A", 2, "件", "商品B", 3, "斤")]
    [InlineData("客戶A\n商品A2件\n商品B9.99斤", 2, "商品A", 2, "件", "商品B", 9.99, "斤")]
    public void ConvertStringToOrderInfo_ShouldReturnCorrectOrderInfo_WithValidString(string input, int expectedItemCount,
        string expectedFirstItemName, decimal expectedFirstItemQuantity, string expectedFirstItemUnit,
        string expectedSecondItemName, decimal expectedSecondItemQuantity, string expectedSecondItemUnit)
    {
        // Arrange
        var orderService = new OrderService(null);

        // Act
        var result = orderService.ConvertStringToOrderInfo(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("客戶A", result.ClientName);
        Assert.Equal(expectedItemCount, result.OrderItems.Count);

        var firstItem = result.OrderItems.First();
        Assert.Equal(expectedFirstItemName, firstItem.Name);
        Assert.Equal(expectedFirstItemQuantity, firstItem.Quantity, 3);
        Assert.Equal(expectedFirstItemUnit, firstItem.Unit);

        var secondItem = result.OrderItems.Last();
        Assert.Equal(expectedSecondItemName, secondItem.Name);
        Assert.Equal(expectedSecondItemQuantity, secondItem.Quantity, 3);
        Assert.Equal(expectedSecondItemUnit, secondItem.Unit);
    }

    [Fact]
    public void ConvertStringToOrderInfo_ShouldReturnNull_ForEmptyString()
    {
        // Arrange
        var orderService = new OrderService(null);
        var testInput = "";

        // Act
        var result = orderService.ConvertStringToOrderInfo(testInput);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ConvertStringToOrderInfo_ShouldIgnoreInvalidOrderItemLines()
    {
        // Arrange
        var orderService = new OrderService(null);
        var testInput = "客戶C\n無效項\n商品C 1件";

        // Act
        var result = orderService.ConvertStringToOrderInfo(testInput);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("客戶C", result.ClientName);
        Assert.Single(result.OrderItems);
        Assert.Equal("商品C", result.OrderItems[0].Name);
    }
    [Fact]
    public void ConvertStringToOrderInfo_ShouldReturnNull_WhenNoOrderItemsProvided()
    {
        // Arrange
        var orderService = new OrderService(null);
        var testInput = "客戶B";

        // Act
        var result = orderService.ConvertStringToOrderInfo(testInput);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("\n")]
    [InlineData("客戶B\n無效項")]
    public void ConvertStringToOrderInfo_ShouldReturnNull_ForInvalidOrEmptyInput(string input)
    {
        // Arrange
        var orderService = new OrderService(null);

        // Act
        var result = orderService.ConvertStringToOrderInfo(input);

        // Assert
        Assert.Null(result);
    }
}
