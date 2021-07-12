using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using System.Threading;

namespace Homo.Api
{
    public interface ILocalizer
    {
        Dictionary<string, Dictionary<string, string>> Mapping { get; set; }
        string Get(string key, CultureInfo cultureInfo);
    }

    public class ErrorMessageLocalizer : ILocalizer
    {
        public Dictionary<string, Dictionary<string, string>> Mapping { get; set; }
        private string _sourcePath;
        public ErrorMessageLocalizer(string sourcePath)
        {
            _sourcePath = sourcePath;
            Mapping = new Dictionary<string, Dictionary<string, string>>();
        }

        public string Get(string key, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentCulture;
            }
            string cultureName = cultureInfo.Name;
            if (Mapping.ContainsKey(cultureName) && Mapping[cultureName].ContainsKey(key))
            {
                return Mapping[cultureName][key];
            }

            if (!System.IO.File.Exists($"{_sourcePath}/Error/{cultureName}.json"))
            {
                return key;
            }
            Mapping[cultureName] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"{_sourcePath}/Error/{cultureName}.json"));
            return Mapping.ContainsKey(cultureName) && Mapping[cultureName].ContainsKey(key) ? Mapping[cultureName][key] : key;
        }
    }

    public class CommonLocalizer : ILocalizer
    {
        public Dictionary<string, Dictionary<string, string>> Mapping { get; set; }
        private string _sourcePath;
        public CommonLocalizer(string sourcePath)
        {
            _sourcePath = sourcePath;
            Mapping = new Dictionary<string, Dictionary<string, string>>();
        }

        public string Get(string key, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentCulture;
            }
            string cultureName = cultureInfo.Name;
            if (Mapping.ContainsKey(cultureName) && Mapping[cultureName].ContainsKey(key))
            {
                return Mapping[cultureName][key];
            }
            if (!System.IO.File.Exists($"{_sourcePath}/Common/{cultureName}.json"))
            {
                return key;
            }
            Mapping[cultureName] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"{_sourcePath}/Common/{cultureName}.json"));
            return Mapping.ContainsKey(cultureName) && Mapping[cultureName].ContainsKey(key) ? Mapping[cultureName][key] : key;
        }
    }

    public class ValidationLocalizer : ILocalizer
    {
        public Dictionary<string, Dictionary<string, string>> Mapping { get; set; }
        private string _sourcePath;
        public ValidationLocalizer(string sourcePath)
        {
            _sourcePath = sourcePath;
            Mapping = new Dictionary<string, Dictionary<string, string>>();
        }

        public string Get(string key, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = Thread.CurrentThread.CurrentCulture;
            }
            string cultureName = cultureInfo.Name;
            if (Mapping.ContainsKey(cultureName) && Mapping[cultureName].ContainsKey(key))
            {
                return Mapping[cultureName][key];
            }
            if (!System.IO.File.Exists($"{_sourcePath}/Validation/{cultureName}.json"))
            {
                return key;
            }
            Mapping[cultureName] = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"{_sourcePath}/Validation/{cultureName}.json"));
            return Mapping.ContainsKey(cultureName) && Mapping[cultureName].ContainsKey(key) ? Mapping[cultureName][key] : key;
        }
    }
}