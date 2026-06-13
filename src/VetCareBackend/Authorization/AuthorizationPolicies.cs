namespace VetCareBackend.Presentation.Authorization
{
    public enum AuthorizationPolicies
    {
        SoloClient,
        SoloVeterinarian,
        SoloAdministrator,
        SoloSysadmin,
        VetAdm,
        ClientAdm,
        Admins
    }

    public static class Policies
    {
        public const string SoloClient = nameof(AuthorizationPolicies.SoloClient);
        public const string SoloVeterinarian = nameof(AuthorizationPolicies.SoloVeterinarian);
        public const string SoloAdministrator = nameof(AuthorizationPolicies.SoloAdministrator);
        public const string SoloSysadmin = nameof(AuthorizationPolicies.SoloSysadmin);
        public const string VetAdm = nameof(AuthorizationPolicies.VetAdm);
        public const string ClientAdm = nameof(AuthorizationPolicies.ClientAdm);
        public const string Admins = nameof(AuthorizationPolicies.Admins);
    }
}
