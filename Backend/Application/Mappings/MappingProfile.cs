using AutoMapper;
using Domain.Models;
using Web.DTO;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventDTO, EventExtendedModel>()
                .ReverseMap();

            CreateMap<FilterOptionsDTO, List<FilterOption>>()
                .ConvertUsing<FilterOptionsConverter>();

            CreateMap<BookingDTO, Booking>()
                .ForMember(b => b.CreatedDate, ops => ops.MapFrom(b => DateTime.Now));
        }
    }
}
