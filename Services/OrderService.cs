using System.Text.RegularExpressions;
using tongDe.Data.Repository.Interfaces;
using tongDe.Models;
using tongDe.Services.DTOs.Infos;
using tongDe.Services.Interfaces;

namespace tongDe.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _order;

    public OrderService(IOrderRepository orderRepository)
    {
        _order = orderRepository;
    }
    public Task<int> CreateOrderAsync(OrderInfo orderInfo)
    {
        throw new NotImplementedException();
    }

    public OrderInfo StringProcess(string unFormattedString)
    {
        string[] splittedWords = unFormattedString.Split('\n');

        OrderInfo order = new OrderInfo()
        {
            ClientName = splittedWords[0],
        };

        //get the words after the first string
        foreach (var item in splittedWords.Skip(1))
        {
            var parts = Regex.Match(item, @"(\D+)\s*(\d+)\s*(.+)");
            if (!parts.Success) continue;

            var orderItem = new OrderItem
            {
                Name = parts.Groups[1].Value.Trim(),
                Quantity = int.Parse(parts.Groups[2].Value),
                Unit = parts.Groups[3].Value.Trim()
            };
            order.OrderItems.Add(orderItem);
        }
        return order;
    }
}
