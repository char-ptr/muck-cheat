using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using UnityEngine;
namespace Lib
{
    public class Main : MonoBehaviour
    {
        public void Start()
        {
            
            
        }
        public void Update()
        {
            inGame = GameManager.state == GameManager.GameState.Playing;
            if (Set.GodMode)
                PlayerStatus.Instance.hp = PlayerStatus.Instance.maxHp;
            if (Set.InfStam)
                PlayerStatus.Instance.stamina = PlayerStatus.Instance.maxStamina;
            if (Set.InfHung)
                PlayerStatus.Instance.hunger = PlayerStatus.Instance.maxHunger;
        }
        public void OnGUI()
        {
            // if (!inGame)
            // {
            //     
            //     GUI.Label(new Rect( 20, 20, 240, 100 ),"load into game");
            // }
            //
            // else 
                GUI.Window(0,new Rect( 20, 20, 240, 500 ),DoMyWindow,"main window");
            
            if (Set.PlayerMan) 
                GUI.Window(1,new Rect( 240+40, 20, 240, 500 ),PlayerWindow,"Player Manager");
            if (Set.ShowChestTools) 
                GUI.Window(2,new Rect( (240+40)*2, 20, 520, 400 ),ChestWindow,"Spawn Tools");
        }

        void ChestWindow(int winid)
        {
            GUI.DragWindow(new Rect(0, 0, 0x1000, 30));
            Settings.ScrollPos = GUI.BeginScrollView(Settings.ItemSpawnerScrollViewPosition, Settings.ScrollPos, Settings.ItemSpawnerCountPosition, false, true);
            int x = 0;
            int y = 0;

            for (int i = 0; i < ItemManager.Instance.allScriptableItems.Count(); i++)
            {
                InventoryItem item = ItemManager.Instance.allScriptableItems[i];

                if (GUI.Button(new Rect(x, y, 50, 50), new GUIContent(item.sprite.texture, "Spawn " + item.name)))
                    if (Set.SpawnAtPlayer)
                        SpawnItem(item.id, (int)Set.SpawnAmount,Set.Plr.transform.position);
                    else
                        SpawnItem(item.id, (int)Set.SpawnAmount);

                if (i != 0 && i % 7 == 0)
                { 
                    x = 0; y += 60; 
                }
                else
                    x += 60;
            }
            GUI.EndScrollView();
            Set.SpawnAmount = GUI.VerticalSlider(new Rect(450, 60, 50, 330), Set.SpawnAmount, 666.0f, 1.0f);
            GUI.Label(new Rect(465, 60, 50, 330),$"a:{(int)Set.SpawnAmount}");
            Settings.ItemSpawnerCountPosition = new Rect(0, 0, 0, y+60);
        }
        void PlayerWindow(int winid)
        {
            int Ypos = 20;
            Set.Plr = ImGuiExtension.DropDown<PlayerManager>(new Rect(20, Ypos+=20, 210, 20), GameManager.players.Select(v=>v.Value).ToArray(), "Player","username");
            
            if (GUI.Button(new Rect(10, Ypos += 40, 220, 20), "Kill Player"))
            {

                ClientSend.PlayerHit(43444,Set.Plr.hitable.GetId(),434343,2,new Vector3());
            }
            if (GUI.Button(new Rect(10, Ypos += 40, 220, 20), "Revive Player"))
            {

                ClientSend.RevivePlayer(Set.Plr.id,55555,true);
                
            }
            Set.SpawnAtPlayer = GUI.Toggle(new Rect(10,Ypos+=20,220,20),Set.SpawnAtPlayer,"Spawn Items at player");
            
        }

        void wait()
        {
            
        }
        void SpawnItem(int id, int a,Vector3 pos)
        {
            Chest[] ches = ChestManager.Instance.chests.Where(v => !v.Value.inUse).Select(v=>v.Value).ToArray();
            System.Random r = new System.Random();
            Chest che = ches[r.Next(0, ches.Length)];
            ClientSend.ChestUpdate(che.id,1,id,a);
            base.Invoke("wait", (float)(NetStatus.GetPing() * 2) / 1000f);
            var item = ChestManager.Instance.chests[che.id].cells[1];
            InventoryUI.Instance.AddItemToInventory(item);
            ClientSend.DropItemAtPosition(item.id,(int)Set.SpawnAmount,pos);
            return;
        }
        InventoryItem SpawnItem(int id, int a)
        {
            Chest[] ches = ChestManager.Instance.chests.Where(v => !v.Value.inUse).Select(v=>v.Value).ToArray();
            System.Random r = new System.Random();
            Chest che = ches[r.Next(0, ches.Length)];
            ClientSend.ChestUpdate(che.id,1,id,a);
            base.Invoke("wait", (float)(NetStatus.GetPing() * 2) / 1000f);
            var item = ChestManager.Instance.chests[che.id].cells[1];
            InventoryUI.Instance.AddItemToInventory(item);
            return item;
        }
        
