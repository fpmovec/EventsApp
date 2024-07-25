using AutoMapper;
using Domain.Models;
using Web.ViewModels;

namespace Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventViewModel, EventExtendedModel>()
                .ReverseMap();

            CreateMap<FilterOptionsViewModel, List<FilterOption>>()
                .ConvertUsing<FilterOptionsConverter>();

            CreateMap<BookingViewModel, Booking>()
                .ForMember(b => b.CreatedDate, ops => ops.MapFrom(b => DateTime.Now));
        }
    }
}
