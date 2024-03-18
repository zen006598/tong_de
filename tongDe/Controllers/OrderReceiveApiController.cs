using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using tongDe.Data;
using tongDe.Models;

namespace tongDe.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrderReceiveApiController : ControllerBase
{
    private readonly ILogger<OrderReceiveApiController> _logger;

    public OrderReceiveApiController(ILogger<OrderReceiveApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet("ShopAuth")]
    public async Task<ActionResult> ShopAuth()
    {
        return Ok();
    }

    [HttpPost("ReceiveOrder")]
    public async Task<ActionResult> ReceiveOrder(LineBotMessage lineBotMessage)
    {
        //驗證token(Shop.Token) IShopService => verify token
        //找到Shop IShopRepository => get Shop
        //驗證傳入非空字串
        //字串拆分（[^\\w\\s]）
        //第一字串為client Name
        //將剩餘string[] 做逐一的拆分，個別抓中文字元 與 數字 預設會為['品名', '數量', '單位'] ([\u4e00-\u9fa5]+)|(\d+)|([^\d\u4e00-\u9fa5]+)
        //拆分後驗證品項存在(item, itemAlias)
        //如存在於item
        //  判斷統一品項(itemTag)
        //      做成orderItem { Name: "高麗菜", quantity: 20, unit: "斤", exist: true, itemTag: ""}
        //      做成orderItem { Name: "豆乾", quantity: 5, unit: "斤", exist: true, itemTag: "豆乾"}

        //如存在於itemAlias("蘿蔔") 取的itemName("白蘿蔔")
        //  判斷統一品項(itemTag)
        //      做成orderItem {Name: "白蘿蔔", quantity: 3, unit: "件", exist: true, itemTag: ""}

        //如不存在於item與itemAlias  做成orderItem {Name: "新品項", quantity: 20, unit: "斤", exist: false, itemTag: ""}
        //在所屬的Shop成立Order {client: "clientName", items: [orderItem1, orderItem2]}
        _logger.LogInformation($"Received order: {lineBotMessage.Message}");

        return Ok();
    }

}
