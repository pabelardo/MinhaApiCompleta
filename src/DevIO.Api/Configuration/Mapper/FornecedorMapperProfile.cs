using AutoMapper;
using DevIO.Api.DTO;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration.Mapper;

public class FornecedorMapperProfile : Profile
{
    public FornecedorMapperProfile()
    {
        CreateMap<Fornecedor, FornecedorDTO>().ReverseMap();
    }
}