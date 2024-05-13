using ISc.Application.Features.HeadOfCamps.Assigning.Commands.AssignTrainees;
using ISc.Application.Features.HeadOfCamps.Assigning.Commands.UnAssignTrainees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize]
    public class HeadController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public HeadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("assignTraniee")]
        public async Task<ActionResult> AssignTrainee([FromBody] AssignTraineeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("unAssignTrainee")]
        public async Task<ActionResult> UnAssignTrainee([FromBody] string traineeId)
        {
            return Ok(await _mediator.Send(new UnAssignTraineeCommand(traineeId)));
        }
    }
}
