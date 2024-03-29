using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using tongDe.Models.ViewModels;

namespace tongDe.Models;

public class AutoMapper : Profile
{
  public AutoMapper()
  {
    CreateMap<ClientCreateVM, Client>();
    CreateMap<ClientEditVM, Client>().ForMember(dest => dest.ShopId, opt => opt.Ignore());
    CreateMap<Client, ClientEditVM>();

    CreateMap<ItemCreateVM, Item>();
    CreateMap<ItemEditVM, Item>()
      .ForMember(dest => dest.ShopId, opt => opt.Ignore())
      .ForMember(dest => dest.Id, opt => opt.Ignore());
    CreateMap<Item, ItemEditVM>();

    CreateMap<Shop, ItemsVM>().ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Id));
    CreateMap<Shop, ClientVM>().ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Id));
    CreateMap<Shop, ItemCategoryVM>().ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.Id));
    CreateMap<Shop, ShopEditVM>();
    CreateMap<ShopEditVM, Shop>();

    CreateMap<ItemAliasCreateVM, ItemAlias>();

    CreateMap<ItemCategoryCreateVM, ItemCategory>();
    CreateMap<ItemCategoryEditVM, ItemCategory>().ForMember(dest => dest.ShopId, opt => opt.Ignore()); ;
    CreateMap<ItemCategory, ItemCategoryEditVM>();

    CreateMap<Shop, ShopDetailsVM>()
             .ForMember(dest => dest.Shop, opt => opt.MapFrom(src => src))
             .ForMember(dest => dest.Clients, opt => opt.MapFrom(src => src.Clients));
  }
}
