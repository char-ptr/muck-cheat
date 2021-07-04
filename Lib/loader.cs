using System;
using System.Diagnostics;
using System.IO;
using SharpMonoInjector;
using UnityEngine;

namespace Lib
{
    public class Loader : MonoBehaviour
    {
        public static void Init()
        {
            GameObject _Load = new GameObject();
            _Load.AddComponent<Main>();
            GameObject.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            _Unload();
        }

        public static void InjectLol()
        {
        
            string assemblyPath = Directory.GetCurrentDirectory()+"/dll.dll";
            string @namespace = "dll";
            string className = "Loader";
            string methodName = "Init";
            byte[] assembly;
            
            
            
        
            try {
                assembly = File.ReadAllBytes(assemblyPath);
            } catch {
                Console.WriteLine("Could not read the file " + assemblyPath);
                return;
            }
            
            Injector injector = new Injector("Muck");
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
        private static void _Unload()
        {
            GameObject.Destroy(_Load);
        }
        private GameObject _gameObject;
        static private GameObject _Load;
    }
}