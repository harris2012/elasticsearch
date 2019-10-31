using Example;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var type = typeof(Product);

            Core core = new Core();

            File.WriteAllText("G:\\1.txt", core.BuildMappings(type), Encoding.UTF8);

        }

    }
}
