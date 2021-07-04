using System;
using System.IO;
using System.Reflection;
using SharpMonoInjector;

namespace Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            string assemblyPath = Directory.GetCurrentDirectory()+"/Lib.dll";
            string @namespace = "Lib";
            string className = "Loader";
            string methodName = "Init";
            byte[] assembly;
            
            try {
                assembly = File.ReadAllBytes(assemblyPath);
            } catch {
                Console.WriteLine("Could not read the file " + assemblyPath);
                return;
            }
            
            SharpMonoInjector.Injector injector = new SharpMonoInjector.Injector("Muck");
            using (injector) {
                IntPtr remoteAssembly = IntPtr.Zero;
        
                try {
                    remoteAssembly = injector.Inject(assembly, @namespace, className, methodName);
                } catch (InjectorException ie) {
                    Console.WriteLine("Failed to inject assembly: " + ie);
                } catch (Exception exc) {
                    Console.WriteLine("Failed to inject assembly (unknown error): " + exc);
                }
        
                if (remoteAssembly == IntPtr.Zero)
                    return;
        
                Console.WriteLine("Injected @ " +
                                  (injector.Is64Bit
                                      ? $"0x{remoteAssembly.ToInt64():X16}"
                                      : $"0x{remoteAssembly.ToInt32():X8}"));
            }
        }
    }
}