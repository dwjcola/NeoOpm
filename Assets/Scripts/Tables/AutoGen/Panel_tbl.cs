
using System.IO;
using System;
using UnityEngine;
namespace ProHA{
    public class PanelLine :ITableLine<string> 
    {
		public string Id{get;private set;}
		public string AssetName{get;private set;}
		public string UIGroupName{get;private set;}
		public bool AllowMultiInstance{get;private set;}
		public bool PauseCoveredUIForm{get;private set;}
		public string LuaPath{get;private set;}
		public string LuaName{get;private set;}
		public bool Mask{get;private set;}
		public bool ShowHead{get;private set;}
		public bool ShowRes{get;private set;}
		public int UITween{get;private set;}

        public void Read(string line)
        {
            string[] columnStrings = line.Split('\t');
            int _idx=0;
            try{
			Id = columnStrings[_idx];_idx++;
			AssetName = columnStrings[_idx];_idx++;
			UIGroupName = columnStrings[_idx];_idx++;
			AllowMultiInstance = string.IsNullOrEmpty(columnStrings[_idx]) ? default:bool.Parse((columnStrings[_idx]));_idx++;
			PauseCoveredUIForm = string.IsNullOrEmpty(columnStrings[_idx]) ? default:bool.Parse((columnStrings[_idx]));_idx++;
			LuaPath = columnStrings[_idx];_idx++;
			LuaName = columnStrings[_idx];_idx++;
			Mask = string.IsNullOrEmpty(columnStrings[_idx]) ? default:bool.Parse((columnStrings[_idx]));_idx++;
			ShowHead = string.IsNullOrEmpty(columnStrings[_idx]) ? default:bool.Parse((columnStrings[_idx]));_idx++;
			ShowRes = string.IsNullOrEmpty(columnStrings[_idx]) ? default:bool.Parse((columnStrings[_idx]));_idx++;
			UITween = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;

            }
            catch(Exception e){
                Debug.LogError( $"Parse Error at Index:{_idx-1}. value:{columnStrings[_idx-1]} Line:{line}" );
                throw e;
            }

	    }
	     string  ITableLine< string >.Id() {
		    return Id;
	    }
    };
    public partial class Tables
    {
        public TableReader< string , PanelLine > Panel = new TableReader<string, PanelLine>("Panel.txt");
    }

}
    