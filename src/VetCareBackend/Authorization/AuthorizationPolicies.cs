namespace VetCareBackend.Presentation.Authorization
{
    public enum AuthorizationPolicies
    {
        SoloClient,
        SoloVeterinarian,
        Administrator,
        VetAdm,
        ClientAdm,
    }

    public static class Policies
    {
        public const string soloClient = nameof(AuthorizationPolicies.SoloClient);
        public const string soloVeterinarian = nameof(AuthorizationPolicies.SoloVeterinarian);
        public const string Administrator = nameof(AuthorizationPolicies.Administrator);
        public const string VetAdm = nameof(AuthorizationPolicies.VetAdm);
        public const string ClientAdm = nameof(AuthorizationPolicies.ClientAdm);
    }
}
