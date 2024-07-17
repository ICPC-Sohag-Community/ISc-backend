using ISc.Application.Features.Mobile.Commands.AddAttendnce;
using ISc.Application.Features.Mobile.Commands.UpdateTraineePoints;
using ISc.Application.Features.Mobile.Queries.GetCamps;
using ISc.Application.Features.Mobile.Queries.GetPresistanceTrainees;
using ISc.Application.Features.Mobile.Queries.GetTraineesByCampId;
using ISc.Presentation.Middlerware;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [MobileAuthorizeFilter]
    public class MobileController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MobileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getTraineesByCampId/{campId}")]
        public async Task<ActionResult<GetTraineesByCampIdQueryDto>> GetTraineesByCampId(int campId)
        {
            return Ok(await _mediator.Send(new GetTraineesByCampIdQuery(campId)));
        }

        [HttpGet("getCamps")]
        public async Task<ActionResult<GetCampsQueryDto>> GetCamps()
        {
            return Ok(await _mediator.Send(new GetCampsQuery()));
        }

        [HttpPost("addTraineeToAttendence")]
        public async Task<ActionResult<string>> AddTraineeToAttendence([FromBody] AddTraineeToAttendenceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("getPresentTrainees")]
        public async Task<ActionResult<List<GetPresentTraineesQueryDto>>> GetPresentTrainee([FromQuery] GetPresentTraineesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPut("updateTraineePoints")]
        public async Task<ActionResult<int>> UpdateTraineePoints(UpdateTraineePointsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
