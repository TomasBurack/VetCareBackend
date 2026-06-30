using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface IMailService 
    {
        Task SendEmail(string ToEmail, string ToName, string subjet, string body);
    }
}
