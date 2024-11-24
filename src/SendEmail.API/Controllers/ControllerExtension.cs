using Microsoft.AspNetCore.Mvc;
using SendEmail.Domain.DTOs;
using SendEmail.Domain.Enum;

namespace SendEmail.API.Controllers;

public class ControllerExtension : ControllerBase
{
    protected IActionResult ResponseHTTP(Response result)
        => result?.Status switch
        {
            EResponse.BAD_REQUEST => BadRequest(result),
            EResponse.NOT_FOUND => NotFound(result),
            EResponse.UNAUTHORIZED => Unauthorized(result),
            EResponse.INTERNAL_SERVER_ERROR => StatusCode(500, result),
            _ => Ok(result)
        };
}