        void DoMyWindow(int windowID)
        {
            int Ypos = 20;

            // toggles
            Set.GodMode = GUI.Toggle(new Rect(10,Ypos+=20,100,20),Set.GodMode,"toggle gm");
            Set.InfHung = GUI.Toggle(new Rect(10,Ypos+=20,100,20),Set.InfHung,"toggle inf hung");
            Set.InfStam = GUI.Toggle(new Rect(10,Ypos+=20,100,20),Set.InfStam,"toggle inf stam");
            Set.PlayerMan = GUI.Toggle(new Rect(10,Ypos+=20,220,20),Set.PlayerMan,"toggle Player tools");
            Set.ShowChestTools = GUI.Toggle(new Rect(10,Ypos+=20,220,20),Set.ShowChestTools,"toggle Spawn Tools");
            // Set.MakeAllChestsFree = GUI.Toggle(new Rect(10,80,100,20),Set.MakeAllChestsFree,"All free chests");
            
            //buttons

            if (GUI.Button(new Rect(10, Ypos+=20, 100, 20), "All free chests"))
            {
                Set.MakeAllChestsFree = true;
                Console.WriteLine("your mother");
                foreach (var chest in FindObjectsOfType<LootContainerInteract>())
                {
                    chest.price = -1;
                    Console.WriteLine("update");
                }
            }
            if (GUI.Button(new Rect(10, Ypos+=20, 100, 20), "Pickup all"))
            {
                foreach (var item in FindObjectsOfType<PickupInteract>())
                {
                    item.Interact();
                }
            }if (GUI.Button(new Rect(10, Ypos+=20, 100, 20), "kill all mobs"))
            {
                foreach (var mob in FindObjectsOfType<HitableMob>())
                {
                    ClientSend.PlayerDamageMob(mob.GetId(), 323232, 3, 1, new Vector3(), 0);
                }
            }
            if (GUI.Button(new Rect(10, Ypos+=20, 100, 20), "fix boat"))
            {
                foreach (var boat in FindObjectsOfType<Boat>())
                {
                    // fuck ur private loser
                    typeof(Boat).GetMethod("SendBoatFinished", BindingFlags.NonPublic | BindingFlags.Instance)?
                        .Invoke(boat,null);
                }
            }
            if (GUI.Button(new Rect(10, Ypos+=20, 100, 20), "Leave island"))
            {
                ClientSend.Interact(FindObjectOfType<FinishGameInteract>().GetId());
            }
            if (GUI.Button(new Rect(10, Ypos+=20, 100, 20), "Teleport to boat"))
            {
                PlayerMovement.Instance.transform.position = FindObjectOfType<Boat>().transform.position + new Vector3(0,20,0);
            }
            if (GUI.Button(new Rect(10, Ypos += 40, 220, 20), "Give infinite money"))
            {

                SpawnItem(3, 232323);
            }
            if (GUI.Button(new Rect(10, Ypos += 20, 220, 20), "Unlock all achievements"))
            {

                var ah = AchievementManager.Instance;
                
                ah.Karlson();
                ah.Jump();

                foreach (var ach in Achievements)
                {
                    ach.RunAntiSuicide();
                }
                
            }
            //label
            GUI.color = Color.red;
            GUI.Label(new Rect(10, Ypos+=20, 220, 20),"THIS WILL TAKE A WHILE.");
            GUI.color = Color.white;
            GUI.Label(new Rect(10, Ypos+=20, 100, 20),"by pozm");
            GUI.Label(new Rect(10, Ypos+=20, 100, 20),Settings.ItemSpawnerCountPosition.height.ToString());
            
            GUI.BringWindowToFront(windowID);
        }
        
        
        
        private Settings Set = new Settings();
        private bool inGame = false;
        private GUIContent content;

