using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using Tasks.Application.Interfaces.Task;
using Tasks.Application.Task.Models;
using Tasks.Application.User.Models;
using Tasks.Domain.Common.Interfaces;

namespace Tasks.API.Controllers.Task
{
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _appService;
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(ITaskService appService, IUnitOfWork unitOfWork)
        {
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("api/tasks")]
        [ProducesResponseType(typeof(Domain.Entities.Task), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CreateTask(
            [FromBody] CreateTaskDTO createTaskDTO,
            CancellationToken cancellationToken = default)
        {
            var result = default(Domain.Entities.Task);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.CreateTask(createTaskDTO.Title, createTaskDTO.Description, createTaskDTO.Assignee, createTaskDTO.DueDate, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Complete();
            }

            return Created(string.Empty, result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully retrieved task.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpGet("api/tasks")]
        [ProducesResponseType(typeof(List<Domain.Entities.Task>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Domain.Entities.Task>>> GetAllTasks(CancellationToken cancellationToken)
        {
            var result = default(List<Domain.Entities.Task>);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.GetAllTasks(cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully retrieved task.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpGet("api/tasks/assignee/{assigneeId}")]
        [ProducesResponseType(typeof(List<Domain.Entities.Task>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Domain.Entities.Task>>> GetAllAssigneeTask([FromRoute] Guid assigneeId, CancellationToken cancellationToken)
        {
            var result = default(List<Domain.Entities.Task>);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.GetAllAssigneeTasks(assigneeId, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully retrieved task.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpGet("api/tasks/{id}")]
        [ProducesResponseType(typeof(Domain.Entities.Task), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Domain.Entities.Task>> GetTask([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = default(Domain.Entities.Task);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.GetTask(id, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully updated task.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPut("api/tasks")]
        [ProducesResponseType(typeof(Domain.Entities.Task), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Domain.Entities.Task>> UpdateUser([FromBody] UpdateTaskDTO updateTaskDTO, CancellationToken cancellationToken)
        {
            var result = default(Domain.Entities.Task);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var task = new Domain.Entities.Task(updateTaskDTO.Title, updateTaskDTO.Description, updateTaskDTO.Assignee, updateTaskDTO.DueDate)
                {
                    Id = updateTaskDTO.Id,
                };

                result = await _appService.UpdateTask(updateTaskDTO.Id, task, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully deleted task.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpDelete("api/tasks/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = default(bool);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _appService.DeleteTask(id, cancellationToken);
                transaction.Complete();
            }

            return Ok(result);
        }
    }
}
