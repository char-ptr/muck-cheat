using UnityEngine;

namespace Lib
{
    public class Settings
    {
        public bool GodMode;
        public bool InfStam;
        public bool InfHung;
        public bool MakeAllChestsFree;
        public bool PlayerMan;
        public bool ShowChestTools;
        public string ChestItemId = "1";
        
        public float SpawnAmount = 1;
        public bool SpawnAtPlayer;
        public PlayerManager Plr;

        public static Rect ItemSpawnerCountPosition = new Rect();
        public static readonly Rect ItemSpawnerScrollViewPosition = new Rect(10, 60, 430, 330);
        public static Vector2 ScrollPos { get; set; } = Vector2.zero;

    }
}