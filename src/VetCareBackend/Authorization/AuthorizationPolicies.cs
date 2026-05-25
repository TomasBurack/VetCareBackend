namespace VetCareBackend.Presentation.Authorization
{
    public enum AuthorizationPolicies
    {
        SoloClient,
        SoloVeterinarian,
        SoloAdministrator
    }

    public static class Policies
    {
        public const string soloClient = nameof(AuthorizationPolicies.SoloClient);
        public const string soloVeterinarian = nameof(AuthorizationPolicies.SoloVeterinarian);
        public const string soloAdministrator = nameof(AuthorizationPolicies.SoloAdministrator);
    }
}
