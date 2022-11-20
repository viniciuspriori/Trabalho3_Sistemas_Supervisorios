using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho3_Sistemas_Supervisorios
{
    public class ConfigManager
    {
        string _modelPath = Path.Combine(Environment.CurrentDirectory, "configModel.json");

        private ConfigModel ConfigModel;

        public ConfigManager()
        {
            ConfigModel = new ConfigModel();
            ConfigModel.Default();
            ConfigModel = OpenConfigModel();
        }

        public string GetTagAddressByIndex(int index) =>
             $"{ConfigModel.DeviceName}.{ConfigModel.Tags[index]}";

        public string GetTagName(int index) =>
            ConfigModel.Tags[index];

        public ConfigModel OpenConfigModel()
        {
            if (File.Exists(_modelPath) && new FileInfo(_modelPath).Length > 0)
            {
                using (StreamReader file = File.OpenText(_modelPath))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject config = (JObject)JToken.ReadFrom(reader);
                    return JsonConvert.DeserializeObject<ConfigModel>(config.ToString());
                };
            }
            else
            {
                var newConfig = new ConfigModel();
                newConfig.Default();
                return newConfig;
            }
        }

        public async Task SaveConfigModel()
        {
            using (StreamWriter sw = new StreamWriter(_modelPath))
            {
                var str = JsonConvert.SerializeObject(ConfigModel, Formatting.Indented);
                sw.Write(str);
            }

            await Task.Delay(500);
        }

    }
}
