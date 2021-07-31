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
                ShowMenu();
                return;
            }

            if ("init".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllText("elasticsearch.json", JsonConvert.SerializeObject(new Param(), Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include }));

                File.WriteAllText("update.bat", CodeResource.Update);

                return;
            }

            if (!File.Exists("elasticsearch.json"))
            {
                Console.WriteLine("elasticsearch.json is required.");
                return;
            }

            var content = File.ReadAllText("elasticsearch.json");
            Param param = JsonConvert.DeserializeObject<Param>(content);

            switch (args[0])
            {
                case "setup":
                    SetupManager.Process(param);
                    break;
                case "update":
                    {
                        //生成 java 代码
                        UpdateManager.Process(param);

                        //生成 mapping 文件
                        MappingManager.Process(param);
                    }
                    break;
                default:
                    break;
            }
        }

        static void ShowMenu()
        {
            var menu = @"1. elasticsearch init 创建项目，生成soa.json模版文件
2. elasticsearch setup 初始化项目
3. elasticsearch update 更新生成的文件
";

            Console.WriteLine(menu);
        }


    }
}
