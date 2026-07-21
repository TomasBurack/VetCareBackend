using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Responses
{
    public class TwoFactorRecoveryCodesResponse
    {
        public List<string> RecoveryCodes { get; set; } = new();
    }
}
