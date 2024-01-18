namespace IdentityService.Infrastructure.Settings;

public class MongoDbSetting
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