        private AchievementAntiSuicideHelper[] Achievements =
        {
            //You're not a fish & Muck this game
            new AchievementAntiSuicideHelper("AddDeath",101,new object[] {PlayerStatus.DamageType.Drown}),
            // movement shit
            new AchievementAntiSuicideHelper("MoveDistance",100,new object[] {1000,1000}),
            new AchievementAntiSuicideHelper("Jump",10000,new object[] {}),
            new AchievementAntiSuicideHelper("NewDay",100,new object[] {100000}),
            new AchievementAntiSuicideHelper("OpenChest",100,new object[] {}),
            new AchievementAntiSuicideHelper("WieldedWeapon",1,new object[] {ItemManager.Instance.allItems.Single(v=>v.Value.name== "Night Blade").Value}),
            // diff
            new AchievementAntiSuicideHelper("StartGame",1,new object[] {GameSettings.Difficulty.Easy}),
            new AchievementAntiSuicideHelper("StartGame",1,new object[] {GameSettings.Difficulty.Normal}),
            new AchievementAntiSuicideHelper("StartGame",1,new object[] {GameSettings.Difficulty.Gamer}),
            new AchievementAntiSuicideHelper("ItemCrafted",1,new object[] {ItemManager.Instance.allItems.Single(v=>v.Value.name== "Coin").Value,2222222}),

            new AchievementAntiSuicideHelper("CheckGameOverAchievements",1,new object[] {-3},
                () =>
                {
                    GameManager.instance.onlyRock = true;
                    GameManager.instance.damageTaken = false;
                    GameManager.instance.powerupsPickedup = false;
                    GameManager.instance.currentDay = 0;
                    GameManager.gameSettings.gameMode =GameSettings.GameMode.Survival ;
                }),            
            new AchievementAntiSuicideHelper("CheckGameOverAchievements",1,new object[] {-3},
                () =>
                {
                    GameManager.instance.onlyRock = true;
                    GameManager.instance.damageTaken = false;
                    GameManager.instance.powerupsPickedup = false;
                    GameManager.instance.currentDay = 0;
                    GameManager.gameSettings.gameMode =GameSettings.GameMode.Survival ;
                    NetworkController.Instance.nPlayers = 2;
                }),
            new AchievementAntiSuicideHelper("CheckGameOverAchievements",1,new object[] {-3},
                () =>
                {
                    GameManager.instance.onlyRock = true;
                    GameManager.instance.damageTaken = false;
                    GameManager.instance.powerupsPickedup = false;
                    GameManager.instance.currentDay = 0;
                    GameManager.gameSettings.gameMode =GameSettings.GameMode.Survival ;
                    NetworkController.Instance.nPlayers = 4;
                }),
            new AchievementAntiSuicideHelper("CheckGameOverAchievements",1,new object[] {-3},
                () =>
                {
                    GameManager.instance.onlyRock = true;
                    GameManager.instance.damageTaken = false;
                    GameManager.instance.powerupsPickedup = false;
                    GameManager.instance.currentDay = 0;
                    GameManager.gameSettings.gameMode =GameSettings.GameMode.Survival ;
                    NetworkController.Instance.nPlayers = 8;
                }),
            new AchievementAntiSuicideHelper("StartBattleTotem",1000,new object[] {}),
            new AchievementAntiSuicideHelper("ReviveTeammate",5,new object[] {}),
            new AchievementAntiSuicideHelper("PickupPowerup",1,new object[] {"Danis Milk"}, () =>
            {
                ((int[]) typeof(PowerupInventory).GetField("powerups").GetValue(null))[
                    ItemManager.Instance.stringToPowerupId["Danis Milk"]] = 100;
            }),
            new AchievementAntiSuicideHelper("AddKill",10000,new object[] {PlayerStatus.WeaponHitType.Ranged}),

        };
    }

    class AchievementAntiSuicideHelper
    {
        private string MethodName;
        private int Amount;
        private object[] Parameters;
        private static Type ty= typeof(AchievementManager);
        private Action setup; 

        public AchievementAntiSuicideHelper(string mn, int a, object[] p)
        {
            MethodName = mn;
            Amount = a;
            Parameters = p;
        }
        public AchievementAntiSuicideHelper(string mn, int a, object[] p,Action s)
        {
            MethodName = mn;
            Amount = a;
            Parameters = p;
            setup = s;
        }
        public void RunAntiSuicide()
        {
            if (setup != null)
                setup();
            var method = ty.GetMethod(MethodName);
            if (method == null)
                return;
            for (int i = 0; i < Amount; i++)
            {
                method?.Invoke(AchievementManager.Instance,Parameters);
            }
        }
    }


}