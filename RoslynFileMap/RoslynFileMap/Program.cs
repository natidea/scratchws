using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynFileMap
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = @"D:\Repos\natideavs\Analyzers";
            var fileMap = GetFileMap(directory);

            Console.ReadKey();
        }

        private static Dictionary<string, string> GetFileMap(string directory)
        {
            var len = directory.Length;

            return new DirectoryInfo(directory)
                .GetFiles("*.cs", SearchOption.AllDirectories)
                .Select(f => f.FullName.Substring(len + 1))
                .ToDictionary(p => p.ToLowerInvariant());
        }
    }
}
