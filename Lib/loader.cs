using System;
using System.Diagnostics;
using System.IO;
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
        private static void _Unload()
        {
            GameObject.Destroy(_Load);
        }
        private GameObject _gameObject;
        static private GameObject _Load;
    }
}