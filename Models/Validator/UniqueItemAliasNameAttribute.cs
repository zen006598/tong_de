using System.ComponentModel.DataAnnotations;
using tongDe.Data;

namespace tongDe.Models.Validator;
public class UniqueItemAliasNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var logger = validationContext.GetService(typeof(ILogger<UniqueItemNameAttribute>)) as ILogger<UniqueItemNameAttribute> ?? throw new ArgumentException("logger is not available.");

        var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext ?? throw new ArgumentException("Database context is not available.");
        var itemAliasName = value as string;

        var itemIdProperty = validationContext.ObjectInstance.GetType().GetProperty("ItemId");
        var itemId = itemIdProperty is not null ? (int?)itemIdProperty.GetValue(validationContext.ObjectInstance) : 0;
        var shopId = dbContext.Items.Where(i => i.Id == itemId).Select(i => i.ShopId).FirstOrDefault();

        bool itemNameExists = dbContext.Items.Any(i => i.Name == itemAliasName && i.ShopId == shopId);

        logger?.LogInformation("Validating item name: {itemAliasName} in shopId: {ShopId} with itemId: {ItemId}. UniqueItemAliasNameAttribute Verify Result: {itemNameExists}.", itemAliasName, shopId, itemId, itemNameExists);

        if (itemNameExists)
        {
            return new ValidationResult($"An item with the name \"{itemAliasName}\" already exists in the shop.");
        }

        bool itemAliasExists = dbContext.ItemAliases
            .Any(ia => ia.Name == itemAliasName && ia.Item.ShopId == shopId);

        logger?.LogInformation("Validating item name: {itemAliasName} in shopId: {ShopId} with itemId: {ItemId}. UniqueItemAliasNameAttribute Verify Result: {itemAliasExists}.", itemAliasName, shopId, itemId, itemAliasExists);

        if (itemAliasExists)
        {
            return new ValidationResult($"An item alias with the name \"{itemAliasName}\" already exists in the shop.");
        }

        return ValidationResult.Success;
    }
}

