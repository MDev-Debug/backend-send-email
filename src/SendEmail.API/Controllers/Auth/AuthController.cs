using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Domain.DTOs.Request;
using SendEmail.Domain.Interfaces.Services;

namespace SendEmail.API.Controllers.Auth;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerExtension
{
    private readonly IAccountService _accountService;
    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountRequestDTO account)
    {
        var result = await _accountService.CriarConta(account);
        return ResponseHTTP(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var result = await _accountService.Login(request);
        return ResponseHTTP(result);
    }
}
