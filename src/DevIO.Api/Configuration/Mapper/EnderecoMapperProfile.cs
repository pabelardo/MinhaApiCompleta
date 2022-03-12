using AutoMapper;
using DevIO.Api.DTO;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration.Mapper;

public class EnderecoMapperProfile : Profile
{
    public EnderecoMapperProfile()
    {
        CreateMap<Endereco, EnderecoDTO>().ReverseMap();
    }
}