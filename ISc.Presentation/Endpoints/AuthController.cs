﻿using ISc.Application.Features.Authentication.CheckResetOtp;
using ISc.Application.Features.Authentication.ForgetPassword;
using ISc.Application.Features.Authentication.Login;
using ISc.Application.Features.Authentication.MobileLogin;
using ISc.Application.Features.Authentication.ResetPassword;
using ISc.Application.Features.Authentication.SendConfirmEmailOtp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ISc.Presentation.Endpoints
{
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginQueryResponse>> Login([FromBody] LoginQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("mobile-login")]
        public async Task<ActionResult<MobileLoginQueryDto>> MobileLogin([FromBody] MobileLoginQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("forget-password")]
        public async Task<ActionResult<string>> ForgetPassword(string email)
        {
            return Ok(await _mediator.Send(new ForgetPasswordCommand(email)));
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("sendConfirmOtp")]
        public async Task<ActionResult<int>> SendEmailConfirmation([FromBody] string email)
        {
            return Ok(await _mediator.Send(new SendConfirmEmailOtpCommand(email)));
        }

        [HttpGet("checkResetOtp")]
        public async Task<ActionResult<int>> CheckResetOtp([FromQuery]CheckResetOtpQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
