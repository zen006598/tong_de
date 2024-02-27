using System.ComponentModel.DataAnnotations;
using tongDe.Data;

namespace tongDe.Models.Validator;

public class UniqueItemNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {

        var logger = validationContext.GetService(typeof(ILogger<UniqueItemNameAttribute>)) as ILogger<UniqueItemNameAttribute>;

        var itemName = value as string;

        if (string.IsNullOrWhiteSpace(itemName)) return new ValidationResult("Item name is required.");

        var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

        if (dbContext is null) return new ValidationResult("Database context is not available.");

        var currentObject = validationContext.ObjectInstance;

        var shopIdProperty = currentObject.GetType().GetProperty("ShopId");


        if (shopIdProperty is null) throw new InvalidOperationException("ShopId not found on validating object.");

        var shopId = (int)shopIdProperty.GetValue(currentObject);

        bool nameExists = dbContext.Items.Any(item => item.Name == itemName && item.ShopId == shopId);

        logger?.LogInformation($"UniqueItemName Verify Result : {nameExists}.");

        if (nameExists)
        {
            return new ValidationResult($"An item with the name \"{itemName}\" already exists.");
        }

        return ValidationResult.Success;
    }
}
