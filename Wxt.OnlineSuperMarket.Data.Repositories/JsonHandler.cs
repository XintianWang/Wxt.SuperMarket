using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    public class JsonHandler
    {
        public T GetRecords<T>(string jsonFile)
        {
            string fileContent = "";
            try
            {
                fileContent = File.ReadAllText(jsonFile);
                return JsonConvert.DeserializeObject<T>(fileContent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Warning: {e.Message}.");
                return default(T);
            }
        }

        public void SaveRecords<T>(string jsonFile, T obj)
        {
            string jsonstr = JsonConvert.SerializeObject(obj);
            File.WriteAllText(jsonFile, jsonstr);
        }
    }
}
