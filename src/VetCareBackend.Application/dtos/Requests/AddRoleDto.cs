using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Requests
{
    public class AddRoleRequest
    {
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string enrollment { get; set; } = string.Empty;
    }
}
