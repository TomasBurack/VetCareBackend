using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Requests
{
    public class TwoFactorDisableRequest
    {
        public string Password { get; set; } = string.Empty;
    }
}
