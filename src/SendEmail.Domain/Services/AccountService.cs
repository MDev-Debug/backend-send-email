using AutoMapper;
using SendEmail.Domain.DTOs;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Enum;
using SendEmail.Domain.Interfaces.Repositories;
using SendEmail.Domain.Interfaces.Services;
using SendEmail.Domain.Model;

namespace SendEmail.Domain.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICorrelationId _correlationId;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    public AccountService(IAccountRepository accountRepository,
                          ICorrelationId correlationId,
                          IMapper mapper,
                          ITokenService tokenService)
    {
        _accountRepository = accountRepository;
        _correlationId = correlationId;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<Response> AtualizarStatusConta(string identificadorConta, bool statusAtivo)
    {
        var account = await _accountRepository.BuscarContaPorIdentificador(identificadorConta);

        if (account == null)
            return new Response(_correlationId.ObterCorrelationId(), "Account not found", EResponse.BAD_REQUEST);

        await _accountRepository.AtualizarStatusConta(identificadorConta, statusAtivo);

        var status = statusAtivo == true ? "Ativo" : "Inativo";
        return new Response(_correlationId.ObterCorrelationId(), $"Usuario: {account.NomeCompleto} - {identificadorConta} está: {status}", EResponse.OK);
    }

    public async Task<Response> CriarConta(AccountRequestDTO accountDTO)
    {
        var accountExists = await _accountRepository.BuscarContaPorDocumentoOuEmail(accountDTO.Documento, accountDTO.Email);

        if (accountExists != null)
            return new Response(_correlationId.ObterCorrelationId(), "Account already exists", EResponse.BAD_REQUEST);

        var account = _mapper.Map<Account>(accountDTO);
        await _accountRepository.CriarConta(account);

        var token = await _tokenService.GenerateToken(account.Documento, account.Admin);
        return new Response(_correlationId.ObterCorrelationId(), new { token_access = token, admin = account.Admin }, EResponse.OK);
    }

    public async Task<Response> Login(LoginRequestDTO request)
    {
        var account = await _accountRepository.BuscarContaPorDocumentoOuEmail(request.DocumentoOuEmail, request.DocumentoOuEmail);

        if (account == null)
            return new Response(_correlationId.ObterCorrelationId(), "Account not found", EResponse.BAD_REQUEST);

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Senha, account.Senha);

        if (!passwordValid)
            return new Response(_correlationId.ObterCorrelationId(), "Login ou senha invalidos", EResponse.BAD_REQUEST);

        var token = await _tokenService.GenerateToken(account.Documento, account.Admin);
        return new Response(_correlationId.ObterCorrelationId(), new { token_access = token, admin = account.Admin }, EResponse.OK);
    }
}
