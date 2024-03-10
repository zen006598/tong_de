using System.ComponentModel.DataAnnotations;
using tongDe.Data;
namespace tongDe.Models.Validator;

public class UniqueItemAndItemAliasNameAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    int shopId;
    var logger = validationContext.GetService(typeof(ILogger<UniqueItemAndItemAliasNameAttribute>)) as ILogger<UniqueItemAndItemAliasNameAttribute> ?? throw new ArgumentException("logger is not available.");
    var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext ?? throw new ArgumentException("Database context is not available.");
    string unValidatedName = (string)value;
    var currentObject = validationContext.ObjectInstance;
    var idProperty = currentObject.GetType().GetProperty("Id");
    var id = idProperty is not null ? (int?)idProperty.GetValue(currentObject) : null;

    shopId = currentObject.GetType().GetProperty("ShopId")?.GetValue(validationContext.ObjectInstance) as int? ?? 0;
    // if Shop id is 0 the object must be the item alias
    if (shopId is 0)
    {
      int itemId = currentObject.GetType().GetProperty("ItemId")?.GetValue(validationContext.ObjectInstance) as int? ?? 0;
      if (itemId is 0) return new ValidationResult("The item no found.");

      shopId = dbContext.Items.Where(i => i.Id == itemId).Select(i => i.ShopId).FirstOrDefault();
    }

    if (shopId is 0) return new ValidationResult("The shop no found.");

    bool itemNameExists = dbContext.Items.Any(i =>
      i.Name == unValidatedName &&
      i.ShopId == shopId &&
      (!id.HasValue || i.Id != id.Value));

    if (itemNameExists) return new ValidationResult("The name is already in use by an item.");

    bool itemAliasExists = dbContext.ItemAliases
       .Any(ia => ia.Name == unValidatedName && ia.Item.ShopId == shopId);

    if (itemAliasExists) return new ValidationResult("The name is already in use by an itemAlias.");

    return ValidationResult.Success;
  }
}