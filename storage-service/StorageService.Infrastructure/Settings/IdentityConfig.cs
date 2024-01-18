namespace StorageService.Infrastructure.Settings
{
    public class IdentityConfig
    {
        public string AuthenticationScheme { get; set; }
        public string Authority { get; set; }
        public bool ValidateAudience { get; set; }
        public string Audience { get; set; }
    }
}
