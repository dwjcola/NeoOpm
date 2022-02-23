using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NeoOPM;
using System.Collections;
using System;
using UnityEngine.UI;
using XLua;
using Object = UnityEngine.Object;
using TMPro;
using UNum = NeoOPM.UGuiForm.UNum;
internal class UIViewBaseEditor
{
    const string strEmpty = "";
    #region Field and FieldType
    public class Field : IComparer<Field>
    {
        public string name=strEmpty;
        public FieldType type= FieldType.GameObject;
        public UnityEngine.Object obj = null;
        public string content = string.Empty;
        // int float bool 共用
        public UGuiForm.UNum num;

        public int Compare(Field x, Field y)
        {
            return x.name.CompareTo(y.name);
        }
        public int Compare(Field x, string y)
        {
            return x.name.CompareTo(y);
        }

        public  void CopyTo(Field newObj)
        {
            newObj.name = name;
            newObj.type = type;
            newObj.obj = obj;
            newObj.content = content;
            newObj.num = num;
        }
    }

    public enum FieldType
    {
        Null,
        Transform,
        Image,
        RawImage,
        Button,
        TextTMP,
        Text,
        InputField,
        SpriteRenderer,
        Slider,
        Toggle,
        Canvas,
        ScrollRect,
        Scrollbar,
        AudioSource,
        Animation,
        Animator,
        VLayoutGroup,
        HLayoutGroup,
        GridLayoutGroup,
        GameObject,
        RectTransform,
        Dropdown,
        Array,//CUIListObjsTool
        Int,
        Bool,
        Float,
        String,
    }
    #endregion
    static readonly List<FieldType> baseTypes = new List<FieldType>() { FieldType.Float, FieldType.Int, FieldType.Bool, FieldType.String };
    static readonly List<FieldType> valueTypes = new List<FieldType>() { FieldType.Float, FieldType.Int, FieldType.Bool };
    static readonly Dictionary<FieldType,string> valueTypeKeyExts=new Dictionary<FieldType, string> { { FieldType.Float,"float"}, { FieldType.Int,"int"}, { FieldType.Bool,"bool"} };

    List<Field> fields = null;
    UGuiForm tmpForm;

    static List<string> copykeyList = null;
    static List<Object> copyvalueList = null; 
    static List<string> copystrkeyList = null; 
    static List<string> copystrvalueList = null; 
    static List<string> copyintkeyList = null; 
    static List<int>    copyintvalueList = null;

