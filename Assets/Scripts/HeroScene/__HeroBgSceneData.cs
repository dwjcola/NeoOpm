using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 背景场景模板数据
[CreateAssetMenu(fileName = "HeroBgSceneData", menuName = "英雄展示测试/场景测试数据")]
[System.Serializable]
public class __HeroBgSceneData : ScriptableObject
{
    // 背景场景类型
    public enum HeroBGSceneType
    {
        village,
        rome01
    }
    
    public HeroBGSceneType bgSceneType;
    public GameObject prefab;
    // public string assetPath;
}
