using System.Collections.Generic;
using System.Threading.Tasks;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Application.Interfaces
{
    public interface IBreedService
    {
        Task<IReadOnlyList<string>> GetBreedsByTypeAsync(TypePet typePet);
    }
}
