using AutoMapper;
using CinemaApp.Dto;
using CinemaApp.Models;

namespace CinemaApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Cinema, CinemaDto>();
            CreateMap<CinemaDto, Cinema>();
            CreateMap<CinemaHall, CinemaHallDto>();
            CreateMap<CinemaHallDto, CinemaHall>();
            CreateMap<Seat, SeatDto>();
            CreateMap<SeatDto, Seat>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieDto, Movie>();
            CreateMap<SessionDto, Session>();
            CreateMap<Session, SessionDto>();
        }

    }
}
