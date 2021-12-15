
using System.IO;
using System;
using UnityEngine;
namespace ProHA{
    public class AdressablesConfigProfileLine :ITableLine<string> 
    {
		public string ProfileName{get;private set;}
		public string RemoteBuildPath{get;private set;}
		public string RemoteLoadPath{get;private set;}

        public void Read(string line)
        {
            string[] columnStrings = line.Split('\t');
            int _idx=0;
            try{
			ProfileName = columnStrings[_idx];_idx++;
			RemoteBuildPath = columnStrings[_idx];_idx++;
			RemoteLoadPath = columnStrings[_idx];_idx++;

            }
            catch(Exception e){
                Debug.LogError( $"Parse Error at Index:{_idx-1}. value:{columnStrings[_idx-1]} Line:{line}" );
                throw e;
            }

	    }
	     string  ITableLine< string >.Id() {
		    return ProfileName;
	    }
    };
    public partial class Tables
    {
        public TableReader< string , AdressablesConfigProfileLine > AdressablesConfigProfile = new TableReader<string, AdressablesConfigProfileLine>("AdressablesConfigProfile.txt");
    }

}
    