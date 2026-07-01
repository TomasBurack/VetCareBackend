using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VetCareBackend.Domain.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool isUsed { get; set; } = false;


    }
}
