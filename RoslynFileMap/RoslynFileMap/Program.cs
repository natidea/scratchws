using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RoslynFileMap
{
    // http://www.newtonsoft.com/json/help/html/DeserializeDictionary.htm
    // http://www.newtonsoft.com/json/help/html/SerializeDictionary.htm
    class Program
    {
        static int option = 0;

        static List<string> keys = new List<string> {
            "dotnet/coby",
            "dotnet/roslyn"
        };

        static void Main(string[] args)
        {
            string key = keys[0];

            if (option == 0)
            {
                CreateSourceMap(
                    args.Any() ? args.First() : 
                        ConfigurationManager.AppSettings[key + "/rootDir"],
                    ConfigurationManager.AppSettings[key + "/output"]);
            }
            
            if (option == 1)
            {
                DemoReadMap(key);
            }

            Console.ReadKey();
        }

        public static void CreateSourceMap(string directory, string output)
        {
            // get map
            var fileMap = GetFileMap(directory);

            // serialize to string
            var json = JsonConvert.SerializeObject(fileMap, Formatting.Indented);

            // write to file
            File.WriteAllText(output, json);

            Console.WriteLine($"Wrote {fileMap.Count} entries");
        }

        private static Dictionary<string, string> GetFileMap(string directory)
        {
            var len = directory.Length;

            return new DirectoryInfo(directory)
                .GetFiles("*.cs", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(len + 1).Replace("\\", "/"))
                .ToDictionary(p => p.ToLowerInvariant());
        }

        public static void DemoReadMap(string repository)
        {
            string key = repository + "/sourceMap";
            string pathToMap = ConfigurationManager.AppSettings[key];

            // Make a request to get raw map synchronously
            HttpClient client = new HttpClient();
            var response = client.GetAsync(pathToMap).Result;
            string rawMap = response.Content.ReadAsStringAsync().Result;

            var demoMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawMap);
            Console.WriteLine(demoMap.First());
        }


    }
}
