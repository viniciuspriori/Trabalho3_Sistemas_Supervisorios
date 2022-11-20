using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho3_Sistemas_Supervisorios.Config
{
    public class ConfigManager //gerenciador das configurações do sistema
    {
        string _modelPath = Path.Combine(Environment.CurrentDirectory, "configModel.json"); //caminho de salvamento do modelo

        private ConfigModel ConfigModel;

        public ConfigManager()
        {
            ConfigModel = new ConfigModel(); //instancia a classe de modelo
            ConfigModel = OpenConfigModel(); //abre o modelo se houver
        }

        public void UpdateTotalizers(int numOpaca, int numTransp) //atualiza os totalizadores
        {
            ConfigModel.TotalizadorOpaca = numOpaca;
            ConfigModel.TotalizadorTransp = numTransp;
        }

        public string GetTagAddressByIndex(int index) =>
             $"{ConfigModel.DeviceName}.{ConfigModel.Tags[index]}";

        public string GetTagName(int index) => //retorna o nome da tag de acordo com o índice
            ConfigModel.Tags[index];

        public int GetTagsCount() => ConfigModel.Tags.Count; //retorna o contador de tags

        public string GetServerUrl() //retorna a URL do server a ser conectado
        {
           return $"{ConfigModel.ServerName}/{ConfigModel.DeviceName}"; 
        }

        public ConfigModel GetConfigModel() => ConfigModel;

        public Dictionary<int, string> GetTags() => ConfigModel.Tags;

        public string GetDeviceName() => ConfigModel.DeviceName;

        public ConfigModel OpenConfigModel() //pega do arquivo salvo se houver e tiver algo escrito, se não houver cria outro com configurações padrão
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

        public async Task SaveConfigModel() //salva o modelo
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
