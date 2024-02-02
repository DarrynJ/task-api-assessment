using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using Tasks.Domain.Common.Interfaces;
using Tasks.Application.User.Models;
using Tasks.Application.Interfaces.User;

namespace Tasks.API.Controllers.User
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _appService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserService appService, IUnitOfWork unitOfWork)
        {
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully logged in.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("api/users/login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> LoginUser(
            [FromBody] LoginUserDTO loginUserDTO,
            CancellationToken cancellationToken)
        {
            var result = default(string);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.LoginUser(loginUserDTO.Username, loginUserDTO.Password, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("api/users")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateUser(
            [FromBody] CreateUserDTO createUserDTO,
            CancellationToken cancellationToken = default)
        {
            var result = default(bool);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.CreateUser(createUserDTO.Username, createUserDTO.EmailAddress, createUserDTO.Password, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }

            return Created(string.Empty, result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully retrieved user.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpGet("api/users/{id}")]
        [ProducesResponseType(typeof(Domain.Entities.User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Domain.Entities.User>> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = default(Domain.Entities.User);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.GetUser(id, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully updated user.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPut("api/users")]
        [ProducesResponseType(typeof(Domain.Entities.User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Domain.Entities.User>> UpdateUser([FromBody] UpdateUserDTO updateUserDTO, CancellationToken cancellationToken)
        {
            var result = default(Domain.Entities.User);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = new Domain.Entities.User(updateUserDTO.Username, updateUserDTO.EmailAddress, updateUserDTO.Password)
                {
                    Id = updateUserDTO.Id,
                };

                result = await _appService.UpdateUser(user, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully deleted user.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpDelete("api/users/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = default(bool);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.DeleteUser(id, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }
    }
}
