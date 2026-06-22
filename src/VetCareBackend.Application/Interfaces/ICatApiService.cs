using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface ICatApiService
    {
        Task<IReadOnlyList<string>> GetAllBreedsAsync();
    }
}
