using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface IMailService
    {
        void SendEmail(string ToEmail, string ToName, string subjet, string body);
    }
}
