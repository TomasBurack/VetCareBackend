using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Requests
{
    public class TwoFactorConfirmRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
