using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;
using WebAPI_DotNetCore_Demo.Application.Persistence;
using WebAPI_DotNetCore_Demo.Application.Services.Interfaces;
using WebAPI_DotNetCore_Demo.Authorizations;
using WebAPI_DotNetCore_Demo.Options;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;
        public UsersController(IUnitOfWork unitOfWork, IUserService userService, IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticatedUserDto>> AuthenticateUserAsync(
            [FromBody] LoginUserDto loginUserDto, CancellationToken cancellationToken)
        {
            return await _userService.AuthenticateUserAsync(loginUserDto,
                _jwtSettings.Secret, _jwtSettings.ExpiryInMilliseconds,
                _jwtSettings.Issuer, _jwtSettings.Audience, cancellationToken);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return (await _userService.GetAllUsersAsync(cancellationToken)).ToList();
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<UserDto>> GetByIDAsync(Guid userID, CancellationToken cancellationToken)
        {
            return await _userService.GetUserByIDAsync(userID, cancellationToken);
        }

        [HttpGet("username/{userName}")]
        public async Task<ActionResult<UserDto>> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await _userService.GetUserByUserNameAsync(userName, cancellationToken);
        }

        [HttpDelete("{userID}")]
        [Authorize(Policy = Policy.AdminOnly)]
        public async Task<IActionResult> DeleteUserAsync(Guid userID, CancellationToken cancellationToken)
        {
            _userService.SetUserInactive(userID);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Policy = Policy.AdminOnly)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken)
        {
            await _userService.CreateUserAsync(createUserDto, cancellationToken);
            await _unitOfWork.CompleteWithAuditAsync(cancellationToken);

            return NoContent();
        }
    }
}
