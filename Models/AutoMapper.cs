using AutoMapper;
using tongDe.Models.ViewModels;

namespace tongDe.Models;

public class AutoMapper : Profile
{
  public AutoMapper()
  {
    CreateMap<ClientCreateVM, Client>();
    CreateMap<ClientEditVM, Client>().ForMember(dest => dest.ShopId, opt => opt.Ignore());
    CreateMap<Client, ClientEditVM>();
  }
}
