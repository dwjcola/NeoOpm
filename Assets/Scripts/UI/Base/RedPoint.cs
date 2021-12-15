using UnityEngine;
using System.Collections.Generic;
using XLua;
using System.Linq;
using UnityEngine.UI;
namespace ProHA
{

public class TriggerParam
{
    public int triggerID ;
    public int triggerNum ;
}
/// <summary>
/// 红点提示统一组件
/// </summary>
public class RedPoint : MonoBehaviour
{
    private static Dictionary<int,int> TriggerList = new Dictionary<int,int>();//id,num
    private static List<RedPoint> mRedPointList = new List<RedPoint>();

    public List<int> TriggerID;
    private int[] CurTriggerArr;//这里记录当前此节点所有已响应的红点id
    private int[] CurTrigCount;//这里记录了 CurTriggerArr中红点id对自己的数量，目前_TriggerCount 是拿这个算的，应该会比较准
    private int _TriggerCount = 0;//这里记录了当前此节点所有红点id响应数量
    private int triggerCount {
        get {
            return _TriggerCount;
        }
        set
        {
            _TriggerCount = value;
            Triggerried = _TriggerCount>0;
            if (_TriggerCount <= 0)//这个红点不再显示
            {
                ResetRedPoint();
            }
            else
            {
                if (mImage != null)
                    mImage.enabled = true;
                ShowTriggerNum();
            }
        }
    }

    public bool showNumber = true;

    public Image mImage;
    public TMPro.TextMeshProUGUI TextMeshPro;
    int len = 0;

    bool Triggerried = false;

    void Awake()
    {
        if (mImage == null)
        {
            mImage = GetComponent<Image>();
        }
        if (TextMeshPro != null)
        {
            TextMeshPro.text = "";
        }
        
        if (TriggerID == null || TriggerID.Count == 0) return;
        len = TriggerID.Count;
        CurTriggerArr = new int[len];
        CurTrigCount = new int[len];
        
        if (!mRedPointList.Contains(this))
        {
            mRedPointList.Add(this);
        }
        ReTriggerSelf();
    }
    /// <summary>
    /// 重置当前红点的所有状态，用于初始化、回收等
    /// </summary>
    public void ResetRedPoint()
    {
        if (TriggerID == null) return;
        len = TriggerID.Count;
        CurTriggerArr = new int[len];
        CurTrigCount = new int[len];

        if (mImage != null)
            mImage.enabled = false;
        if (!mRedPointList.Contains(this))
        {
            mRedPointList.Add(this);
        }
        if (TextMeshPro != null)
            TextMeshPro.text = "";
        _TriggerCount = 0;
    }
    /// <summary>
    /// 重新计算了当前节点的红点响应数量
    /// </summary>
    public void ReTriggerSelf()
    {
        for (int i = 0; i < TriggerID.Count; i++)
        {
            int num = 0;
            if (TriggerList.TryGetValue(TriggerID[i],out num))
            {
                CurTriggerArr[i] = TriggerID[i];
                CurTrigCount[i] = num;
            }
        }
        ReCalculateTriggerCount();
    }
    public void SetTriggerIdList(LuaTable IDs)
    {
        List<int> iddds = new List<int>();
        foreach (var item in IDs.GetKeys())
        {
            iddds.Add(IDs.Get<object, int>(item));
        }
        TriggerID = iddds;
        ResetRedPoint();
        ReTriggerSelf();
    }
    public void SetRedPoint(int triggerID,int num)
    {
        List<int> iddds = new List<int>();
        iddds.Add(triggerID);
        TriggerID = iddds;
        ResetRedPoint();
        ReTriggerSelf();
        UpdataTrigger(triggerID, num);
    }
    public void ClearRedPoint()
    {
        TriggerID.Clear();
        ResetRedPoint();
        ReTriggerSelf();
    }
    public void initTriggerID(int id)
    {
        TriggerID =new List<int>();
        TriggerID.Add(id);
    }
 
    void OnEnable()
    {
        if (mImage != null)
            mImage.enabled = triggerCount>0;
        ShowTriggerNum();
    }

    void OnDestroy()
    {
        mRedPointList.Remove(this);
        mImage = null;
    }

