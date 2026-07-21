using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Responses
{
    public class TwoFactorSetupResponse
    {
        public string OtpAuthUri { get; set; } = string.Empty;
    }
}
