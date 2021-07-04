using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Lib
{
    public static class ImGuiExtension 
    {
        private static Rect Position;
        private static object[] Entries;
        private static object Selected;
        private static bool Droped;
        private static GUIStyle TogStyle = new GUIStyle();

        public static T DropDown<T>(Rect pos, T[] ent, string var, [CanBeNull] string field)
        {
            Position = pos;
            Entries = (object[])((object) ent);
            Selected = Selected ??ent[0];
            
            TogStyle.normal.textColor = Color.white;
            TogStyle.normal.background = Texture2D.linearGrayTexture;
            TogStyle.padding = new RectOffset(5, 5, 0, 0);

            string readAbleValue = field != null
                ? typeof(T).GetField(field).GetValue(Selected).ToString()
                : Selected.ToString();
            Droped = GUI.Toggle(pos, Droped, $"{var} : {readAbleValue}",TogStyle);

            if (Droped)
            {
                int YPos = -20;
                
                GUI.depth = -21;
                GUI.BeginGroup(new Rect(pos.x,pos.y+pos.height, pos.width,Entries.Length*20));

                foreach (var entry in Entries)
                {
                    if (GUI.Button(new Rect(0, YPos += 20, pos.width, 20), field != null ? typeof(T).GetField(field).GetValue(entry).ToString() : entry.ToString()))
                    {

                        Selected = entry;
                        Droped = false;

                    }
                    
                }

                GUI.depth = 0;
                GUI.EndGroup();
                
            }


            return (T)Selected;
        }


    }
}