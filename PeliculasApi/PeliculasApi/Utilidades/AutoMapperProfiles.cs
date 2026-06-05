using AutoMapper;
using PeliculasApi.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasApi.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeoGenero();
        }

        private void ConfigurarMapeoGenero()
        {
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();
        }
    }
}
