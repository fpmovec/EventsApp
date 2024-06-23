using Application.Models;
using AutoMapper;
using Domain.Models;
using Web.ViewModels;

namespace Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventViewModel, EventExtendedModel>().ReverseMap();
            CreateMap<FilterOptionsViewModel, List<FilterOption>>()
                .ConvertUsing<FilterOptionsConverter>();
        }
    }
}
