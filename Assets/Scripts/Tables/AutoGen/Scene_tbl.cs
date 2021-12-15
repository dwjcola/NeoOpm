
using System.IO;
using System;
using UnityEngine;
namespace ProHA{
    public class SceneLine :ITableLine<int> 
    {
		public int Id{get;private set;}
		public string AssetName{get;private set;}
		public int BackgroundMusicId{get;private set;}

        public void Read(string line)
        {
            string[] columnStrings = line.Split('\t');
            int _idx=0;
            try{
			Id = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			AssetName = columnStrings[_idx];_idx++;
			BackgroundMusicId = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;

            }
            catch(Exception e){
                Debug.LogError( $"Parse Error at Index:{_idx-1}. value:{columnStrings[_idx-1]} Line:{line}" );
                throw e;
            }

	    }
	     int  ITableLine< int >.Id() {
		    return Id;
	    }
    };
    public partial class Tables
    {
        public TableReader< int , SceneLine > Scene = new TableReader<int, SceneLine>("Scene.txt");
    }

}
    