﻿using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Commands
{
    public class UserUpdatePasswordCommand : IRequest<UserQuery>
    {
        public string Id { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetSenhaExpiresIn { get; set; }
        public string NewPassword { get; set; }
    }
}
