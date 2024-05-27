﻿using ISc.Application.Features.SystemRoles.Commands.Assign;
using ISc.Application.Features.SystemRoles.Commands.Create;
using ISc.Application.Features.SystemRoles.Commands.Delete;
using ISc.Application.Features.SystemRoles.Queries.GetAvailableRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    [Authorize]
    public class RolesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] string name)
        {
            return Ok(await _mediator.Send(new CreateRoleCommand(name)));
        }

        [HttpDelete]
        public async Task<ActionResult<string>> Delete([FromBody] string name)
        {
            return Ok(await _mediator.Send(new DeleteRoleCommand(name)));
        }

        [HttpGet("availableRoles/{userId}")]
        public async Task<ActionResult<List<GetAvailableRolesQueryDto>>> AvailableRoles(string userId)
        {
            return Ok(await _mediator.Send(new GetAvailableRolesQuery(userId));
        }

        [HttpPost("assignToRole")]
        public async Task<ActionResult<string>> AssignToRole([FromBody]AssignToRoleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
