using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.dtos.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        
        public string Email { get; set; } = string.Empty;
    }
}
