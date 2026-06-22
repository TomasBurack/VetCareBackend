using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Application.Interfaces
{
    public interface IDogApiService
    {
        Task<IReadOnlyList<string>> GetAllBreedsAsync();
    }
}
