using ISc.Application.Features.CampsModels.Commands.Create;
using ISc.Application.Features.CampsModels.Commands.Delete;
using ISc.Application.Features.CampsModels.Queries.GetAll;
using ISc.Domain.Comman.Constant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize(Roles =Roles.Leader)]
    public class CampModelController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CampModelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetAllCampsModelsQueryDto>> GetAll([FromQuery]GetAllCampsModelsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("{name}")]
        public async Task<ActionResult> Create(string name)
        {
            return Ok(await _mediator.Send(new CreateCampModelCommand(name)));
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            return Ok(await _mediator.Send(new DeleteCampModelCommand(name)));
        }
    }
}
