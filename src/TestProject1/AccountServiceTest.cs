using AutoMapper;
using FluentAssertions;
using Moq;
using SendEmail.Domain.Enum;
using SendEmail.Domain.Interfaces.Repositories;
using SendEmail.Domain.Interfaces.Services;
using SendEmail.Domain.Model;
using SendEmail.Domain.Services;

namespace TestProject1;
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
    }

    [Fact]
    public async Task AtualizarStatusConta_ShouldReturnSuccessResponse_WhenAccountExists()
    {
        var conta = new Account("12345678912", "Joao", "teste@gmail.com", "123@3..aBc", "11985869974");
        var identificador = Guid.NewGuid().ToString();

        _accountRepositoryMock
            .Setup(x => x.BuscarContaPorIdentificador(identificador))
            .ReturnsAsync(conta);

        _accountRepositoryMock.Setup(x => x.AtualizarStatusConta(identificador, true));
        var response = await _accountService.AtualizarStatusConta(identificador, true);

        _accountRepositoryMock.Verify(x => x.BuscarContaPorIdentificador(identificador), Times.Once);
        _accountRepositoryMock.Verify(x => x.AtualizarStatusConta(identificador, true), Times.Once);
        Assert.NotNull(response);
        response.Status.Should().Be(EResponse.OK);
    }

    [Fact]
    public async Task AtualizarStatusConta_ShouldReturnSuccessResponse_WhenAccountNotExists()
    {
        var identificador = Guid.NewGuid().ToString();

        _accountRepositoryMock
            .Setup(x => x.BuscarContaPorIdentificador(identificador));

        var response = await _accountService.AtualizarStatusConta(identificador, true);

        _accountRepositoryMock.Verify(x => x.BuscarContaPorIdentificador(identificador), Times.Once);
        _accountRepositoryMock.Verify(x => x.AtualizarStatusConta(identificador, true), Times.Never);
        Assert.NotNull(response);
        response.Status.Should().Be(EResponse.BAD_REQUEST);
    }
}
