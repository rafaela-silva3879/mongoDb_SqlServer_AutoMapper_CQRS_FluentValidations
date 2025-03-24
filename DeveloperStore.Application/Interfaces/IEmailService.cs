using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Interfaces
{
    public interface IEmailService
    {
        void Send(string toEmail, string subject, string body);
    }
}
