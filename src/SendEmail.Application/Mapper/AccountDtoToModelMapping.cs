using AutoMapper;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Model;

namespace SendEmail.Application.Mapper;

public class AccountDtoToModelMapping : Profile
{
    public AccountDtoToModelMapping()
    {
        CreateMap<AccountRequestDTO, Account>()
            .ForMember(dest => dest.NomeCompleto, map => map.MapFrom(src => src.NomeCompleto))
            .ForMember(dest => dest.Documento, map => map.MapFrom(src => src.Documento))
            .ForMember(dest => dest.Email, map => map.MapFrom(src => src.Email))
            .ForMember(dest => dest.Senha, map => map.MapFrom(src => HashPassword(src.Senha)))
        ;
    }

    private string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, 10);
}
