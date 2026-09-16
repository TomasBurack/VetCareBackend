using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Domain.Enums;

namespace VetCareBackend.Infrastructure.ExternalService
{
    public class BreedService : IBreedService
    {
        private const string MixedBreed = "Mixed Breed";

        private readonly ICatApiService _catApiService;
        private readonly IDogApiService _dogApiService;

        public BreedService(ICatApiService catApiService, IDogApiService dogApiService)
        {
            _catApiService = catApiService;
            _dogApiService = dogApiService;
        }

        public async Task<IReadOnlyList<string>> GetBreedsByTypeAsync(TypePet typePet)
        {
            switch (typePet)
            {
                case TypePet.Feline:
                    return (await _catApiService.GetAllBreedsAsync()).Prepend(MixedBreed).ToList();
                case TypePet.Canine:
                    return (await _dogApiService.GetAllBreedsAsync()).Prepend(MixedBreed).ToList();
                default:
                    return new List<string>();
            }
        }
    }
}
