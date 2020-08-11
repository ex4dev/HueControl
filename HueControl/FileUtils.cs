using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HueControlSettings
{
    public static class FileUtils
    {
        public static async Task SaveToAppdataAsync(string filename, string contents)
        {
            string path = Path.Combine(GetAppdataPath(), filename);

            await File.WriteAllTextAsync(path, contents);
        }

        public static async Task<bool> SaveToAppdataAsyncJson(string filename, object contents)
        {
            string path = Path.Combine(GetAppdataPath(), filename);

            await File.WriteAllTextAsync(path, JsonConvert.SerializeObject(contents));

            return true;
        }
        
        public static async Task<object> GetFromAppdataAsyncJson(string filename, Type type)
        {
            string path = Path.Combine(GetAppdataPath(), filename);

            string text = await File.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject(text, type);
        }
        
        public static async Task<object> GetFromAppdataAsyncJson(string filename)
        {
            string path = Path.Combine(GetAppdataPath(), filename);

            string text = await File.ReadAllTextAsync(path);

            return JsonConvert.DeserializeObject(text);
        }

        public static void DeleteFromAppdata(string filename)
        {
            string path = Path.Combine(GetAppdataPath(), filename);

            File.Delete(path);
        }

        public static string GetAppdataPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "HueControl");

            Directory.CreateDirectory(path);

            return path;
        }
    }

    public class HueControlConfig
    {
        public string DefaultBridge;
        public bool Debug_PrintInfo;
        public bool Debug_PrintErrors;
        public bool Events_EnableService;
        
        public HueControlConfig()
        {
            DefaultBridge = "<NONE>";
            Debug_PrintInfo = false;
            Debug_PrintErrors = true;
            Events_EnableService = false;
        }

       
        public async Task<HueControlConfig> SaveConfigAsync()
        {
            await FileUtils.SaveToAppdataAsyncJson("config.json", this);
            return this;
        }

        public static async Task<HueControlConfig> ReadConfigAsync()
        {
            return (HueControlConfig) await FileUtils.GetFromAppdataAsyncJson("config.json", typeof(HueControlConfig));
        }
        public static HueControlConfig ReadConfig()
        {
            try
            {
                return (HueControlConfig) FileUtils.GetFromAppdataAsyncJson("config.json", typeof(HueControlConfig))
                    .Result;
            }
            catch (Exception)
            {
                return new HueControlConfig().SaveConfigAsync().Result;
            }
        }
        
    }
}