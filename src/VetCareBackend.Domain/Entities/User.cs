using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Domain.Entities
{
    public abstract class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Dni {  get; set; } = string.Empty ;

        public string Email { get; set; } = string.Empty;

        public string Password {  get; set; } = string.Empty;

        public Role Role { get; set; } = Role.Client;

    }
}
