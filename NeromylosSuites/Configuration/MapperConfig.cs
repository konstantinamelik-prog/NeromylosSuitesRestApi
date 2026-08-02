using AutoMapper;
using NeromylosSuites.DTO;
using NeromylosSuites.Models;

namespace NeromylosSuites.Configuration
{
    public class MapperConfig : Profile
    {

        public MapperConfig()
        {
            CreateMap<User, UserReadOnlyDTO>()
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<MemberSignupDTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 1));

            CreateMap<MemberSignupDTO, Member>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Member, MemberReadOnlyDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.User.Firstname))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.User.Lastname));

            CreateMap<Booking, BookingReadOnlyDTO>()
                .ForMember(dest => dest.RoomNames,
                    opt => opt.MapFrom(src => src.Rooms.Select(r => r.Name).ToList()))
                .ForMember(dest => dest.GuestName,
                    opt => opt.MapFrom(src => src.User != null
                        ? $"{src.User.Firstname} {src.User.Lastname}" 
                            : src.Visitor != null 
                                ? $"{src.Visitor.Firstname} {src.Visitor.Lastname}" 
                                : null));

            CreateMap<CreateBookingDTO, Booking>()
                .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.CheckIn!.Value))
                .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.CheckOut!.Value))
                .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.NumberOfGuests!.Value))
                .ForMember(dest => dest.SpecialRequests, opt => opt.MapFrom(src => src.SpecialRequests))
                .ForMember(dest => dest.Rooms, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.VisitorId, opt => opt.Ignore());

            CreateMap<Visitor, VisitorReadOnlyDTO>();

            CreateMap<CreateVisitorDTO, Visitor>();

            CreateMap<Room, RoomReadOnlyDTO>();
        }
    }
}
