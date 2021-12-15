
using System.IO;
using System;
using UnityEngine;
namespace ProHA{
    public class DicLine :ITableLine<string> 
    {
		public string Id{get;private set;}
		public string Content{get;private set;}

        public void Read(string line)
        {
            string[] columnStrings = line.Split('\t');
            int _idx=0;
            try{
			Id = columnStrings[_idx];_idx++;
			Content = columnStrings[_idx];_idx++;

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
        public TableReader< string , DicLine > Dic = new TableReader<string, DicLine>("Dic.txt");
    }

}
    