using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 英雄数据
[CreateAssetMenu(fileName = "HeroData", menuName = "英雄展示测试/英雄测试数据")]
[System.Serializable]
public class __HeroData : ScriptableObject
{
    public int rid;    // 不合理，但只是测试用
    public int tid;
    public GameObject prefab;    // spine资源

    public Vector2 scale = Vector2.one;
    public Vector2 offset = Vector2.zero;
    // public string assetPath;
    public __HeroBgSceneData bgSceneData;
}