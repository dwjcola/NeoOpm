using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateEx : Editor
{
    [MenuItem("GameObject/UI/Image")]
    static void CreatImage()
    {
        if(Selection.activeTransform)
        {
            if(Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject obj = new GameObject("Image",typeof(Image));
                obj.GetComponent<Image>().raycastTarget = false;
                obj.transform.SetParent(Selection.activeTransform);
            }
        }
    }
    
    [MenuItem("GameObject/UI/Text - TextMeshPro")]
    static void CreatText()
    {
        if(Selection.activeTransform)
        {
            if(Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject obj = new GameObject("Text",typeof(TextMeshProUGUI));
                obj.GetComponent<TextMeshProUGUI>().raycastTarget = false;
                obj.transform.SetParent(Selection.activeTransform);
            }
        }
    }
}
