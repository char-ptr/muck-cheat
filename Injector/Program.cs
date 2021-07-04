using System;
using System.IO;
using System.Reflection;

namespace Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            string strExeFilePath = Assembly.GetExecutingAssembly().Location;
            string dllPath = Directory.GetParent(strExeFilePath) + @"\dll.dll";
            if (!File.Exists(dllPath))
            {
                
                Console.WriteLine($"Did not find dll. @ {dllPath}");
                return;
            
            }
            else
            {
                // inject

                Assembly asm = Assembly.LoadFrom(dllPath);
                asm.GetType("dll.Loader")?.GetMethod("InjectLol")?.Invoke(null, new object[0]);
            }
        }
    }
}