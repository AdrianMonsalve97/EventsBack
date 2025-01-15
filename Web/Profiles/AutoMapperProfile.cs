using AutoMapper;
using EventsApi.Domain.Entities;
using EventsApi.Models;
using EventsApi.Models.DTO;

namespace EventsApi.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Evento, EventoDto>();
            CreateMap<Inscripcion, UsuarioInscritoDto>()
                .ForMember(dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.Correo,
                    opt => opt.MapFrom(src => src.Usuario.CorreoCorporativo));

            CreateMap<Usuario, UsuarioDto>();
            CreateMap<Inscripcion, InscripcionDto>()
                .ForMember(dest => dest.EventoNombre,
                    opt => opt.MapFrom(src => src.Evento.Nombre));
        }
    }
}
