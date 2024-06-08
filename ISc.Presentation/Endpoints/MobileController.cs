using ISc.Application.Features.Mobile.Commands;
using ISc.Application.Features.Mobile.Queries;
using ISc.Application.Features.Mobile.Query;
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
        public async Task<ActionResult<GetTraineesByCampIdQueryDto>> GetTraineesByCampId( GetTraineesByCampIdQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("getCamps")]
        public async Task<ActionResult<GetCampsQueryDto>> ForgetPassword(GetCampsQueryDto query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("addTraineeToAttendence")]
        public async Task<ActionResult<string>> ResetPassword(AddTraineeToAttendenceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}
