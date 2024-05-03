using ISc.Application.Features.Leader.Camps.Commands.Create;
using ISc.Application.Features.Leader.Camps.Queries.GetAllCampsWithPagination;
using ISc.Application.Features.Leader.Camps.Queries.GetAllHeadsOfCamp;
using ISc.Application.Features.Leader.Camps.Queries.GetAllMentor;
using ISc.Application.Features.Leader.Dashboard.Queries.GetCampsAnalysis;
using ISc.Application.Features.Leader.Dashboard.Queries.GetFeedbacks;
using ISc.Application.Features.Leader.Dashboard.Queries.GetTraineesAnalysis;
using ISc.Application.Features.Leader.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize]
    public class LeaderController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public LeaderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createAccount")]
        public async Task<ActionResult<string>> AddUser([FromForm] CreateAccountCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("dashboard/traineesAnalysis")]
        public async Task<ActionResult<List<GetTraineesAnalysisQueryDto>>> GetTraineesAnalysis()
        {
            return Ok(await _mediator.Send(new GetTraineesAnalysisQuery()));
        }

        [HttpGet("dashboard/feedbacks")]
        public async Task<ActionResult<List<GetFeedbacksQueryDto>>> GetFeedbacks()
        {
            return Ok(await _mediator.Send(new GetFeedbacksQueryDto()));
        }

        [HttpGet("dashboard/camps")]
        public async Task<ActionResult<List<GetCampsAnalysisQueryDto>>> GetCampsAnalysis()
        {
            return Ok(await _mediator.Send(new GetCampsAnalysisQuery()));
        }

        [HttpGet("camps/mentors")]
        public async Task<ActionResult<List<GetAllMentorsQueryDto>>> GetAllMentor()
        {
            return Ok(await _mediator.Send(new GetAllMentorsQuery()));
        }

        [HttpGet("camps/headsOfCamp")]
        public async Task<ActionResult<List<GetAllHeadsOfCampQueryDto>>> GetHeadsOfCamp()
        {
            return Ok(await _mediator.Send(new GetAllHeadsOfCampQuery()));
        }

        [HttpGet("camps/getAll")]
        public async Task<ActionResult<GetAllCampsWithPaginationQueryDto>> GetAll([FromQuery] GetAllCampsWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("camps")]
        public async Task<ActionResult<CreateCampCommand>> CreateCamp([FromBody] CreateCampCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
