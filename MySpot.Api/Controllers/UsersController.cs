using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class UsersController : ControllerBase
{
    private readonly ICommandHandler<SignUp> _signUpHandler;
    private readonly ICommandHandler<SignIn> _signInHandler;
    private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsersHandler;
    private readonly IQueryHandler<GetUser, UserDto> _getUserHandle;
    private readonly ITokenStorage _tokenStorage;


    public UsersController
    (
        ICommandHandler<SignUp> signUpHandler,
        IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
        IQueryHandler<GetUser, UserDto> getUserHandle, IAuthenticator authenticator, 
        ICommandHandler<SignIn> signInHandler,
        ITokenStorage tokenStorage
    )
    {
        _signUpHandler = signUpHandler;
        _getUsersHandler = getUsersHandler;
        _getUserHandle = getUserHandle;
        _signInHandler = signInHandler;
        _tokenStorage = tokenStorage;
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = "is-admin")]
    public async Task<ActionResult<UserDto>> Get(Guid userId)
    {
        var user = await _getUserHandle.HandleAsync(new GetUser() { UserId = userId });
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpGet]
    [Authorize(Policy = "is-admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query)
        => Ok(await _getUsersHandler.HandleAsync(query));

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<JwtDto>> Get()
    {
        if (string.IsNullOrEmpty(HttpContext.User.Identity?.Name))
        {
            return NotFound();
        }

        var userId = Guid.Parse(HttpContext.User.Identity.Name);
        var user = await _getUserHandle.HandleAsync(new GetUser() { UserId = userId });
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }


    [HttpPost]
    public async Task<ActionResult> Post(SignUp command)
    {
        command = command with { UserId = Guid.NewGuid() };
        await _signUpHandler.HandleAsync(command);

        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<JwtDto>> Post(SignIn command)
    {
        await _signInHandler.HandleAsync(command);
        var accessToken = _tokenStorage.Get();
        return Ok(accessToken);
    }
}