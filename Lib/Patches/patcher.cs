using System;
using System.Reflection;
using HarmonyLib;

namespace Lib.Patches
{
    public class patcher
    {
        public static Harmony harm;
        public static void Patch()
        {
            
            harm = new Harmony("cacti.cheat.muck");
            Console.WriteLine("Created harmony");

            
            harm.PatchAll(typeof(Player_Damage).Assembly);
            
        }
    }
}