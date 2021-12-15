using UnityEngine;
using System.Collections.Generic;
using System;
using GameFramework;
using GameFramework.AddressableResource;

namespace ProHA
{

/// </summary>
public class UIReminder : MonoBehaviour
{
    private static int CengJiNum = int.MaxValue;
    static UIReminder Instance = null;
    public static void InitReminder()
    {
        if (GameEntry.UI != null)
        {
            MonoBehaviour root=GameEntry.UI.GetUIGroup("Reminder").Helper as MonoBehaviour;
            UIReminder reminder = root.GetComponent<UIReminder>();
            if (reminder == null)
            {
                reminder = root.gameObject.AddComponent<UIReminder>();
            }
        }
    }
    void Awake()
    {
        Instance = this;
        LoadRemindItem();
    }
    public async void LoadRemindItem()
    {
        IAddressableResourceManager resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
        reminderItemObj = await resMgr.LoadAssetAsync<GameObject>(AssetUtility.GetUIFormAsset("Common/ReminderItem")).Task;
        reminderPowerItemObj = await resMgr.LoadAssetAsync<GameObject>(AssetUtility.GetUIFormAsset("Common/ReminderPowerItem")).Task;
    }
    void Start()
    {
    }
    public static Queue<ReminderItem> UpQueue = new Queue<ReminderItem>();
    public static Queue<ReminderItem> UpQueuePool = new Queue<ReminderItem>();
    
    public static Queue<ReminderPowerItem> PowerQueue = new Queue<ReminderPowerItem>();
    public static Queue<ReminderPowerItem> PowerQueuePool = new Queue<ReminderPowerItem>();
    public static int ItemHeight = 40;
    private static GameObject reminderItemObj;
    private static GameObject reminderPowerItemObj;

    /*public static void Show(string DicId, params string[] values)
    {
        Show(TableDic.GetDicValue(DicId, values), Color.white);
    }

    public static void Show(string DicId, Color color, params string[] values)
    {
        Show(TableDic.GetDicValue(DicId, values), color);
    }*/

    public static void ShowByDicId(string DicId, Color color, params string[] values)
    {
        Show(TableDic.GetDicValue(DicId, values), color);
    }

    public static void Show(string content)
    {
        Show(content, Color.white);
    }

    public  void ResetTransform(Transform trans)
    {
        if (trans == null)
            return;
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="power"></param>
    public static void ShowPowerUp(string power,Vector3 target,Action _event=null)
    {
        if (Instance == null) return;
        ReminderPowerItem reminder;
        if (PowerQueuePool.Count <= 0)
        {
            GameObject obj = Instantiate(reminderPowerItemObj) as GameObject;
            reminder = obj.GetComponent<ReminderPowerItem>();
        }
        else
        {
            reminder = PowerQueuePool.Dequeue();
        }
        CommonUtility.SetParent(reminder.transform, Instance.transform);
        reminder.gameObject.SetActive(false);
        reminder.SetContent(power);
        reminder.SetTargetPos(target);
        reminder.SetEndEvent(_event);
        PowerQueue.Enqueue(reminder);
        while(PowerQueue.Count > 10)
        {
            ReminderPowerItem item= PowerQueue.Dequeue();
            RecycleReminder(item);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    public static void Show(string content, Color color)
    {
        if (Instance == null) return;
        ReminderItem reminder;
        if (UpQueuePool.Count <= 0)
        {
            GameObject obj = Instantiate(reminderItemObj) as GameObject;
            reminder = obj.GetComponent<ReminderItem>();
        }
        else
        {
            reminder = UpQueuePool.Dequeue();
        }
        CommonUtility.SetParent(reminder.transform, Instance.transform);
        reminder.gameObject.SetActive(false);
        reminder.SetContent(content);
        reminder.Color = color;
        UpQueue.Enqueue(reminder);
		while(UpQueue.Count > 10)
		{
			ReminderItem item=UpQueue.Dequeue();
            RecycleReminder(item);
		}
    }
    
    long upTimestamp = 0;
    long upTimestamp2 = 0;
    long curTime = 500;
    public  long commonRemindTimer = 1000;
    void Update()
    {
        curTime += (long)(Time.deltaTime * 1000);

        if (UpQueue.Count > 0)
        {
            if (curTime - upTimestamp >= commonRemindTimer)
            {
                ReminderItem item = UpQueue.Dequeue();
                item.SendOut();
                upTimestamp = curTime;
            }
        }
        if (PowerQueue.Count > 0)
        {
            if (curTime - upTimestamp2 >= commonRemindTimer)
            {
                ReminderPowerItem item = PowerQueue.Dequeue();
                item.SendOut();
                upTimestamp2 = curTime;
            }
        }
    }
    public static void RecycleReminder(ReminderItem item)
    {
        item.gameObject.SetActive(false);
        if (item is ReminderPowerItem)
        {
            PowerQueuePool.Enqueue(item as ReminderPowerItem);
        }else
            UpQueuePool.Enqueue(item);
    }
    public static void ClearUIReminder()
    {
        UpQueue.Clear();
    }
}
}