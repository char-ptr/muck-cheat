using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Lib
{
    public static class ImGuiExtension
    {
        private static Dictionary<int, object> Selected = new Dictionary<int, object>();
        private static Dictionary<int, bool> Droped = new Dictionary<int, bool>();
        private static GUIStyle TogStyle = new GUIStyle();

        public static T DropDown<T>(int id,Rect pos, T[] ent, string var, [CanBeNull] string field)
        {
            if (!Selected.ContainsKey(id))
                Selected.Add(id, Selected.ContainsKey(id) ? Selected[id] : ent[0]);
            if (!Droped.ContainsKey(id))
                Droped.Add(id,false);
            
            TogStyle.normal.textColor = Color.white;
            TogStyle.normal.background = Texture2D.linearGrayTexture;
            TogStyle.padding = new RectOffset(5, 5, 0, 0);
            
            string readAbleValue = field != null
                ? typeof(T).GetField(field).GetValue(Selected[id]).ToString()
                : Selected[id].ToString();
            Droped[id] = GUI.Toggle(pos, Droped[id], $"{var} : {readAbleValue}",TogStyle);

            if (Droped[id])
            {
                int YPos = -20;
                
                GUI.depth = 1;
                GUI.BeginGroup(new Rect(pos.x,pos.y+pos.height, pos.width,ent.Length*20));
            
                foreach (var entry in ent)
                {
                    if (GUI.Button(new Rect(0, YPos += 20, pos.width, 20), field != null ? typeof(T).GetField(field).GetValue(entry).ToString() : entry.ToString()))
                    {
            
                        Selected[id] = entry;
                        Droped[id] = false;
            
                    }
                    
                }
            
                GUI.depth = 10;
                GUI.EndGroup();
                
            }
            return (T)Selected[id];
        }


    }
}