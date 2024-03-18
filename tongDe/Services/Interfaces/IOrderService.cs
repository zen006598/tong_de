using tongDe.Services.DTOs.Infos;

namespace tongDe.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(OrderInfo orderInfo);

    OrderInfo ConvertStringToOrderInfo(string unFormattedString);
}
