using System.ComponentModel.DataAnnotations;
using tongDe.Data;

namespace tongDe.Models.Validator;

public class UniqueItemNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {

        var logger = validationContext.GetService(typeof(ILogger<UniqueItemNameAttribute>)) as ILogger<UniqueItemNameAttribute>;

        var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

        if (dbContext is null) return new ValidationResult("Database context is not available.");

        var itemName = value as string;

        if (string.IsNullOrWhiteSpace(itemName)) return new ValidationResult("Item name is required.");

        var currentObject = validationContext.ObjectInstance;
        var shopIdProperty = currentObject.GetType().GetProperty("ShopId");
        var itemIdProperty = currentObject.GetType().GetProperty("Id");

        if (shopIdProperty is null) throw new InvalidOperationException("ShopId not found on validating object.");

        var shopId = (int?)shopIdProperty.GetValue(currentObject);
        var itemId = itemIdProperty is not null ? (int?)itemIdProperty.GetValue(currentObject) : null;

        bool nameExists = dbContext.Items.Any(item =>
            item.Name == itemName &&
            item.ShopId == shopId &&
            (!itemId.HasValue || item.Id != itemId.Value));

        logger?.LogInformation("Validating item name: {ItemName} in shopId: {ShopId} with itemId: {ItemId}. UniqueItemName Verify Result: {NameExists}.", itemName, shopId, itemId, nameExists);

        if (nameExists)
        {
            return new ValidationResult($"An item with the name \"{itemName}\" already exists.");
        }

        return ValidationResult.Success;
    }
}
