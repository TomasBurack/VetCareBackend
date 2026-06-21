using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using VetCareBackend.Application.Interfaces;
using VetCareBackend.Infrastructure.ExternalDTO;

namespace VetCareBackend.Infrastructure.ExternalService
{
    public class CatApiService : ICatApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClient CatClient {  get; set; }

        public CatApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            CatClient = _httpClientFactory.CreateClient("catHttpClient");
        }

        public async Task<IReadOnlyList<string>> GetAllBreedsAsync()
        {
            var breeds = await CatClient.GetFromJsonAsync<List<ExternalBreedDTO>>("v1/breeds") 
                ?? new List<ExternalBreedDTO>();

            return breeds.Select(b => b.Name).ToList();

        }
    }
}
