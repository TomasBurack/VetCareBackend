using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Configuration
{
    public class MailOptions
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
