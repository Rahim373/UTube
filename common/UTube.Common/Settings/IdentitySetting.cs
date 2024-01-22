namespace UTube.Common.Settings;

public class IdentitySetting
{
    public string Authority { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public bool RequireHttpsMetadata { get; set; } = false;
    public bool ValidateIssuer { get; set; } = false;
}
