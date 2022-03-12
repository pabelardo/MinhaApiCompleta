using AutoMapper;
using DevIO.Api.DTO;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration.Mapper;

public class ProdutoMapperProfile : Profile
{
    public ProdutoMapperProfile()
    {
        CreateMap<Produto, ProdutoDTO>()
            .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome))
            .ReverseMap();   
    }
}