    public void ShowGUI(UGuiForm form)
    {
        
        if (tmpForm == null)
        {
            tmpForm = form;
            fields = null;
        }
        if(form!=tmpForm)
        {
            tmpForm = form;
            fields = null;
        }

        if (fields == null)
        {
            fields = new List<Field>();
            for (int i = 0; i < form.keyList.Count; i++)
            {
                fields.Add(Get(form.keyList[i], form.valueList[i]));
            }
            for (int i = 0; i < form.strkeyList.Count; i++)
            {
                fields.Add(Get(form.strkeyList[i], form.strvalueList[i]));
            }
            for (int i = 0; i < form.intkeyList.Count; i++)
            {
                fields.Add(Get(form.intkeyList[i], form.intvalueList[i]));
            }

        }
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < fields.Count; i++)
        {
            Field field = fields[i];
            if (field == null)
            {

                Debug.LogError($"意外的null：{i}+ count:{fields.Count}");
                fields.RemoveAt(i);
                continue;
            }
            
            EditorGUILayout.BeginHorizontal();

            // key 
            string originName = field.name;
            //string newName = EditorGUILayout.DelayedTextField(field.name,GUILayout.MaxWidth(200));
            string newName = EditorGUILayout.TextField(field.name, GUILayout.MaxWidth(200));
            newName = newName.Trim().Replace(" ", "");
            if (newName != field.name)
            {
                field.name = newName;
                //修改原始数据
                UpdateData(field, originName,newName);
            }

            // type
            FieldType originType = field.type;
            if(originType!=FieldType.Null)
            {
                //GUILayout.FlexibleSpace();
                FieldType newType = (FieldType)EditorGUILayout.EnumPopup(originType, GUILayout.MaxWidth(130));
                if (newType != originType)
                {
                    //Debug.Log(newType.ToString() + "<<----" + originType.ToString());
                    // 更新类型及数据
                    ChangeType(field, newType);

                    // 更新原始数据
                    UpdateData(field, originType, field.type);
                }
            }
            else
            {
                if (field.obj != null)
                {
                    // 更新类型及数据
                    ChangeType(field, FieldType.Null,true);

                    // 更新原始数据
                    UpdateData(field, originType, field.type);
                }
                //GUILayout.FlexibleSpace();
                EditorGUILayout.EnumPopup(field.type, GUILayout.MaxWidth(130));
            }
            
            // value
            if (field.type == FieldType.Bool)
            {
                bool originbv = field.num.bv;
                
                bool newbv= EditorGUILayout.Toggle(originbv, GUILayout.MaxWidth(200));
                if(newbv != originbv)
                {
                    field.num.bv = newbv;
                    // 更新原始数据
                    UpdateData(field);
                }
            }
            else if (field.type == FieldType.Int)
            {
                int originiv = field.num.iv;
                int newiv = EditorGUILayout.DelayedIntField(originiv, GUILayout.MaxWidth(200));
                if (newiv != originiv)
                {
                    field.num.iv = newiv;
                    // 更新原始数据
                    UpdateData(field);
                }

            }
            else if (field.type == FieldType.Float)
            {
                float originfv = field.num.fv;
                float newfv = EditorGUILayout.DelayedFloatField(originfv, GUILayout.MaxWidth(200));
                if (newfv != originfv)
                {
                    field.num.fv = newfv;
                    // 更新原始数据
                    UpdateData(field);
                }
            }
            else if (field.type == FieldType.String)
            {
                string originStr = field.content;
                string newStr = EditorGUILayout.DelayedTextField(originStr, GUILayout.MaxWidth(200));
                newStr = newStr.Trim().Replace(" ", "");
                if (newStr != originStr)
                {
                    field.content= newStr;
                    // 更新原始数据
                    UpdateData(field);
                }
            }
            else
            {
                Object originObj = field.obj;
                FieldType fieldType = field.type;
                if(fieldType== FieldType.Null)
                {
                    fieldType = FieldType.GameObject;
                }
                Object newObj = EditorGUILayout.ObjectField(field.obj, GetRealType(fieldType),true, GUILayout.MaxWidth(200));

                if (newObj != originObj)
                {
                    field.obj = newObj;
                    // 更新原始数据
                    UpdateData(field);
                    if (originObj == null && newObj != null&& fieldType== FieldType.GameObject)
                    {
                        field.type = FieldType.Null;
                    }
                }
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("-", GUILayout.MaxWidth(20)))
            {
                // 删除一条记录
                // 更新数据
                fields.RemoveAt(i);

                // 更新原始数据
                DeleteData(field);
            }

            EditorGUILayout.EndHorizontal();

            //自动填充 key name
            if(field!=null&&string.IsNullOrEmpty(field.name)&& field.obj != null)
            {
                field.name = field.obj.name;
                UpdateData(field,strEmpty,field.name);
            }

        }

        if (GUILayout.Button("add new feild"))
        {
            fields.Add(new Field());
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("copy list"))
        {
            copykeyList = form.keyList;
            copyvalueList = form.valueList;
            copystrkeyList = form.strkeyList;
            copystrvalueList = form.strvalueList;
            copyintkeyList = form.intkeyList;
            copyintvalueList = form.intvalueList;
        }
        if (GUILayout.Button("paste list"))
        {
            if (copykeyList != null)
                form.keyList.AddRange(copykeyList);
            if(copyvalueList != null)
                form.valueList.AddRange(copyvalueList);
            if(copystrkeyList != null)
                form.strkeyList.AddRange(copystrkeyList);
            if(copystrvalueList != null)
                form.strvalueList.AddRange(copystrvalueList);
            if(copyintkeyList != null)
                form.intkeyList.AddRange(copyintkeyList);
            if(copyintvalueList != null)
                form.intvalueList.AddRange(copyintvalueList);
            fields = null;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();


    }

