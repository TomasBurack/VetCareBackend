using System;
using System.Collections.Generic;
using System.Text;

namespace VetCareBackend.Infrastructure.HttpResliliencePolicies
{
    public class ApiClientConfigurationDTO
    {
        public int RetryCount;
        public int RetryAttemptInSeconds;
        public int HandledEventsAllowedBeforeBreaking;
        public int DurationOfBreakInSeconds;
    }
}
