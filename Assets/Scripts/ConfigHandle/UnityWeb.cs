using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWeb
{

    public UnityWebRequest webRequest;

    public string downloadUrl;

   

    public string savePathAndName;

    public string downloadFileName;

    public Action action;

    public UnityWeb(string downloadUrl, string savePath, string downloadFileName, Action action = null)
    {
        this.webRequest = UnityWebRequest.Get(downloadUrl);
        this.downloadUrl = downloadUrl;
        this.downloadFileName = downloadFileName;
        this.savePathAndName = savePath ;
        this.action = action;
    }

    public float GetDownloadProgress()
    {
        if (webRequest == null)
            return 0;
        if (webRequest.isDone)
            return 1;
        else if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            return 0;
        else
            return webRequest.downloadProgress;
    }
}

public class UnityWebLoad
{
    public MonoBehaviour mono;
    public IEnumerator Down(UnityWeb unityWeb, Action action = null)
    {
        //发送请求
        unityWeb.webRequest.timeout = 30;//设置超时，若webRequest.SendWebRequest()连接超时会返回，且isNetworkError为true

        yield return unityWeb.webRequest.SendWebRequest();

        if (unityWeb.webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Download Error:" + unityWeb.webRequest.error);
        }
        else
        {
            //获取二进制数据
            var File = unityWeb.webRequest.downloadHandler.data;
            //创建文件写入对象
            FileStream nFile = new FileStream(unityWeb.savePathAndName, FileMode.Create);
            //写入数据
            nFile.Write(File, 0, File.Length);

            nFile.Close();

            if (unityWeb.action != null)
                unityWeb.action();
            if (action != null)
                action();
        }
    }

}

public class UnityWebLoadOnceFile : UnityWebLoad
{
    UnityWeb unityWeb;

    public UnityWebLoadOnceFile(UnityWeb unityWeb, MonoBehaviour mono)
    {
        this.mono = mono;
        this.unityWeb = unityWeb;
    }

    public void LoadFile()
    {
        mono.StartCoroutine(Down(unityWeb));
    }
}
public class UnityWebLoadQueue : UnityWebLoad
{

    private Queue<UnityWeb> unityWebs;

    private Action finishAction;
    public UnityWebLoadQueue(Queue<UnityWeb> unityWebs, MonoBehaviour mono, Action finishAction)
    {
        this.mono = mono;
        this.unityWebs = unityWebs;
        this.finishAction = finishAction;
    }

    public void LoadFile()
    {
        if (unityWebs.Count > 0)
        {
            UnityWeb unityWeb = unityWebs.Dequeue();
            mono.StartCoroutine(Down(unityWeb, LoadFile));
        }
        else
        {
            if (this.finishAction != null)
                this.finishAction();
        }
    }
}
