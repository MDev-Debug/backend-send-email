using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Interfaces.Services;

namespace SendEmail.API.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerExtension
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("atualizar-status-conta")]
        public async Task<IActionResult> AtualizarStatusConta([FromBody] AtualizarContaRequestDTO request)
        {
            var result = await _accountService.AtualizarStatusConta(request.IdentificadorConta, request.StatusAtivo);
            return ResponseHTTP(result);
        }
    }
}
