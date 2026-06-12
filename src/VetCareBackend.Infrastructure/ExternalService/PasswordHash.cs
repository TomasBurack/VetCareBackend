using Azure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VetCareBackend.Application.Interfaces;

namespace VetCareBackend.Infrastructure.ExternalService
{
    public class PasswordHash : IPasswordHash
    {
        public string Hash(string password) 
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
    }
}
