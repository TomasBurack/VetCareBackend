using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Infrastructure.ExternalDTO;

namespace VetCareBackend.Infrastructure.ExternalService
{
    public class DogApiService : IDogApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClient DogClient { get; set; }

        public DogApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            DogClient = _httpClientFactory.CreateClient("dogHttpClient");
        }

        public async Task<IReadOnlyList<string>> GetAllBreedsAsync()
        {
            var breeds = await DogClient.GetFromJsonAsync<List<ExternalBreedDTO>>("v1/breeds")
                ?? new List<ExternalBreedDTO>();

            return breeds.Select(b => b.Name).ToList();
        }
    }
}
