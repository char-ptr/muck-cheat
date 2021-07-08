using UnityEngine;

namespace Lib
{
    public class Settings
    {
        public static bool GodMode;
        public static bool InfStam;
        public static bool InfHung;
        
        public static bool PlayerMan;
        public static bool ShowChestTools;
        
        public static float SpawnAmount = 1;
        public static bool SpawnAtPlayer;
        
        
        public static bool Sewerslide;
        
        public static PlayerManager Plr;

        public static InventoryItem.ItemType? ItemFilterType = InventoryItem.ItemType.Item;
        public static string ItemSearch = null;

        public static Rect ItemSpawnerCountPosition = new Rect();
        public static readonly Rect ItemSpawnerScrollViewPosition = new Rect(10, 60, 430, 330);
        public static Vector2 ScrollPos { get; set; } = Vector2.zero;

    }
}