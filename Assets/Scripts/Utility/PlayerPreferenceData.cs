//集中管理客户端玩家持久化数据，使用Unity的PlayerPrefs类，在windows上保存在注册表中，在设备上保存为.plist文件

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NeoOPM
{
	public static class PlayerPreferenceData
	{
		//上次登录acc
		private const string AccName = "SLG_ACCID";
		public static string LastAccName
		{

			set
			{
				PlayerPrefs.SetString(AccName, value);
				PlayerPrefs.Save();
			}

			get
			{
				return PlayerPrefs.GetString(AccName, null);
			}
		}

		public static void SetIntArray(string key, List<int> intArray)  
		{  
			if (intArray.Count == 0) return;  
		
			System.Text.StringBuilder sb = new System.Text.StringBuilder();  
			for (int i = 0; i < intArray.Count-1; ++i)
			{
				sb.Append(intArray[i]).Append("|");  
			}
			sb.Append(intArray[intArray.Count - 1]);  

			PlayerPrefs.SetString(key, sb.ToString());   
		}  

		public static List<int> GetIntArray(string key)  
		{  
			if (PlayerPrefs.HasKey(key))  
			{  
				string[] stringArray = PlayerPrefs.GetString(key).Split("|"[0]);  
				List<int> intArray = new List<int>();  
				for (int i = 0; i < stringArray.Length; i++) 
				{
					intArray.Add(Convert.ToInt32(stringArray[i]));  
				}
				return intArray;  
			}
			return null;
		} 
		public static void SetInt(string key,int value )
		{
			PlayerPrefs.SetInt ( key, value );
			PlayerPrefs.Save ( );
		}
		public static int GetInt(string key )
		{
			return PlayerPrefs.GetInt ( key, -1 );
		}

		public static void SetString ( string key, string value )
		{
			PlayerPrefs.SetString ( key, value );
			PlayerPrefs.Save ( );
		}
		public static string GetString ( string key )
		{
			return PlayerPrefs.GetString ( key, "" );
		}

		public static void SetFloat ( string key, float value )
		{
			PlayerPrefs.SetFloat ( key, value );
			PlayerPrefs.Save ( );
		}
		public static float GetFloat ( string key )
		{
			return PlayerPrefs.GetFloat ( key, -1 );
		}
    }
}