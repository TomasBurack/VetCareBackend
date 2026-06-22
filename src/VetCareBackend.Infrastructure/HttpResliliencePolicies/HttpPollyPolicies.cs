using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Infrastructure.HttpResliliencePolicies
{
    public static class HttpPollyPolicies
    {
        public static class PollyResiliencePolicies
        {
            public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ApiClientConfigurationDTO config)
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(
                    config.RetryCount, 
                    retryAttempt => TimeSpan.FromSeconds(config.RetryAttemptInSeconds)
                    );
            }
            
            public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ApiClientConfigurationDTO config) 
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .CircuitBreakerAsync(
                    config.HandledEventsAllowedBeforeBreaking,
                    TimeSpan.FromSeconds(config.DurationOfBreakInSeconds)
                    );
            }


        }
    }
}
