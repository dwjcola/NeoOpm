using NeoOPM;
using System;
using System.Collections.Generic;

public class UnityWebManager 
{
    //  private Queue<UnityWeb> unityWebs = new Queue<UnityWeb>();
    //private static UnityWebManager m_instance;

    //public static UnityWebManager Instance
    //{
    //    get
    //    {
    //        return m_instance;
    //    }
    //}
    //private void Awake()
    //{
    //    m_instance = this;
    //}
    public static void LoadFile(string downloadUrl, string savePath, string downloadFileName, Action action = null)
    {
        LoadFile(new UnityWeb(downloadUrl, savePath, downloadFileName, action));
    }

    public static void LoadFile(UnityWeb unityWeb)
    {
        UnityWebLoadOnceFile unityWebLoadOnce = new UnityWebLoadOnceFile(unityWeb, GameEntry.UI);
        unityWebLoadOnce.LoadFile();
    }

    public static void LoadFile(Queue<UnityWeb> unityWebs, Action finishAction)
    {
        UnityWebLoadQueue unityWebLoadQueue = new UnityWebLoadQueue(unityWebs, GameEntry.UI, finishAction);
        unityWebLoadQueue.LoadFile();
    }



}
