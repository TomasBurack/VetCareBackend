using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Application.dtos.Requests
{
    public class UserResponse
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