    public void Remove()
    {
        mRedPointList.Remove(this);
    }

    public void OnTrigger(int id, int num)
    {
        var index=TriggerID.IndexOf(id);
        if (index >= 0)
        {
            CurTriggerArr[index] = id;
            CurTrigCount[index] += num;
            ReCalculateTriggerCount();
        }
    }
    /// <summary>
    /// 削减触发数量
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public void OnDecreaseNum(int id,int num)
    {
        var index = TriggerID.IndexOf(id);
        if (index >= 0)
        {
            CurTrigCount[index] -= num;
            if (CurTrigCount[index] <= 0)
            {
                CurTrigCount[index] = 0;
                CurTriggerArr[index] = 0;
            }
            ReCalculateTriggerCount();
        }
    }

    public void ReCalculateTriggerCount()
    {
        triggerCount=CurTrigCount.Sum();
    }
    public void ShowTriggerNum()
    {
        if (TextMeshPro != null)
        {
            TextMeshPro.text = (showNumber&&_TriggerCount>0) ?_TriggerCount.ToString():"";
            TextMeshPro.gameObject.SetActive(showNumber);
        }
    }

    public static bool HasTriggerID(int id)
    {
        return TriggerList.ContainsKey(id);
    }/// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num">如果不传，默认显示1</param>
    /// <param name="Immediate"></param>
    public static void PushTriggerID(int id,int num=1, bool Immediate=true)
    {
        if (!TriggerList.ContainsKey(id))
        {
            TriggerList.Add(id,num);

            
            //Note: anonymous action will cause GC, foreach on IList will not case gc now.
            //            mRedPointList.ForEach((point) =>
            //            {
            //                point.OnTrigger(id, Immediate);
            //            });
        }
        else
        {
            TriggerList[id] += num;
        }
        foreach (var p in mRedPointList)
        {
            p.OnTrigger(id, num);//只是加
        }
    }
    /// <summary>
    /// 加减可以统一走这个
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public static void UpdataTrigger(int id,int num=1)
    {
        if (!TriggerList.ContainsKey(id))
        {
            TriggerList.Add(id,num);
        }
        else
        {
            TriggerList[id] = num;
        }
        if (TriggerList[id] <= 0)
        {
            TriggerList.Remove(id);
        }
        foreach (var p in mRedPointList)
        {
            p.OnTriggerUpdate(id, num);//覆盖
        }
    }

    public void OnTriggerUpdate(int id,int num)
    {
        var index=TriggerID.IndexOf(id);
        if (index >= 0)
        {
            CurTriggerArr[index] = id;
            CurTrigCount[index] = num;
            ReCalculateTriggerCount();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num">如果是0，则移除所有红点</param>
    public static void RemoveID(int id,int num=0)
    {
        if (!TriggerList.ContainsKey(id))
            return;
        var Count = TriggerList[id];
        if (num > Count||num==0)
            num = Count;
        TriggerList[id] -= num;
        if (TriggerList[id] <= 0)
        {
            TriggerList.Remove(id);
        }
        mRedPointList.ForEach((point) => { point.OnDecreaseNum(id, num); });//只是修改数量
    }

    public static void ResetAllTriggerIds()//重置所有红点id，避免切换角色后上一角色红点id导致新角色红点异常
    {
        TriggerList.Clear();

    }
    //在本地标记此红点是否触发过
    public static void SaveOpenedInID(int id, bool open)
    {
        string valName = string.Format("RedPoint_{0}_{1}", "GUID", id);
        PlayerPrefs.SetInt(valName, (open) ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool OpenedInID(int id)
    {
        string valName = string.Format("RedPoint_{0}_{1}","GUID" , id);
        int value = PlayerPrefs.GetInt(valName);

        return value == 1;
    }


   
    public static void SaveIDValue(int id, int value)
    {
        string valName = string.Format("RedPoint_{0}_{1}", "GUID", id);
        PlayerPrefs.SetInt(valName, value);
        PlayerPrefs.Save();
    }

    public static int getIDValue(int id)
    {
        string valName = string.Format("RedPoint_{0}_{1}", "GUID", id);
        int value = PlayerPrefs.GetInt(valName);

        return value;
    }
}
    
}