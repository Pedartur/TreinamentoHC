using AutoMapper;
using TreinamentoAPI.Models.DTOs;
using TreinamentoAPI.Models;

namespace ContatoAPI.Mappings
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
            // Mapeamento de Contato para ContatoDto
            CreateMap<Contato, ContatoDto>().ReverseMap();

            // Mapeamento de CriarContatoDto para Contato
            CreateMap<CriarContatoDto, Contato>();

            // Mapeamento de AtualizarContatoDto para Contato
            CreateMap<AtualizarContatoDto, Contato>();
        }
    }
}