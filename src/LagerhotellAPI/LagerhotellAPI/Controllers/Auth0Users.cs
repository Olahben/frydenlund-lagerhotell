global using LagerhotellAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LagerhotellAPI.Controllers;

[ApiController]
[Route("auth0-users")]
public class Auth0UsersController : ControllerBase
{
    private readonly Auth0UserService _auth0UserService;
    private readonly RefreshTokens _refreshTokensRepository;
    private readonly ICompanyUserService _companyUserService;
    private readonly UserRepository _userRepository;
    private readonly ILogger<Auth0UsersController> _logger;
    public Auth0UsersController(Auth0UserService auth0UserService, RefreshTokens refreshTokensRepository, ICompanyUserService companyUserService, UserRepository userRepository, ILogger<Auth0UsersController> logger)
    {
        _auth0UserService = auth0UserService;
        _refreshTokensRepository = refreshTokensRepository;
        _companyUserService = companyUserService;
        _userRepository = userRepository;
        _logger = logger;
    }

    // Endpoints are going to be protected with authorization after auth0 tokens are integrated
    // For now, they are open for testing purposes
    // Endpoints do not currently have request models or response models, this will be implemented and validated later
    [HttpGet]
    [Route("get-user/{auth0UserId}")]
    public async Task<IActionResult> GetUser([FromRoute] string auth0UserId)
    {
        try
        {
            var user = await _auth0UserService.GetCompleteUser(auth0UserId);
            return Ok(new GetAuth0UserResponse(user));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.GetUser");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    // Endpoint only used for testing purposes
    [HttpPost]
    [Route("signup-user")]
    public async Task<IActionResult> SignupUser([FromBody] SignupUserAuth0Request user)
    {
        try
        {
            await _auth0UserService.AddUser(user.user, false, false);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.SignupUser");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    [Route("delete-user")]
    public async Task<IActionResult> DeleteUser([FromBody] Auth0IdRequest request)
    {
        try
        {
            var user = await _userRepository.GetByAuth0Id(request.Auth0Id);
            try
            {
                await _userRepository.DeleteUser(user.Id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
        }
        catch (KeyNotFoundException)
        {
            var user = await _companyUserService.GetUserByAuth0Id(request.Auth0Id);
            try
            {
                await _companyUserService.DeleteCompanyUserAsync(user.CompanyUserId);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.DeleteUser");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        try
        {
            await _auth0UserService.DeleteUser(request.Auth0Id);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.DeleteUser");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPatch]
    [Route("update-and-verify-user-email")]
    public async Task<IActionResult> UpdateAndVerifyUserEmail([FromQuery] string auth0UserId, [FromBody] UpdateAndVerifyUserEmailRequest request)
    {
        try
        {
            await _auth0UserService.UpdateAndVerifyEmail(auth0UserId, request.email);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.UpdateAndVerifyUserEmail");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Endpoint for updating a users password when the user has not forgotten the password
    /// </summary>
    /// <param name="auth0UserId"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("change-user-password")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordRequest request)
    {
        try
        {
            await _auth0UserService.IsLoginCredentialsCorrect(request.Email, request.OldPassword);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Old password is incorrect");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.ChangeUserPassword");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
        try
        {
            await _auth0UserService.ChangeUserpassword(request.Auth0Id, request.NewPassword);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.ChangeUserPassword");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("send-forgot-password-email")]
    public async Task<IActionResult> SendForgotPasswordEmail([FromBody] SendForgotPasswordEmailRequest request)
    {
        try
        {
            await _auth0UserService.SendForgotPasswordEmail(request.email);
            return Created();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.SendForgotPasswordEmail");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("exchange-code-for-tokens")]
    public async Task<IActionResult> ExchangeCodeForTokens([FromBody] ExchangeCodeForTokensRequest request)
    {
        // Gets tokens based on code
        // Saves refresh token in database
        // Returns access token to frontend
        try
        {
            (string accessToken, string refreshToken) = await _auth0UserService.ExchangeCodeForTokens(request.Code);
            string auth0Id = await _auth0UserService.GetUserIdViaToken(accessToken);
            UserAuth0 user = await _auth0UserService.GetCompleteUser(auth0Id);
            await _refreshTokensRepository.CreateRefreshToken(new RefreshTokenDocument(refreshToken, auth0Id));
            return Ok(new ExchangeCodeForTokensResponse(accessToken, user.UserId));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (TooManyRequestsException e)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Auth0UsersController.ExchangeCodeForTokens");
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshAccessTokenRequest request)
    {
        try
        {
            RefreshTokenDocument? refreshTokenDocument = await _refreshTokensRepository.GetRefreshToken(request.Auth0Id);
            if (refreshTokenDocument == null)
            {
                return NotFound("Refresh token not found");
            }
            string newAccessToken = await _auth0UserService.RefreshAccessToken(refreshTokenDocument.RefreshToken);
            return Ok(new RefreshAccessTokenResponse(newAccessToken));
        }
        catch (HttpRequestException e)
        {
            if (e.Message.Contains("400"))
            {
                return BadRequest("Refresh token is invalid");
            }
            else if (e.Message.Contains("401"))
            {
                return Unauthorized("Refresh token is invalid");
            }
            _logger.LogError(e, "Error in Auth0UsersController.RefreshAccessToken");
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    [Route("send-verification-email")]
    public async Task<IActionResult> SendVerificationEmail([FromBody] SendVerificationEmailRequest request)
    {
        try
        {
            await _auth0UserService.SendVerificationEmail(request.Auth0Id);
            return Created();
        }
        catch (HttpRequestException e)
        {
            if (e.Message.Contains("400"))
            {
                return BadRequest(e.Message);
            }
            else if (e.Message.Contains("401"))
            {
                return Unauthorized(e.Message);
            }
            else if (e.Message.Contains("429"))
            {
                return StatusCode(429, e.Message);
            }
            else if (e.Message.Contains("403"))
            {
                return StatusCode(403, e.Message);
            }
            _logger.LogError(e, "Error in Auth0UsersController.SendVerificationEmail");
            return StatusCode(500, e.Message);
        }
    }

}
