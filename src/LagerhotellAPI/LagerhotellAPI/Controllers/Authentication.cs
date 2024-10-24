using LagerhotellAPI.Models.CustomExceptionModels;
using Microsoft.AspNetCore.Mvc;

namespace LagerhotellAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly AccountManagementService _accountManagementService;

    public AuthenticationController(AccountManagementService accountManagementService)
    {
        _accountManagementService = accountManagementService;
    }

    [HttpPost]
    [Route("start-email-verification")]
    public async Task<IActionResult> StartEmailVerification([FromBody] StartEmailVerificationRequest request)
    {
        await _accountManagementService.StartEmailVerification(request.Email);
        return Ok();
    }

    [HttpPost]
    [Route("verify-email-verification-code")]
    public async Task<IActionResult> VerifyEmailVerificationCode([FromBody] VerifyEmailVerificationCodeRequest request)
    {
        try
        {
            var result = await _accountManagementService.VerifyEmailVerificationCode(request.Email, request.Code);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            return StatusCode(410);
        }
        catch (InvalidVerificationCodeException)
        {
            return BadRequest();
        }
        return Ok();
    }
}
