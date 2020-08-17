using System;
using System.IO;
using SBLScripting;

namespace sbls
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) return;
            var file = args[0];
            if(!File.Exists(file)) Console.WriteLine("Error! File " + args[0] + " does not exist. Exiting...");

            var content = File.ReadAllText(file);
            var compiler = new SBLCompiler(); 
            
            var result = compiler.Parse(content);

            if(!result) Console.WriteLine("Error! File has errors! Exiting...");
            else
            {
                Console.WriteLine("Ok! File is valid");
            }
        }
    }
}
