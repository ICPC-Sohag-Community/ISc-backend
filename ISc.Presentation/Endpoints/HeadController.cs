using ISc.Application.Features.HeadOfCamps.Assigning.Commands.AssignTrainees;
using ISc.Application.Features.HeadOfCamps.Assigning.Commands.UnAssignTrainees;
using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetMentorAssign;
using ISc.Application.Features.HeadOfCamps.Assigning.Queries.GetTraineeAssignWithPagination;
using ISc.Application.Features.HeadOfCamps.Attendance.Queries.GetAllAttendance;
using ISc.Application.Features.HeadOfCamps.Materials.Commands.Create;
using ISc.Application.Features.HeadOfCamps.Materials.Commands.Delete;
using ISc.Application.Features.HeadOfCamps.Materials.Commands.UpdateMaterialOrder;
using ISc.Application.Features.HeadOfCamps.Materials.Queries.GetAllMaterials;
using ISc.Application.Features.HeadOfCamps.Sheets.Commands.Create;
using ISc.Application.Features.HeadOfCamps.Sheets.Commands.Delete;
using ISc.Application.Features.HeadOfCamps.Sheets.Commands.Update;
using ISc.Application.Features.HeadOfCamps.Sheets.Queries.GetById;
using ISc.Application.Features.HeadOfCamps.WeeklyFilter.Commands.FilterTraineeById;
using ISc.Application.Features.HeadOfCamps.WeeklyFilter.Queries.GetOtherTrainees;
using ISc.Application.Features.HeadOfCamps.WeeklyFilter.Queries.GetToFilter;
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

        [HttpGet("weeklyFilter/getToFilter")]
        public async Task<ActionResult<List<GetToFilterQueryDto>>> GetToFilterTrainees()
        {
            return Ok (await _mediator.Send(new GetToFilterQuery()));
        }

        [HttpDelete("weeklyFilter/filterTrainee")]
        public async Task<ActionResult<string>>FilterTraineeById([FromBody]string id)
        {
            return Ok(await _mediator.Send(new FilterTraineeByIdCommand(id)));
        }

        [HttpGet("getMaterailsBySheetId/{id}")]
        public async Task<ActionResult<List<GetAllMaterialsQueryDto>>>GetAllMaterialsBySheetId(int id)
        {
            return Ok(await _mediator.Send(new GetAllMaterialsQuery(id)));
        }

        [HttpGet("weeklyFilter/getOthers")]
        public async Task<ActionResult<List<GetOtherTraineesQueryDto>>>GetOtherTrainees([FromQuery]List<string> traineesIds)
        {
            return Ok(await _mediator.Send(new GetOtherTraineesQuery(traineesIds)));
        }

        [HttpPost("materials")]
        public async Task<ActionResult<int>>CreateMaterial(CreateMaterialCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("materials/{id}")]
        public async Task<ActionResult<string>>DeleteMaterial(int id)
        {
            return Ok(await _mediator.Send(new DeleteMaterialByIdCommand(id)));
        }

        [HttpPost("sheets")]
        public async Task<ActionResult<int>>CreateSheet(CreateSheetCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("materials/updateOrders")]
        public async Task<ActionResult<int>>UpdateMaterialOrders(UpdateMaterialOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("sheets/{id}")]
        public async Task<ActionResult<GetSheetByIdQueryDto>>GetSheetById(int id)
        {
            return Ok(await _mediator.Send(new GetSheetByIdQuery(id)));
        }

        [HttpPut("sheets")]
        public async Task<ActionResult<int>>UpdateSheet(UpdateSheetCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("sheet/{id}")]
        public async Task<ActionResult<string>>DeleteSheet(int id)
        {
            return Ok(await _mediator.Send(new DeleteSheetByIdCommand(id)));
        }
    }
}
