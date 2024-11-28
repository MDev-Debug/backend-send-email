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
    public async Task AtualizarStatusConta_AccountExists_UpdatesStatusAndReturnsOk()
    {
        // Arrange
        var account = new Account();
        _accountRepositoryMock.Setup(x => x.BuscarContaPorIdentificador("123"))
            .ReturnsAsync(account);

        // Act
        var result = await _accountService.AtualizarStatusConta("123", true);

        // Assert
        _accountRepositoryMock.Verify(x => x.AtualizarStatusConta("123", true), Times.Once);
        Assert.Equal(EResponse.OK, result.Status);
        Assert.Equal("Usuario: John Doe - 123 está: Ativo", result.ResponseData);
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
    public async Task CriarConta_NewAccount_CreatesAccountAndReturnsToken()
    {
        // Arrange
        var accountDTO = new AccountRequestDTO { Documento = "doc123", Email = "email@example.com" };
        var account = new Account();

        _accountRepositoryMock.Setup(x => x.BuscarContaPorDocumentoOuEmail("doc123", "email@example.com"))
            .ReturnsAsync((Account)null);

        _mapperMock.Setup(x => x.Map<Account>(accountDTO)).Returns(account);

        _tokenServiceMock.Setup(x => x.GenerateToken("doc123", false))
            .ReturnsAsync("generated-token");

        // Act
        var result = await _accountService.CriarConta(accountDTO);

        // Assert
        _accountRepositoryMock.Verify(x => x.CriarConta(account), Times.Once);
        Assert.Equal(EResponse.OK, result.Status);
        Assert.NotNull(result.ResponseData);
        Assert.Equal("generated-token", ((dynamic)result.ResponseData).token_access);
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

    [Fact]
    public async Task Login_InvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var account = new Account();
        _accountRepositoryMock.Setup(x => x.BuscarContaPorDocumentoOuEmail("user", "user"))
            .ReturnsAsync(account);

        var loginRequest = new LoginRequestDTO { DocumentoOuEmail = "user", Senha = "wrong-password" };

        // Act
        var result = await _accountService.Login(loginRequest);

        // Assert
        Assert.Equal(EResponse.BAD_REQUEST, result.Status);
        Assert.Equal("Login ou senha invalidos", result.ResponseData);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var account = new Account();
        _accountRepositoryMock.Setup(x => x.BuscarContaPorDocumentoOuEmail("user", "user"))
            .ReturnsAsync(account);

        _tokenServiceMock.Setup(x => x.GenerateToken("doc123", true))
            .ReturnsAsync("generated-token");

        var loginRequest = new LoginRequestDTO { DocumentoOuEmail = "user", Senha = "password" };

        // Act
        var result = await _accountService.Login(loginRequest);

        // Assert
        Assert.Equal(EResponse.OK, result.Status);
        Assert.NotNull(result.ResponseData);
        Assert.Equal("generated-token", ((dynamic)result.ResponseData).token_access);
    }
}
