using ISc.Application.Features.Leader.Camps.Queries.GetAllMentor;
using ISc.Application.Features.Leader.Camps.Queries.GetHeadsOfCamp;
using ISc.Application.Features.Leader.Dashboard.Queries.GetCampsAnalysis;
using ISc.Application.Features.Leader.Dashboard.Queries.GetFeedbacks;
using ISc.Application.Features.Leader.Dashboard.Queries.GetTraineesAnalysis;
using ISc.Application.Features.Leader.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
        [HttpGet("dashboard/traineesAnalysis")]
        public async Task<ActionResult<List<GetTraineesAnalysisQueryDto>>> GetTraineesAnalysis()
        {
            return Ok(await _mediator.Send(new GetTraineesAnalysisQuery()));
        }

         [HttpGet("dashboard/feedbacks")]
        public async Task<ActionResult<List<GetFeedbacksQueryDto>>> GetFeedbacks()
        {
            return Ok(await _mediator.Send(new GetCampsAnalysisQuery()));
        }

        [HttpGet("dashboard/camps")]
        public async Task<ActionResult<List<GetCampsAnalysisQueryDto>>> GetCampsAnalysis()
        {
            return Ok(await _mediator.Send(new GetCampsAnalysisQuery())); 
        }

		[HttpGet("Camps/mentors")]
		public async Task<ActionResult<List<GetAllMentorQueryDto>>> GetAllMentor()
		{
			return Ok(await _mediator.Send(new GetAllMentorQuery()));
		}

		[HttpGet("Camps/headsOfCamp")]
		public async Task<ActionResult<List<GetHeadsOfCampQueryDto>>> GetHeadsOfCamp()
		{
			return Ok(await _mediator.Send(new GetHeadsOfCampQuery()));
		}

		[HttpPost("createAccount")]
        public async Task<ActionResult<string>>AddUser([FromForm]CreateAccountCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
