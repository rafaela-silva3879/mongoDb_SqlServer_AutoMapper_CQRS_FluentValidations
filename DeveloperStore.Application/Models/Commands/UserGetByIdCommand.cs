using DeveloperStore.Application.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Commands
{
    public class UserGetByIdCommand : IRequest<UserQuery>
    {
        public string Id { get; set; }
    }
}