    public Field Get(string name, Object obj)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        //if (obj == null)
        //{
        //    return null;
        //}
        return new Field { name = name, obj = obj, type = GetType(obj) };
    }
    public Field Get(string name, string str)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        if(str== null)
        {
            str = strEmpty;
        }
        //if (string.IsNullOrEmpty(str))
        //{
        //    return null;
        //}
        return new Field { name = name, content = str, type = FieldType.String };
    }
    public Field Get(string name, int v)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        string[] arr = null;
        if ((arr = name.Split(' ')).Length != 2)
        {
            Debug.LogError($"值类型获取出现异常,name:{name}");
            return new Field { name = name,type= FieldType.Null};
            
        }
        string realKey = arr[0];
        string type = arr[1];
        //Debug.Log($"{realKey},type:{type},{name}");
        if (type == "int")
        {
            return new Field { name = realKey, num ={ iv= v }, type = FieldType.Int };
        }
        else if (type == "float")
        {
            //UGuiForm.UNum num1 = new UGuiForm.UNum();
            //num1.iv = v;
            return new Field { name = realKey, num = { iv = v }, type = FieldType.Float };
        }
        else if (type == "bool")
        {
            return new Field { name = realKey, num = { iv = v }, type = FieldType.Bool };
        }
        else
        {
            Debug.LogError($"发现未知类型：{type},union key:{name}");
        }
        return null;
    }




    public void ChangeType(Field field,FieldType newType,bool isDefault=false)
    {
        if(isDefault)
        {
            GetGameObjectDefaultType(field);
            return;
        }


        FieldType originType = field.type;
        Type realType = GetRealType(newType);
        if(newType==FieldType.Null)
        {
            field.type= newType;
            field.content = strEmpty;
            field.num.iv = 0;
            field.obj = null; 
            return;
        }

        if(realType==null)
        {
            Debug.LogError($"出现异常 Type is null ：{field.name},{field.type}");
            return;
        }
        bool A = baseTypes.Contains(originType);
        bool B = baseTypes.Contains(newType);
        field.type = newType;
        if (A&&B)
        {
            if(originType==FieldType.Float&& newType == FieldType.Int)
            {
                float f = field.num.fv;
                field.num.iv = (int)f;
            }
            else if(originType==FieldType.Int && newType == FieldType.Float)
            {
                int iv = field.num.iv;
                field.num.fv = iv;
            }

        }
        else if (!A && B)
        {
            field.obj = null;
        }
        else if (A &&! B)
        {
            field.content = strEmpty;
            field.num.iv = 0;
        }
        else if (!A && !B)
        {
            if(realType.IsSubclassOf(typeof(Component)))
            {
                var cm = field.obj as Component;
                var go = field.obj as GameObject;
                if (cm != null)
                {
                    field.obj = cm.GetComponent(realType);
                }
                else if (go != null)
                {
                    field.obj = go.GetComponent(realType);
                }
                else
                {
                    field.obj = null;
                }
            }
            else if (realType == typeof(GameObject))
            {
                var cm = field.obj as Component;
                if (cm != null)
                {
                    field.obj = cm.gameObject;
                }
                else
                {
                    field.obj = null;
                }
            }
        }

    }

    private void GetGameObjectDefaultType(Field field)
    {
        FieldType type = FieldType.Null;
        if (field.obj == null)
        {
            return;
        }
        if(!(field.obj is GameObject))
        {
            return;
        }

        GameObject obj= (GameObject)field.obj;
        Component cm = null;
        if ((cm = obj.GetComponent<Button>()) != null)
        {
            type = FieldType.Button;
        }

        else if ((cm = obj.GetComponent<TextMeshProUGUI>()) != null)
        {
            type = FieldType.TextTMP;
        }
        else if ((cm = obj.GetComponent<Text>()) != null)
        {
            type = FieldType.Text;
        }
        else if ((cm = obj.GetComponent<SpriteRenderer>()) != null)
        {
            type = FieldType.SpriteRenderer;
        }
        else if ((cm = obj.GetComponent<Image>()) != null)
        {
            type = FieldType.Image;
        }
        else if ((cm = obj.GetComponent<AudioSource>()) != null)
        {
            type = FieldType.AudioSource;
        }
        else if ((cm = obj.GetComponent<Animation>()) != null)
        {
            type = FieldType.Animation;
        }
        else if ((cm = obj.GetComponent<Animator>()) != null)
        {
            type = FieldType.Animator;
        }
        else if ((cm = obj.GetComponent<InputField>()) != null)
        {
            type = FieldType.InputField;
        }
        else if ((cm = obj.GetComponent<RawImage>()) != null)
        {
            type = FieldType.RawImage;
        }
        else if ((cm = obj.GetComponent<Scrollbar>()) != null)
        {
            type = FieldType.Scrollbar;
        }
        else if ((cm = obj.GetComponent<ScrollRect>()) != null)
        {
            type = FieldType.ScrollRect;
        }
        else if ((cm = obj.GetComponent<Dropdown>()) != null)
        {
            type = FieldType.Dropdown;
        }

        else if ((cm = obj.GetComponent<VerticalLayoutGroup>()) != null)
        {
            type = FieldType.VLayoutGroup;
        }
        else if ((cm = obj.GetComponent<HorizontalLayoutGroup>()) != null)
        {
            type = FieldType.HLayoutGroup;
        }
        else if ((cm = obj.GetComponent<GridLayoutGroup>()) != null)
        {
            type = FieldType.GridLayoutGroup;
        }
        else if ((cm = obj.GetComponent<CUIListObjsTool>()) != null)
        {
            type = FieldType.Array;
        }
        else if ((cm = obj.GetComponent<Canvas>()) != null)
        {
            type = FieldType.Canvas;
        }
        else if ((cm = obj.GetComponent<RectTransform>()) != null)
        {
            type = FieldType.RectTransform;
        }
        else if ((cm = obj.GetComponent<Transform>()) != null)
        {
            type = FieldType.Transform;
        }

        
        if (cm!=null)
        {
            field.type = type;
            field.obj = cm;
        }
        else
        {
            field.type = FieldType.GameObject;
        }

        return ;
    }


    public FieldType GetType(Object obj=null)
    {
        if (obj == null)
            return FieldType.Null;

        if(obj is Button)
        {
            return FieldType.Button;
        }
        
        if (obj is TextMeshProUGUI)
        {
            return FieldType.TextTMP;
        }
        if (obj is Text)
        {
            return FieldType.Text;
        }
        if (obj is Sprite)
        {
            return FieldType.SpriteRenderer;
        }
        if (obj is Image)
        {
            return FieldType.Image;
        }
        if (obj is AudioSource)
        {
            return FieldType.AudioSource;
        }
        if (obj is Animation)
        {
            return FieldType.Animation;
        }
        if (obj is Animator)
        {
            return FieldType.Animator;
        }
        if (obj is InputField)
        {
            return FieldType.InputField;
        }
        if (obj is RawImage)
        {
            return FieldType.RawImage;
        }
        if (obj is Scrollbar)
        {
            return FieldType.Scrollbar;
        }
        if (obj is ScrollRect)
        {
            return FieldType.ScrollRect;
        }
        if (obj is Dropdown)
        {
            return FieldType.Dropdown;
        }

        if (obj is VerticalLayoutGroup)
        {
            return FieldType.VLayoutGroup;
        }
        if (obj is HorizontalLayoutGroup)
        {
            return FieldType.HLayoutGroup;
        }
        if (obj is GridLayoutGroup)
        {
            return FieldType.GridLayoutGroup;
        }
        if (obj is Canvas)
        {
            return FieldType.Canvas;
        }
        
        if (obj is RectTransform)
        {
            return FieldType.RectTransform;
        }
        if (obj is Transform)
        {
            return FieldType.Transform;
        }
        if( obj is CUIListObjsTool)
        {
            return FieldType.Array;
        }
        if (obj is GameObject)
        {
            return FieldType.GameObject;
        }
        return FieldType.Null;
    }

    public Type GetRealType(FieldType type)
    {
        switch (type)
        {
            case FieldType.Null:
                return null;
                
            case FieldType.Transform:
                return typeof(Transform);
                
            case FieldType.Image:
                return typeof(Image);
                
            case FieldType.RawImage:
                return typeof(RawImage);
                
            case FieldType.Button:
                return typeof(Button);
                
            case FieldType.TextTMP:
                return typeof(TextMeshProUGUI);
                
            case FieldType.Text:
                return typeof(Text);
                
            case FieldType.InputField:
                return typeof(InputField);
                
            case FieldType.SpriteRenderer:
                return typeof(Sprite);
                
            case FieldType.Slider:
                return typeof(Slider);
                
            case FieldType.Toggle:
                return typeof(Toggle);
                
            case FieldType.ScrollRect:
                return typeof(ScrollRect);
                
            case FieldType.Scrollbar:
                return typeof(Scrollbar);
                
            case FieldType.AudioSource:
                return typeof(AudioSource);
                
            case FieldType.Animation:
                return typeof(Animation);
                
            case FieldType.Animator:
                return typeof(Animator);
                
            case FieldType.VLayoutGroup:
                return typeof(VerticalLayoutGroup);
                
            case FieldType.HLayoutGroup:
                return typeof(HorizontalLayoutGroup);
                
            case FieldType.GridLayoutGroup:
                return typeof(GridLayoutGroup);
                

            case FieldType.RectTransform:
                return typeof(RectTransform);
                
            case FieldType.Dropdown:
                return typeof(Dropdown);
            case FieldType.Canvas:
                return typeof(Canvas);
            case FieldType.Array:
                return typeof(CUIListObjsTool);
                
            case FieldType.GameObject:
                return typeof(GameObject);
                
            case FieldType.Int:
                return typeof(int);
                
            case FieldType.Bool:
                return typeof(bool);
                
            case FieldType.Float:
                return typeof(float);
                
            case FieldType.String:
                return typeof(string);
        }


        return null;
    }

    /// <summary>
    /// 刷新原始数据的类型
    /// </summary>
    private void UpdateData(Field field, FieldType originType, FieldType newType)
    {
        bool o1=originType == FieldType.String; // string 
        bool o2 = valueTypes.Contains(originType); // value type
        bool o3 = !o1 && !o2; // component or gameObject

        bool n1 = newType == FieldType.String; // string 
        bool n2 = valueTypes.Contains(newType); // value type
        bool n3 = !n1 && !n2; // component or gameObject




        // 如果是o2&&n2 ，因为key字段带有类型，这时候类型改变了，认为key变了，删除原始key，然后添加新的key
        if (o1 && n1|| o3&&n3)
        {
            UpdateData(field,true);
        }
        else
        {
            
            Field field1 = new Field();
            field.CopyTo(field1);
            field1.type = originType;
            // delete origin type data
            DeleteData(field1);

            // add new type data

            UpdateData(field, strEmpty, field.name);
        }
        EditorUtility.SetDirty(tmpForm);

    }
    /// <summary>
    /// 刷新原始数据的key name
    /// 原始数据中的key 一定不为空
    /// </summary>
    /// <param name="field"></param>
    /// <param name="originName">如果是空则添加一条新的数据</param>
    /// <param name="newName">如果是空，则删除对应的数据</param>
    private void UpdateData(Field field,string originName, string newName)
    {
        bool n= string.IsNullOrEmpty(newName);
        

        if (field.type== FieldType.String)
        {

            int index = tmpForm.strkeyList.IndexOf(originName);

            if (n)
            {
                // newName 是空，则删除该原始数据，但是field的数据不删除
                //删除key
                if(index!=-1)
                {
                    tmpForm.strkeyList.RemoveAt(index);
                    tmpForm.strvalueList.RemoveAt(index);
                }
                EditorUtility.SetDirty(tmpForm);
                return;
            }

            if( index!=-1)
            {
                // 修改 key
                tmpForm.strkeyList[index] = newName;
            }
            else
            {
                // 添加 key
                tmpForm.strkeyList.Add(newName);
                tmpForm.strvalueList.Add(field.content);
            }
        }
        else if (valueTypes.Contains(field.type))
        {
            string ext = " " + valueTypeKeyExts[field.type];
            int index = tmpForm.intkeyList.IndexOf(originName + ext);

            if (n)
            {
                // newName 是空，则删除该原始数据，但是field的数据不删除
                //删除key
                if (index != -1)
                {
                    tmpForm.intkeyList.RemoveAt(index);
                    tmpForm.intvalueList.RemoveAt(index);
                }
                EditorUtility.SetDirty(tmpForm);
                return;
            }

            if (index != -1)
            {
                // 修改 key
                tmpForm.intkeyList[index] = newName + ext;
            }
            else
            {
                // 添加 key
                tmpForm.intkeyList.Add(newName + ext);
                tmpForm.intvalueList.Add(field.num.iv);
            }
        }
        else
        {
            int index = tmpForm.keyList.IndexOf(originName);

            if (n)
            {
                // newName 是空，则删除该原始数据，但是field的数据不删除
                //删除key
                if (index != -1)
                {
                    tmpForm.keyList.RemoveAt(index);
                    tmpForm.valueList.RemoveAt(index);
                }
                EditorUtility.SetDirty(tmpForm);
                return;
            }

            if (index != -1)
            {
                // 修改 key
                tmpForm.keyList[index] = newName;
            }
            else
            {
                // 添加 key
                tmpForm.keyList.Add(newName);
                tmpForm.valueList.Add(field.obj);
            }
        }

        EditorUtility.SetDirty(tmpForm);
    }
    /// <summary>
    /// 刷新原始数据
    /// </summary>
    private void UpdateData(Field field,bool updateType=false)
    {
        if (field.type == FieldType.String)
        {
            int index = tmpForm.strkeyList.IndexOf(field.name);
            if (index != -1)
            {
                tmpForm.strvalueList[index] = field.content;
            }
        }
        else if (valueTypes.Contains(field.type))
        {
            string ext = " " + valueTypeKeyExts[field.type];
            int index = tmpForm.intkeyList.IndexOf(field.name + ext);
            if (index != -1)
            {
                //Debug.Log($"{tmpForm.intvalueList[index]}---->>{field.num.iv}");
                tmpForm.intvalueList[index] = field.num.iv;
            }
        }
        else
        {
            int index = tmpForm.keyList.IndexOf(field.name);
            if (index != -1)
            {
                tmpForm.valueList[index] = field.obj;
            }
        }
        EditorUtility.SetDirty(tmpForm);
    }
    /// <summary>
    /// 删除一条原始数据
    /// </summary>
    /// <param name="field"></param>
    private void DeleteData(Field field)
    {
        if (field.type == FieldType.String)
        {
            int index = tmpForm.strkeyList.IndexOf(field.name);
            if (index != -1)
            {
                tmpForm.strkeyList.RemoveAt(index);
                tmpForm.strvalueList.RemoveAt(index);
            }
        }
        else if (valueTypes.Contains(field.type))
        {
            string ext = " " + valueTypeKeyExts[field.type];
            int index = tmpForm.intkeyList.IndexOf(field.name + ext);
            if (index != -1)
            {
                tmpForm.intkeyList.RemoveAt(index);
                tmpForm.intvalueList.RemoveAt(index);
            }

        }
        else
        {
            int index = tmpForm.keyList.IndexOf(field.name);
            if (index != -1)
            {
                tmpForm.keyList.RemoveAt(index);
                tmpForm.valueList.RemoveAt(index);
            }
        }
        EditorUtility.SetDirty(tmpForm);
    }

}

