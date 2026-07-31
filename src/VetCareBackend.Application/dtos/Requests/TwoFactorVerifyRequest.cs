using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Requests
{
    public class TwoFactorVerifyRequest
    {
        public string PendingToken { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
