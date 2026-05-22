using AutoMapper;
using TreinamentoAPI.Models.DTOs;
using TreinamentoAPI.Models;

namespace ProdutoAPI.Mappings
{
    /// <summary>
    /// Perfil de mapeamento AutoMapper
    /// Responsabilidade: Converter Entities em DTOs e vice-versa
    /// Benefício: Evita expor a estrutura interna das entidades e facilita manutenção
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de Produto para ProdutoDto
            CreateMap<Produto, ProdutoDto>().ReverseMap();

            // Mapeamento de CriarProdutoDto para Produto
            CreateMap<CriarProdutoDto, Produto>();

            // Mapeamento de AtualizarProdutoDto para Produto
            CreateMap<AtualizarProdutoDto, Produto>();
        }
    }
}