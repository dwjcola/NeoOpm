#if DEBUG || UNITY_EDITOR
#define LOG_TO_CONSOLE
#define LOG_TO_FILE 
#endif
using UnityEngine;
using System.IO;
using System.Text;
using System;

public static class Logger
    {
    #region Log To File
    static readonly string logFile = string.Format("{0}/cq_{1}_{2}.log", UnityEngine.Application.persistentDataPath, SystemInfo.deviceModel, SystemInfo.operatingSystem.Replace("/", "_"));
    static readonly string preLogFile = string.Format("{0}/cq_{1}_{2}_prev.log", UnityEngine.Application.persistentDataPath, SystemInfo.deviceModel, SystemInfo.operatingSystem.Replace("/", "_"));
    static FileStream logFStream;
    const int maxLog = 10;
    //const long maxLogFileSize = 10 * 1024 * 1024;
    static StringBuilder logBuilder = new StringBuilder(maxLog);

    static string logFileForTable = string.Format("{0}/TableCount{1}.log", UnityEngine.Application.persistentDataPath, System.DateTime.Now.ToString().Replace("/", "_").Replace(":", "_"));

    //static readonly string preLogFileForTable = string.Format("{0}/TableCountLog_{1}_{2}_prev.log", UnityEngine.Application.persistentDataPath, SystemInfo.deviceModel, System.DateTime.Now.ToString().Replace("/", "_"));
    static FileStream logFStreamForTable;
    static StringBuilder logBuilderForTable = new StringBuilder(maxLog);

    private static bool init=false;

    public static void SwichState (bool state )
        {
        if ( state )
            {
            Init ( );
            }
        else
            {
            Close ( );
            }
        }
    private  static void Init ( )
        {
        if ( init )
            {
            return;
            }
        Close ( );
        UnityEngine.Debug.unityLogger.logEnabled = true;
        try
        {
            UnityEngine.Debug.Log(logFile);
            if (File.Exists(preLogFile))
            {
                File.Delete(preLogFile);
            }
            if (File.Exists(logFile))
            {
                File.Move(logFile, preLogFile);
            }
            logFStream = new FileStream(logFile, FileMode.OpenOrCreate);
            //TryTruncate();
            logFStream.Position = logFStream.Length;
            logBuilder.AppendLine(System.DateTime.Now.ToString());

            //UnityEngine.Debug.LogError(logFileForTable);
            if (File.Exists(logFileForTable))
            {
                File.Delete(logFileForTable);
            }
            logFStreamForTable = new FileStream(logFileForTable, FileMode.OpenOrCreate);
            logFStreamForTable.Position = logFStreamForTable.Length;
        }
        catch 
        {
            logFStream = null;
            logFStreamForTable = null;
        }
        UnityEngine.Application.logMessageReceivedThreaded += CatchUnityException;
        init = true;
        }

    public static void Close ()
	{
        if (logFStream != null)
        {
            Flush();
            logFStream.Close();
            logFStream = null;
            UnityEngine.Application.logMessageReceivedThreaded -= CatchUnityException;
        }
        if (logFStreamForTable != null)
        {
            FlushTable();
            logFStreamForTable.Close();
            logFStreamForTable = null;
        }
        
        init = false;
	}
    private static void CatchUnityException(string a, string b, UnityEngine.LogType t)
    {
        if (t == UnityEngine.LogType.Error || t == UnityEngine.LogType.Exception)
        {
            Log(string.Format("{0:H:mm:ss} [EXCEPTION] {1}", System.DateTime.Now,t.ToString() + a + b ));
        }
    }
    public static void Flush ()
	{
        if (logFStream != null)
        {
            var log = logBuilder.ToString();

            if (!string.IsNullOrEmpty(log))
            {
                logBuilder.Remove(0, logBuilder.Length);
                var bytes = Encoding.UTF8.GetBytes(log);
                var count = Encoding.UTF8.GetByteCount(log);
                logFStream.Write(bytes, 0, count);
            }
        }
    }

    public static void FlushTable()
    {
        if (logFStreamForTable != null)
        {
            var log = logBuilderForTable.ToString();

            if (!string.IsNullOrEmpty(log))
            {
                logBuilderForTable.Remove(0, logBuilderForTable.Length);
                var bytes = Encoding.UTF8.GetBytes(log);
                var count = Encoding.UTF8.GetByteCount(log);
                logFStreamForTable.Write(bytes, 0, count);
            }
        }
    }

    static void TryTruncate() {
		//if (logFStream != null) {
		//	if (logFStream.Length >= maxLogFileSize) {
		//		logFStream.SetLength(0);
		//	}
		//}
	}

    public static void LogTable(string message)
    {
        FlushTable();
        if (logBuilderForTable != null)
        {
            logBuilderForTable.AppendLine(message);
        }
    }

	static void Log(string message)
	{
        //if (logBuilder.Length + message.Length > (maxLog - 8))
        {
            Flush();
        }

        logBuilder.AppendLine(message);
    }
	#endregion

	#region Exception
	public static void Exception (System.Exception exception)
	{
		UnityEngine.Debug.LogException (exception);
        if (true)
        {
            Log(string.Format("{0:H:mm:ss} [EXCEPTION] {1}", System.DateTime.Now, exception));
        }
    }

	public static void Exception (UnityEngine.Object context, System.Exception exception)
	{
		UnityEngine.Debug.LogException (exception, context);
        if (true)
        {
            Log(string.Format("{0:H:mm:ss} [EXCEPTION] {1} {2}", System.DateTime.Now, context, exception));
        }
    } 
	#endregion

	#region Error
	public static void ErrorFormat (UnityEngine.Object context, string template, params object[] args)
	{
		Error (string.Format (template, args), context);
       
	}

	public static void ErrorFormat (string template, params object[] args)
	{
        if (true)
        {
            Error(string.Format(template, args));
        }
	}

	public static void Error (params object[] message)
	{
        if ( true )
        {
            var temp = "";
            for (int i = 0; i < message.Length; i++)
            {
                temp += message[i];
            }
            UnityEngine.Debug.LogError(temp);
            Log ( string.Format ( "{0:H:mm:ss} [ERROR] {1}", System.DateTime.Now, temp) );
        }
	}

    public static void Error ( object message, UnityEngine.Object context )
        {
        UnityEngine.Debug.LogError ( message, context );
        if (true)
        {
            Log(string.Format("{0:H:mm:ss} [ERROR] {1} {2}", System.DateTime.Now, context, message));
        }
    } 
	#endregion

	#region Warning
	public static void WarningFormat (UnityEngine.Object context, string template, params object[] args)
	{
		Warning (string.Format (template, args),context);
	}

	public static void WarningFormat (string template, params object[] args)
	{
        if (true)
        {
            Warning(string.Format(template, args));
        }
	}

	public static void Warning (params object[] message)
	{
        if ( true )
        {
            var temp = "";
            for (int i = 0; i < message.Length; i++)
            {
                temp += message[i];
            }
            UnityEngine.Debug.LogWarning(temp);
            Log( string.Format ( "{0:H:mm:ss} [WARNING] {1}", System.DateTime.Now, temp) );
        }
	}

	public static void Warning (object message, UnityEngine.Object context)
	{
		UnityEngine.Debug.LogWarning (message, context);
        if (true)
        {
            Log(string.Format("{0:H:mm:ss} [WARNING] {1} {2}", System.DateTime.Now, context, message));
        }
    } 
	#endregion

	#region Message
	public static void MessageFormat (UnityEngine.Object context, string template, params object[] args)
	{
		Message (context, string.Format (template, args));
	}

	public static void MessageFormat (string template, params object[] args)
	{
        if (true)
        {
            Message(string.Format(template, args));
        }
	}

	public static void Message (params object[] message)
	{
        if ( true )
        {
            var temp = "";
            for (int i = 0; i < message.Length; i++)
            {
                temp += message[i];
            }

            UnityEngine.Debug.Log(temp);
            Log( string.Format ( "{0:H:mm:ss} [MESSAGE] {1}", System.DateTime.Now, temp) );
        }
	}

	public static void Message (UnityEngine.Object context, object message)
	{
        if ( true )
        {
            UnityEngine.Debug.Log(message, context);
            Log ( string.Format ( "{0:H:mm:ss} [MESSAGE] {1} {2}", System.DateTime.Now, context, message ) );
        }
	} 
	#endregion

	#region Verbose
	public static void VerboseFormat (UnityEngine.Object context, string template, params object[] args)
	{
		Verbose (context, string.Format (template, args));
	}

	public static void VerboseFormat (string template, params object[] args)
	{
		Verbose (string.Format (template, args));
	}

	public static void Verbose (object message)
	{
		UnityEngine.Debug.Log (string.Format ("<color=lime>{0} </color>", message));
	}

	public static void Verbose (UnityEngine.Object context, object message ) { 
		UnityEngine.Debug.Log (string.Format ("<color=lime>{0} </color>", message), context);
	}
	#endregion
}