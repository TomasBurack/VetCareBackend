using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface IPasswordHash
    {
        string Hash(string password);
    }
}
