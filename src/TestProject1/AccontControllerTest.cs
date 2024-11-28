using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendEmail.API.Controllers.Account;
using SendEmail.Domain.DTOs;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Enum;
using SendEmail.Domain.Interfaces.Services;
using Xunit;

namespace SendEmail.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _controller = new AccountController(_accountServiceMock.Object);
        }

        [Fact]
        public async Task AtualizarStatusConta_RetornaBadRequest_QuandoServicoFalha()
        {
            // Arrange
            var request = new AtualizarContaRequestDTO
            {
                IdentificadorConta = "123",
                StatusAtivo = true
            };

            var serviceResult = new
            {
                Success = false,
                Message = "Falha ao atualizar status da conta"
            };

            _accountServiceMock
                .Setup(x => x.AtualizarStatusConta("enfiwengeorig", true))
                .ReturnsAsync(new Response("", new {}, EResponse.OK));

            // Act
            var result = await _controller.AtualizarStatusConta(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(serviceResult, badRequestResult.Value);
        }
    }
}
