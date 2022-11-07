using AbiClassLib.ProjectDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Models;
using ProjectAPI.Services;
using System.Net;
using System.Text.Json;

namespace ProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] UserAuthModelSignup model)
    {
        int matchCount = await _userService.GetActiveUserCountByEmail(model.Email);
        if (matchCount > 0)
        {
            return StatusCode((int)HttpStatusCode.MultipleChoices,
                "Email already used for an active account.");
        }

        string activationCode = await _userService.Singup(model);

        model.Password = activationCode;
        string ipAddressString = $"{Request.HttpContext.Connection.RemoteIpAddress}  |  {Request.Headers["cl-id"]}";
        _userService.RecordActivity(0, ipAddressString, "User Record Created: " + JsonSerializer.Serialize(model));
        Util.SendActivationConfirmRequestEmailviaGmail(model);
        _userService.RecordActivity(0, ipAddressString, $"Activation request email sent to {model.Email}");
        return Ok("{}");
    }

    
    [HttpPost("signup/confirm/{activactionCode}")]
    public async Task<IActionResult> ConfirmSignup(string activactionCode)
    {
        var user = await _userService.ConfirmSingup(activactionCode);
        string ipAddressString = $"{Request.HttpContext.Connection.RemoteIpAddress}  |  {Request.Headers["cl-id"]}";
        if (user is not null)
        {
            _userService.RecordActivity(0, ipAddressString, $"User account activated. Activation code: {activactionCode}");
            UserAuthModelSignup activatedUser = new()
            {
                Email = user.Email,
                FullName = user.FullName,
            };

            Util.SendActivationConfirmEmailviaGmail(activatedUser);
            _userService.RecordActivity(0, ipAddressString, $"Activation confirmation email sent to {user.Email}");
            return Ok(activatedUser);
        }

        return Ok();
    }

    
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthModel model)
    {
        var user = await _userService.Authenticate(model.Username, model.Password);

        if (user == null)
        {
            return Unauthorized("Authentication failed. Please check credentials and try again.");
            // return BadRequest(new { message = "Username or password is incorrect" });
        }

        string ipAddressString = $"{Request.HttpContext.Connection.RemoteIpAddress}  |  {Request.Headers["cl-id"]}";
        _userService.RecordActivity(user.UserId.Value, ipAddressString, Request.Headers["User-Agent"].ToString());

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }
}
