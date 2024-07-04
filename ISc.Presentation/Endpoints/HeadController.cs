using ISc.Application.Features.HeadOfCamps.Assigning.Commands.AssignTrainees;
using ISc.Application.Features.HeadOfCamps.Assigning.Commands.UnAssignTrainees;
using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetMentorAssign;
using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssignWithPagination;
using ISc.Application.Features.HeadOfCamps.Attendance.Queries.GetAllAttendance;
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

        [HttpGet("assign/trainees")]
        public async Task<ActionResult<List<GetTraineeAssignWithPaginationQueryDto>>> GetUnAssignedTrainees([FromQuery] GetTraineeAssignWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("assign/mentors")]
        public async Task<ActionResult<List<GetMentorAssignQueryDto>>> GetMentorForAssign() 
        {
            return Ok(await _mediator.Send(new GetMentorAssignQuery()));
        }

        [HttpGet("attendance")]
        public async Task<ActionResult<GetAllAttendanceQueryDto>> GetAllAttendance()
        {
            return Ok(await _mediator.Send(new GetAllAttendanceQuery()));
        }

    }
}
