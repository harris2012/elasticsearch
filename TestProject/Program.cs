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
            {
                Core core = new Core();

                File.WriteAllText("G:\\1.txt", core.BuildMappings(typeof(Book)), Encoding.UTF8);

            }
        }
    }
}
