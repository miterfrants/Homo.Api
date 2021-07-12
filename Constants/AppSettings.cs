namespace Homo.Api
{
    public interface IAppSettings
    {
        Common Common { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public Common Common { get; set; }
    }

    public class Common
    {
        public string LocalizationResourcesPath { get; set; }
    }
}
