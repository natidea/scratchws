using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynFileMap
{
    // http://www.newtonsoft.com/json/help/html/DeserializeDictionary.htm
    // http://www.newtonsoft.com/json/help/html/SerializeDictionary.htm
    class Program
    {
        static void Main(string[] args)
        {
            string directory = args.Any() ? args.First() : @"D:\Repos\github\roslyn\src";

            // get map
            var fileMap = GetFileMap(directory);

            // serialize to string
            var json = JsonConvert.SerializeObject(fileMap, Formatting.Indented);

            // write to file
            File.WriteAllText("../../output/roslyn.json", json);

            Console.WriteLine($"Wrote {fileMap.Count} entries");
            Console.ReadKey();
        }

        private static Dictionary<string, string> GetFileMap(string directory)
        {
            var len = directory.Length;

            return new DirectoryInfo(directory)
                .GetFiles("*.cs", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(len + 1).Replace("\\", "/"))
                .ToDictionary(p => p.ToLowerInvariant());
        }
    }
}
