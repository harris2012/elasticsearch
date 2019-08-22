using ElasticSearch.Loader;
using ElasticSearch.Manager;
using ElasticSearch.Template;
using ElasticSearch.Template.CodeFirst;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("operation is required.");
                Pause();
                return;
            }

            if ("init".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllText("elasticsearch.json", JsonConvert.SerializeObject(new Param(), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include }));

                File.WriteAllText("setup.bat", CodeResource.Setup);

                File.WriteAllText("update.bat", CodeResource.Update);

                return;
            }

            if (!File.Exists("elasticsearch.json"))
            {
                Console.WriteLine("elasticsearch.json is required.");
                Pause();
                return;
            }

            var content = File.ReadAllText("elasticsearch.json");
            Param param = JsonConvert.DeserializeObject<Param>(content);

            switch (args[0])
            {
                case "setup":
                    SetupManager.Setup(param);
                    break;
                case "update":
                    UpdateManager.Process(param);
                    break;
                default:
                    break;
            }
        }

        static void Pause()
        {
            Console.WriteLine("Press any key to continue.");
            //Console.ReadKey();
        }





    }
}
