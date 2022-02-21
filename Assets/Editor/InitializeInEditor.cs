using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]
[InitializeOnLoad]
public class InitializeInEditor
{
    // Start is called before the first frame update
    static InitializeInEditor()
    {
        ObjectFactory.componentWasAdded += ComponentWasAdded;
        Register();
    }

    // Update is called once per frame
    static void ComponentWasAdded(Component component)
    {
        //Debug.Log(component);
        if(component is  Coffee.UIExtensions.UIParticle)
        {
            WhenUIParticleAdded(component as Coffee.UIExtensions.UIParticle);
        }
    }

    static void WhenUIParticleAdded(Coffee.UIExtensions.UIParticle particle)
    {
        
        Debug.Log("WhenUIParticleAdded::" + particle+" root"+ particle.transform.root);
        var root = particle.gameObject;
        var arr = root.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var item in arr)
        {
            if(item.gameObject.GetComponent<Coffee.UIExtensions.UIParticle>()==null)
            {
                item.gameObject.AddComponent<Coffee.UIExtensions.UIParticle>();
            }
        }
        
        SetLayerRecursively(particle.gameObject,LayerMask.NameToLayer("UI"));
    }
    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="gameObject"><see cref="GameObject" /> 对象。</param>
    /// <param name="layer">目标层次的编号。</param>
    public static void SetLayerRecursively(GameObject gameObject, int layer)
    {
        List<Transform> list = new List<Transform>();
        gameObject.GetComponentsInChildren(true, list);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.layer = layer;
        }

        list.Clear();
    }

    static bool register = false;

    static void Register()
    {
        if (!register)
        {
            RendererLayerEditor.Register();
            register = true;
        }

    }
}
