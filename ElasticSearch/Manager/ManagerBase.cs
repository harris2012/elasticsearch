using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch.Manager
{
    public abstract class ManagerBase
    {

        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false);

        protected static void WriteToFile(string path, string content)
        {
            var fileDirectory = Path.GetDirectoryName(path);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            File.WriteAllText(path, content, DefaultEncoding);
        }

        protected static void WriteToFile(string path, byte[] bytes)
        {
            var fileDirectory = Path.GetDirectoryName(path);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            File.WriteAllBytes(path, bytes);
        }
    }
}
