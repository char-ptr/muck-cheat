using System;
using HarmonyLib;

namespace Lib.Patches
{
    [HarmonyPatch(typeof(Player),"Damage")]
    public class Player_Damage
    {
        [HarmonyPrefix]
        static int dam(ref int dam)
        {
            // if (Settings.GodMode)
            //     dam = 0;
            Settings.Sewerslide = true;

            return 0;
        }
    }
}