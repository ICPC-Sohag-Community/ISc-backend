using ISc.Application.Dtos.Standing;
using ISc.Application.Features.Mentors.Attendance.Queries.GetAttendance;
using ISc.Application.Features.Mentors.Camps.Queries.GetCamps;
using ISc.Application.Features.Mentors.Contests.Queries.GetTraineesContests;
using ISc.Application.Features.Mentors.Practices.Commands.Create;
using ISc.Application.Features.Mentors.Practices.Commands.Delete;
using ISc.Application.Features.Mentors.Practices.Commands.Update;
using ISc.Application.Features.Mentors.Practices.Queries.GetPractice;
using ISc.Application.Features.Mentors.Sheets.Queries.TraineesSheetsQuery;
using ISc.Application.Features.Mentors.Standing.Queries.GetStanding;
using ISc.Application.Features.Mentors.Tasks.Commands.Create;
using ISc.Application.Features.Mentors.Tasks.Commands.Delete;
using ISc.Application.Features.Mentors.Tasks.Commands.Update;
using ISc.Application.Features.Mentors.Tasks.Queries.GetTasksByStatus;
using ISc.Application.Features.Mentors.TraineesInfo.Queries.GetTraineeInDetail;
using ISc.Application.Features.Mentors.TraineesInfo.Queries.GetTraineesInfo;
using ISc.Domain.Comman.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize(Roles = Roles.Mentor)]
    public class MentorController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MentorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("camps")]
        public async Task<ActionResult<GetMentorCampsQueryDto>> GetCamps()
        {
            return Ok(await _mediator.Send(new GetMentorCampsQuery()));
        }

        [HttpGet("trainees")]
        public async Task<ActionResult<List<GetTraineesInfoQueryDto>>> GetTrainees(GetTraineesInfoQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("trainees/{id}")]
        public async Task<ActionResult<GetTraineeInDetailQueryDto>> GetTraineeInDetail(string id)
        {
            return Ok(await _mediator.Send(new GetTraineeInDetailQuery(id)));
        }

        [HttpGet("attendances")]
        public async Task<ActionResult<GetAttendanceQueryDto>> GetAttendance(int campId)
        {
            return Ok(await _mediator.Send(new GetAttendanceQuery(campId)));
        }

        [HttpGet("sheetsTracking/{campId}")]
        public async Task<ActionResult<GetTraineesSheetsQueryDto>> GetTraineesSheetsTracking(int campId)
        {
            return Ok(await _mediator.Send(new GetTraineesSheetsQuery(campId)));
        }

        [HttpGet("standing/{campId}")]
        public async Task<ActionResult<StandingDto>> GetStanding(int campId)
        {
            return Ok(await _mediator.Send(new GetStandingQuery(campId)));
        }

        [HttpGet("contestsTracking/{campId}")]
        public async Task<ActionResult<GetTraineesContestsQueryDto>> GetTraineesContestsTracking(int campId)
        {
            return Ok(await _mediator.Send(new GetTraineesContestsQuery(campId)));
        }

        [HttpGet("tasksOnStatus/{campId}")]
        public async Task<ActionResult<GetTasksByStatusQueryDto>> GetTasks(int campId)
        {
            return Ok(await _mediator.Send(new GetTasksByStatusQuery(campId)));
        }

        [HttpPost("tasks")]
        public async Task<ActionResult<string>> CreateTask(CreateTaskCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("tasks")]
        public async Task<ActionResult<string>> DeleteTask(DeleteTaskCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("tasks")]
        public async Task<ActionResult<int>> UpdateTask(UpdateTaskCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("practices/{campId}")]
        public async Task<ActionResult<GetPracticeQueryDto>> GetPractices(int campId)
        {
            return Ok(await _mediator.Send(new GetPracticeQuery(campId)));
        }

        [HttpPost("practices")]
        public async Task<ActionResult<int>> CreatePractice(CreatePracticeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("practices")]
        public async Task<ActionResult<string>> DeletePractice(DeletePracticeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("practices")]
        public async Task<ActionResult<int>> UpdatePractice(UpdatePracticeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}

