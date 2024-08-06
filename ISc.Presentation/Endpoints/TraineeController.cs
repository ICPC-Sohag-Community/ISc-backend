using Azure;
using ISc.Application.Features.Trainees.Contests.Queries.GetAllContests;
using ISc.Application.Features.Trainees.Contests.Queries.GetInComingContest;
using ISc.Application.Features.Trainees.Feedbacks.Commands.Create;
using ISc.Application.Features.Trainees.Feedbacks.Queries.GetAddFeedbackAccessabilty;
using ISc.Application.Features.Trainees.Feedbacks.Queries.GetAddFeedbackAccessablilty;
using ISc.Application.Features.Trainees.Heads.Queries.GetHeads;
using ISc.Application.Features.Trainees.Mentors.Queries.GetMentor;
using ISc.Application.Features.Trainees.Sheets.Queries.GetAllSheets;
using ISc.Application.Features.Trainees.Sheets.Queries.GetCurrentSheet;
using ISc.Application.Features.Trainees.Tasks.Queries.GetTaskWithStatus;
using ISc.Domain.Comman.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize(Roles =Roles.Trainee)]
    public class TraineeController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TraineeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("sheets")]
        public async Task<ActionResult<List<GetAllSheetsQueriesDto>>> GetAllSheets()
        {
            return Ok(await _mediator.Send(new GetAllSheetsQueries()));
        }

        [HttpGet("currentSheet")]
        public async Task<ActionResult<GetCurrentSheetQueryDto>> GetCurrSheet()
        {
            return Ok(await _mediator.Send(new GetCurrentSheetQuery()));
        }

        [HttpGet("contests")]
        public async Task<ActionResult<List<GetAllContestsQueryDto>>> GetAllContests()
        {
            return Ok(await _mediator.Send(new GetAllContestsQuery()));
        }

        [HttpGet("inComingContest")]
        public async Task<ActionResult<GetInComingContestQueryDto>> GetInComingContest()
        {
            return Ok(await _mediator.Send(new GetInComingContestQuery()));
        }

        [HttpGet("mentorInfo")]
        public async Task<ActionResult<GetMentorQueryDto>> GetMentorInfo()
        {
            return Ok(await _mediator.Send(new GetMentorQuery()));
        }

        [HttpGet("tasks")]
        public async Task<ActionResult<GetTaskWithStatusQueryDto>> GetTaskStatus()
        {
            return Ok(await _mediator.Send(new GetTaskWithStatusQuery()));
        }

        [HttpGet("headInfo")]
        public async Task<ActionResult<GetHeadInfoQueryDto>> GetHeadInfo()
        {
            return Ok(await _mediator.Send(new GetHeadInfoQuery()));
        }

        [HttpPost("feedback")]
        public async Task<ActionResult<Response>> AddFeedback(CreateFeedbackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("canAddFeedback")]
        public async Task<ActionResult<GetAddFeedbackAccessabiltyQueryDto>> CanAddFeedback()
        {
            return Ok(await _mediator.Send(new GetAddFeedbackAccessabiltyQuery()));
        }
    }
}
