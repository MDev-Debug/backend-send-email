using Xunit;
using Moq;
using System.Threading.Tasks;
using SendEmail.Domain.Interfaces.Repositories;
using SendEmail.Domain.Interfaces.Services;
using SendEmail.Domain.DTOs;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Model;
using SendEmail.Domain.Enum;
using SendEmail.Domain.Services;
using AutoMapper;
using System.Net;
using System.Security.Principal;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ICorrelationId> _correlationIdMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _correlationIdMock = new Mock<ICorrelationId>();
        _mapperMock = new Mock<IMapper>();
        _tokenServiceMock = new Mock<ITokenService>();

        _accountService = new AccountService(
            _accountRepositoryMock.Object,
            _correlationIdMock.Object,
            _mapperMock.Object,
            _tokenServiceMock.Object
        );

        // Default correlation ID
        _correlationIdMock.Setup(x => x.ObterCorrelationId()).Returns("correlation-id");
    }

    [Fact]
    public async Task AtualizarStatusConta_AccountNotFound_ReturnsBadRequest()
    {
        // Arrange
        _accountRepositoryMock.Setup(x => x.BuscarContaPorIdentificador("123"))
            .ReturnsAsync((Account)null);

        // Act
        var result = await _accountService.AtualizarStatusConta("123", true);

        // Assert
        Assert.Equal(EResponse.BAD_REQUEST, result.Status);
        Assert.Equal("Account not found", result.ResponseData);
    }

    [Fact]
    public async Task CriarConta_AccountAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        _accountRepositoryMock.Setup(x => x.BuscarContaPorDocumentoOuEmail("doc123", "email@example.com"))
            .ReturnsAsync(new Account());

        var accountDTO = new AccountRequestDTO { Documento = "doc123", Email = "email@example.com" };

        // Act
        var result = await _accountService.CriarConta(accountDTO);

        // Assert
        Assert.Equal(EResponse.BAD_REQUEST, result.Status);
        Assert.Equal("Account already exists", result.ResponseData);
    }

    [Fact]
    public async Task Login_InvalidAccount_ReturnsBadRequest()
    {
        // Arrange
        _accountRepositoryMock.Setup(x => x.BuscarContaPorDocumentoOuEmail("invalid", "invalid"))
            .ReturnsAsync((Account)null);

        var loginRequest = new LoginRequestDTO { DocumentoOuEmail = "invalid", Senha = "password" };

        // Act
        var result = await _accountService.Login(loginRequest);

        // Assert
        Assert.Equal(EResponse.BAD_REQUEST, result.Status);
        Assert.Equal("Account not found", result.ResponseData);
    }
}
