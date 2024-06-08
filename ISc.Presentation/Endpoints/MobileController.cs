using ISc.Application.Features.Mobile.Commands.AddAttendnce;
using ISc.Application.Features.Mobile.Commands.UpdateTraineePoints;
using ISc.Application.Features.Mobile.Queries.GetCamps;
using ISc.Application.Features.Mobile.Queries.GetPresistanceTrainees;
using ISc.Application.Features.Mobile.Queries.GetTraineesByCampId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    public class MobileController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getTraineesByCampId")]
        public async Task<ActionResult<GetTraineesByCampIdQueryDto>> GetTraineesByCampId(GetTraineesByCampIdQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("getCamps")]
        public async Task<ActionResult<GetCampsQueryDto>> ForgetPassword(GetCampsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("addTraineeToAttendence/{traineeId}")]
        public async Task<ActionResult<string>> ResetPassword(string traineeId)
        {
            return Ok(await _mediator.Send(new AddTraineeToAttendenceCommand(traineeId)));
        }

        [HttpGet("GetPresentTrainees/{campId}")]
        public async Task<ActionResult<List<GetPresentTraineesQueryDto>>> GetPresentTrainee(int campId)
        {
            return Ok(await _mediator.Send(new GetPresentTraineesQuery(campId)));
        }

        [HttpPut("UpdateTraineePoints")]
        public async Task<ActionResult<int>> UpdateTraineePoints(UpdateTraineePointsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
