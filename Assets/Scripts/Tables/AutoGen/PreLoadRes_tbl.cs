
using System.IO;
using System;
using UnityEngine;
namespace ProHA{
    public class PreLoadResLine :ITableLine<string> 
    {
		public string Res{get;private set;}
		public string fileType{get;private set;}
		public string AssemblyName{get;private set;}
		public string Txt{get;private set;}
		public int Scene{get;private set;}
		public int Self{get;private set;}

        public void Read(string line)
        {
            string[] columnStrings = line.Split('\t');
            int _idx=0;
            try{
			Res = columnStrings[_idx];_idx++;
			fileType = columnStrings[_idx];_idx++;
			AssemblyName = columnStrings[_idx];_idx++;
			Txt = columnStrings[_idx];_idx++;
			Scene = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			Self = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;

            }
            catch(Exception e){
                Debug.LogError( $"Parse Error at Index:{_idx-1}. value:{columnStrings[_idx-1]} Line:{line}" );
                throw e;
            }

	    }
	     string  ITableLine< string >.Id() {
		    return Res;
	    }
    };
    public partial class Tables
    {
        public TableReader< string , PreLoadResLine > PreLoadRes = new TableReader<string, PreLoadResLine>("PreLoadRes.txt");
    }

}
    