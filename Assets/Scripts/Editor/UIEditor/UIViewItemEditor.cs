using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using NeoOPM;

[CustomEditor(typeof(UIMonoItem))]
public class UIViewItemEditor : Editor
{
    private static UIViewItemEditorWin wind;
    private string UIRootPath = "UIForms";
    private const string panelTxt = "Assets/Resource_MS/LuaScripts/tableRead/TPanel.txt";
    public override void OnInspectorGUI()
    {
        uimono = target as UIMonoItem;
        valueList = uimono.valueList;
        keyList = uimono.keyList;
        strkeyList = uimono.strkeyList;
        strvalueList = uimono.strvalueList;
      
        ShowInfo ();
        ShowStringInfo ( );
        if (GUILayout.Button("打开独立编辑界面"))
        {
            if (wind != null)
            {
                wind.Close();
            }
            wind = UIViewItemEditorWin.OnInit(uimono);
        }
        if (GUILayout.Button("生成资源"))
        {
            UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
            string resPath = AssetDatabase.GetAssetPath(arr[0]);
            string resname = target.name;
            {
                TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(panelTxt);
                if (textAsset == null)
                {
                    return;
                }
                int nret = textAsset.text.IndexOf(resname);
                Resources.UnloadAsset(textAsset);
                if (nret >= 0)
                {
                    Debug.LogError("resname is existed !!! resname:" + resname);
                    return;
                }
            }
            int nstart = resPath.IndexOf(UIRootPath) + UIRootPath.Length + 1;
            int nend = resPath.IndexOf(resname);
            int nlen = nend - nstart;
            string id = resname;
            string path = resPath.Substring(nstart, nlen);
            string tmpStr = path;
            string AssetName = path + resname;
            Debug.Log("ui path:" + path + " resname:" + resname);
            //lua
            string luaFileName = resname + "LUA";
            string clacclua = luaFileName;
            tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            nstart = tmpStr.LastIndexOf("/");
            string luapath = "UI/" + tmpStr.Substring(nstart + 1);
            
            string dataPath = Application.dataPath + "/Resource_MS/LuaScripts/" + luapath;
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            string outfile = dataPath + "/" + luaFileName + ".txt";
            if (!File.Exists(outfile))
            {
                TextAsset textAsset1 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resource_MS/LuaScripts/UI/UICtrlTemplate.txt");
                string tableTxt = textAsset1.text.Replace("UICtrlTemplate", resname);
                Resources.UnloadAsset(textAsset1);
                File.WriteAllText(outfile, tableTxt);
                AssetDatabase.Refresh();
            }
            List<string> lines = new List<string>(File.ReadAllLines(panelTxt));
            string l = "['" + id + "']={";
            l += string.Format(
                "Id='{0}',AssetName='{1}',UIGroupName='Default',AllowMultiInstance=false,PauseCoveredUIForm=false,LuaPath='{2}',LuaName='{3}',Mask=false,ShowHead=false,ShowRes=false,UITween=0,",
                id,AssetName,luapath,clacclua
            );
            l += "},";
            lines.Insert(lines.Count-2,l);
            File.WriteAllLines(panelTxt,lines.ToArray());
        }
        base.OnInspectorGUI();
    }
    private static int _index = 0;
    private static string error = "";
    private static string name = "";
    private static GameObject tempobj = null;
    private static UnityEngine.Object[] tempobjsss = null;
    private static GameObject obj = null;
    protected static UIMonoItem uimono;
    public static List<string> keyList;
    public static List<UnityEngine.Object> valueList;
    public static List<string> strkeyList;
    public static List<string> strvalueList;
    /// <summary>
    /// 显示Action层动作信息
    /// </summary>
    public static void ShowInfo()
    {
        EditorGUILayout.BeginVertical("Box");
        for (int i = 0, len = valueList.Count; i < len; i++)
        {
            GUILayout.BeginHorizontal();
            string key = keyList[i];
            UnityEngine.Object v = valueList[i];
            EditorGUILayout.TextField(key);
            if (v == null)
            {
                Debug.LogError("mono 配置的 key =" + key + "  引用的东西为空"); 
            }
            else
            {
                EditorGUILayout.ObjectField("", v, v.GetType(), true);
                EditorGUILayout.TextField(v.GetType().ToString());
            }
            if (GUILayout.Button("X"))
            {
                valueList.RemoveAt(i);
                keyList.RemoveAt(i);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(uimono.gameObject);
#endif
                return;
            }
            GUILayout.EndHorizontal();
        }
        //ChooseOneObj();
        GUILayout.BeginHorizontal();
        name = EditorGUILayout.TextField(name);
        obj = (GameObject)EditorGUILayout.ObjectField("", obj, obj == null ? typeof(GameObject) : obj.GetType(), true);
        if (obj != null)
        {
            if (tempobj != obj)
            {
                _index = 0;
            }
            tempobj = obj;
            Component[] ss = obj.GetComponents<Component>();
            tempobjsss = new UnityEngine.Object[ss.Length + 1];
            tempobjsss[0] = obj;
            string[] strsss = new string[tempobjsss.Length];
            strsss[0] = typeof(GameObject).ToString();
            int j = 1;
            foreach (Component component in ss)
            {
                tempobjsss[j] = component;
                strsss[j] = component.GetType().ToString();
                j++;
            }
            _index = EditorGUILayout.Popup("", _index, strsss);
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("添加引用"))
        {
            error = "";
            if (string.IsNullOrEmpty(name))
            {
                error = "唯一的key没有设置";
            }
            else if (keyList.IndexOf(name) != -1|| strkeyList.IndexOf ( name ) != -1 )
            {
                error = "唯一的key 【" + name + "】已经存在了";
            }
            else if (obj == null)
            {
                error = "设置的object引用为空";
            }
            else
            {
                valueList.Add(tempobjsss[_index]);
                keyList.Add(name);
                name = "";
                obj = null;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(uimono.gameObject);
#endif
            }
        }
        if ( GUILayout.Button ( "添加数组" ) )
        {
            error = "";
            string arraykey="arraytools";
            if ( string.IsNullOrEmpty ( arraykey ) )
            {
                error = "唯一的key没有设置";
            }
            else if ( keyList.IndexOf ( arraykey ) != -1 || strkeyList.IndexOf ( arraykey ) != -1 )
            {
                error = "【" + arraykey + "】已经存在了";
            }
            else
            {
                var tool=uimono.gameObject.GetComponent<CUIListObjsTool>();
                if ( tool == null )
                {
                    tool = uimono.gameObject.AddComponent<CUIListObjsTool> ( );
                }
                valueList.Add ( tool );
                keyList.Add ( arraykey );
                name = "";
                obj = null;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty ( uimono.gameObject );
#endif
            }
        }
        //CreateSameTypeList();

        if (!string.IsNullOrEmpty(error))
        {
            GUILayout.Space(3f);
            EditorGUILayout.HelpBox(error, MessageType.Error, true);
        }
        EditorGUILayout.EndVertical();
    }

   
    static string tempKey="",tempValue="";
    public static void ShowStringInfo ( )
    {
        EditorGUILayout.BeginVertical ( "Box" );
        for ( int i = 0, len = strvalueList.Count; i < len; i++ )
        {
            GUILayout.BeginHorizontal ( );
            string key = strkeyList[i];
            var v = strvalueList[i];
            EditorGUILayout.TextField ( key );
            if ( v == null )
            {
                Debug.LogError ( "mono 配置的 key =" + key + "  引用的东西为空" );
            }
            else
            {
                EditorGUILayout.TextField ( v );
            }
            if ( GUILayout.Button ( "X" ) )
            {
                strvalueList.RemoveAt ( i );
                strkeyList.RemoveAt ( i );
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty ( uimono.gameObject );
#endif
                return;
            }
            GUILayout.EndHorizontal ( );
        }
        //ChooseOneObj();
        GUILayout.BeginHorizontal ( );

        tempKey = EditorGUILayout.TextField ( tempKey );
        tempValue = EditorGUILayout.TextField ( tempValue );
        GUILayout.EndHorizontal ( );

        if ( GUILayout.Button ( "添加string property" ) )
        {
            error = "";
            if ( string.IsNullOrEmpty ( tempKey ) )
            {
                error = "唯一的key没有设置";
            }
            else if ( keyList.IndexOf ( tempKey ) != -1 || strkeyList.IndexOf ( tempKey ) != -1 )
            {
                error = "唯一的key 【" + tempKey + "】已经存在了";
            }
            else
            {
                strkeyList.Add ( tempKey );
                strvalueList.Add ( tempValue );
                tempKey = ""; tempValue = "";
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty ( uimono.gameObject );
#endif
            }
        }
        //CreateSameTypeList();

        if ( !string.IsNullOrEmpty ( error ) )
        {
            GUILayout.Space ( 3f );
            EditorGUILayout.HelpBox ( error, MessageType.Error, true );
        }
        EditorGUILayout.EndVertical ( );
        }

    }

[CustomEditor(typeof(LUAComponent))]
public class LUAComponentEditor:UIViewItemEditor
{
    public override void OnInspectorGUI()
    {
        uimono = target as LUAComponent;
        LUAComponent lUA = target as LUAComponent;
        valueList = uimono.valueList;
        keyList = uimono.keyList;
        strkeyList = uimono.strkeyList;
        strvalueList = uimono.strvalueList;
        AddProperty();
        GUILayout.BeginHorizontal();
        GUILayout.Label("LUAClassName:");
        lUA.LuaName = EditorGUILayout.TextField(lUA.LuaName);
        GUILayout.EndHorizontal();
        if (GUI.changed) EditorUtility.SetDirty(lUA);
    }
    public virtual void AddProperty()
    {
        ShowInfo();
        ShowStringInfo();
    }
}