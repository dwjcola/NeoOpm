using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XLua;

[CustomEditor(typeof(UserCoin))]
public class UserCoinEditor :LUAComponentEditor
{
    public static List<string> AssetsName;
    public static List<int> AssetsID;
    
    public override void OnInspectorGUI()
    {
        UserCoin assets = target as UserCoin;
        if (AssetsName == null)
        {
            Refresh();
        }
        base.OnInspectorGUI();
        GUILayout.BeginVertical();
        if (GUILayout.Button("刷新货币表"))
        {
            Refresh();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("货币:");
        assets.AssetID = EditorGUILayout.IntPopup(assets.AssetID, AssetsName.ToArray(), AssetsID.ToArray());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Coin表ID:");
        EditorGUILayout.LabelField(assets.AssetID.ToString());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("UseSelfNum:");
        assets.UseSelfNum = EditorGUILayout.Toggle(assets.UseSelfNum);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
    public void Refresh()
    {
        AssetsName = new List<string>();
        AssetsID = new List<int>();
        
        var luaEnv = XluaManager.instance.LuaEnv;
        string luaName = "LUADefine";
        if (XluaManager.instance.DoLuaString(luaName, "base"))
        {
            LuaTable define = luaEnv.Global.Get<LuaTable>(luaName);
            LuaTable coin = define.Get<LuaTable>("COIN");

            var keys= coin.GetKeys ( );
            foreach ( var key in keys )
            {
                string k = key.ToString();
                coin.Get<string, int> ( k, out int value );
                
                AssetsID.Add(value);
                AssetsName.Add(k);
            }
        }
    }
    
}
