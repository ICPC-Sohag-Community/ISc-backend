﻿using ISc.Application.Features.Camps.Queries.GetCampsOpenRegister;
using ISc.Application.Features.Leader.Accounts.Commands.Create;
using ISc.Application.Features.Leader.Archives.Commands.DeleteTraineeById;
using ISc.Application.Features.Leader.Archives.Queries.GetAllStaffsArchiveWithPagination;
using ISc.Application.Features.Leader.Archives.Queries.GetAllTraineesArchiveWithPagination;
using ISc.Application.Features.Leader.Archives.Queries.GetStaffArchiveById;
using ISc.Application.Features.Leader.Archives.Queries.GetTraineeArchiveById;
using ISc.Application.Features.Leader.Camps.Commands.Create;
using ISc.Application.Features.Leader.Camps.Commands.Delete;
using ISc.Application.Features.Leader.Camps.Commands.Empty;
using ISc.Application.Features.Leader.Camps.Commands.Update;
using ISc.Application.Features.Leader.Camps.Queries.GetAllCampsWithPagination;
using ISc.Application.Features.Leader.Camps.Queries.GetAllHeadsOfCamp;
using ISc.Application.Features.Leader.Camps.Queries.GetAllMentor;
using ISc.Application.Features.Leader.Camps.Queries.GetCampEditById;
using ISc.Application.Features.Leader.Dashboard.Queries.GetCampsAnalysis;
using ISc.Application.Features.Leader.Dashboard.Queries.GetFeedbacks;
using ISc.Application.Features.Leader.Dashboard.Queries.GetTraineesAnalysis;
using ISc.Application.Features.Leader.Reports.Queries.CampReoprts;
using ISc.Application.Features.Leader.Request.Commands.DeleteRequestsByIds;
using ISc.Application.Features.Leader.Request.Commands.SubmitRequests;
using ISc.Application.Features.Leader.Request.Queries.DisplayAll;
using ISc.Application.Features.Leader.Staff.Queries.GetAllWithPagination;
using ISc.Application.Features.Leader.Staff.Queries.GetById;
using ISc.Application.Features.Leader.Standing.Queries.GetStandingByCampId;
using ISc.Application.Features.Leader.Trainees.Queries.GetAllWithPagination;
using ISc.Application.Features.Leader.Trainees.Queries.GetById;
using ISc.Domain.Comman.Constant;
using ISc.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize(Roles = Roles.Leader)]
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
            return Ok(await _mediator.Send(new GetFeedbacksQuery()));
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
        public async Task<ActionResult<int>> CreateCamp([FromBody] CreateCampCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("camps/{id}")]
        public async Task<ActionResult<int>> UpdateCamp(int id, [FromBody] UpdateCampCommand command)
        {
            if (id != command.id)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("camps/{id}")]
        public async Task<ActionResult<string>> DeleteCamp(int id)
        {
            return Ok(await _mediator.Send(new DeleteCampCommand(id)));
        }

        [HttpDelete("camps/Emtpy/{id}")]
        public async Task<ActionResult<string>> EmptyCamp(int id)
        {
            return Ok(await _mediator.Send(new EmptyCampCommand(id)));
        }

        [HttpGet("camps/displayEdit/{id}")]
        public async Task<ActionResult<GetCampEditByIdQueryDto>> DisplayCampToEdit(int id)
        {
            return Ok(await _mediator.Send(new GetCampEditByIdQuery(id)));
        }
        [HttpGet("trainees/{id}")]
        public async Task<ActionResult<GetTraineeByIdQueryDto>> GetTraineeById(string id)
        {
            return Ok(await _mediator.Send(new GetTraineeByIdQuery(id)));
        }
        [HttpGet("trainees")]
        public async Task<ActionResult<GetAllTraineeWithPaginationQueryDto>> GetAllTraineesWithPagination([FromQuery] GetAllTraineeWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("traineesRegisterations")]
        public async Task<ActionResult<GetAllRegisterationQueryDto>> GetAllTraineesRegisterRequests(GetAllRegisterationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        //[HttpGet("traineesRegisterationSystemFilter")]
        //public async Task<ActionResult<GetRegisterationOnSystemFilterQueryDto>> GetTraineeRegistersSystemFilter([FromQuery] GetRegisterationOnSystemFilterQuery query)
        //{
        //    return Ok(await _mediator.Send(query));
        //}

        //[HttpGet("traineesRegisterationCustomFilter")]
        //public async Task<ActionResult<GetOnCustomerFilterQueryDto>> GetTraineeRequestsCustomFilter([FromQuery] GetOnCustomerFilterQuery query)
        //{
        //    return Ok(await _mediator.Send(query));
        //}

        [HttpGet("traineeArchive/{id}")]
        public async Task<ActionResult<GetTraineeArchiveByIdQueryDto>> GetTraineeArchiveById(int id)
        {
            return Ok(await _mediator.Send(new GetTraineeArchiveByIdQuery(id)));
        }

        [HttpGet("traineesArchiveWithPagination")]
        public async Task<ActionResult<GetAllTraineesArchiveWithPaginationQueryDto>> GetTraineesArchive([FromQuery] GetAllTraineesArchiveWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("camps/reports")]
        public async Task<ActionResult<List<GetCampReoprtsQueryDto>>> CampsReoprts()
        {
            return Ok(await _mediator.Send(new GetCampReoprtsQuery()));
        }

        [HttpPost("Traineeregisteration/submit")]
        public async Task<ActionResult<string>> SubmitRegisterationRequests(SubmitRequestsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("traineesArchive/{id}")]
        public async Task<ActionResult<string>> DeleteTraineeArchiveById(int id)
        {
            return Ok(await _mediator.Send(new DeleteTraineeArchiveByIdCommand(id)));
        }

        [HttpDelete("traineeRegisteration")]
        public async Task<ActionResult<string>> DeleteRequests(List<int> requests)
        {
            return Ok(await _mediator.Send(new DeleteRequestsByIdsCommand(requests)));
        }

        [HttpGet("staff/{id}")]
        public async Task<ActionResult<GetStaffByIdQueryDto>> GetStaffById(string id)
        {
            return Ok(await _mediator.Send(new GetStaffByIdQuery(id)));
        }

        [HttpGet("staffArchive/{id}")]
        public async Task<ActionResult<GetStaffArchiveByIdQueryDto>> GetStaffArchiveById(int id)
        {
            return Ok(await _mediator.Send(new GetStaffArchiveByIdQuery(id)));
        }

        [HttpGet("staffArchiveWithPagination")]
        public async Task<ActionResult<GetAllStaffsArchiveWithPaginationQueryDto>> GetStaffsArchive([FromQuery] GetAllStaffsArchiveWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("staffWithPagination")]
        public async Task<ActionResult<PaginatedRespnose<GetAllStaffWithPaginationQueryDto>>> GetAllStaff([FromQuery] GetAllStaffWithPaginationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("standing")]
        public async Task<ActionResult<GetStandingByCampIdQueryDto>> GetStanding(int campId)
        {
            return Ok(await _mediator.Send(new GetStandingByCampIdQuery(campId)));
        }

        [HttpGet("openCampsRegister")]
        public async Task<ActionResult<GetCampsOpenRegisterQueryDto>> GetOpenCampsRegisteration()
        {
            return Ok(await _mediator.Send(new GetCampsOpenRegisterQuery()));
        }
    }
}
