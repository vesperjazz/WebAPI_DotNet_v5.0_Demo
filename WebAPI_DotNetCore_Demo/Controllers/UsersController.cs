using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Users;
using WebAPI_DotNetCore_Demo.Application.Infrastructure;
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
        private readonly IBackgroundJobClient _backgroundJobs;

        public UsersController(IUnitOfWork unitOfWork, IUserService userService,
            IOptionsSnapshot<JwtSettings> jwtSettings, IBackgroundJobClient backgroundJobs)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
            _backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
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

            // Note that the background job below does not have an await keyword as Hangfire takes care of it.
            // The EmailService that is resolved is also in a different scope than the current HttpRequest scope.
            // Not required to inject IEmailService into UsersController, as Hangfire resolves it automatically.
            // In fact, injecting an IEmailService into UserController and then using it in the bottom callback
            // causes 2 instances of EmailServices to be instantiated across 2 different scopes.
            _backgroundJobs.Enqueue<IEmailService>(emailer => emailer
                .SendEmailAsync("Welcome to Ivan Chin's ASP.NET Core v3.1 WebAPI Demo",
                    "This email is sent using MailKit's SmtpClient on Hangfire's background job!",
                    "sender@hotmail.com", new List<string> { "firstRecipient@hotmail.com", "secondRecipient@gmail.com" },
                    null, null, cancellationToken));

            return NoContent();
        }
    }
}
