
using System.IO;
using System;
using UnityEngine;
namespace ProHA{
    public class Hero_typeLine :ITableLine<int> 
    {
		public int gid{get;private set;}
		public string name{get;private set;}
		public int type{get;private set;}
		public int battleSpeed{get;private set;}
		public int atkMaxDis{get;private set;}
		public int scanDis{get;private set;}
		public int defFirst{get;private set;}
		public int comSkill{get;private set;}
		public int brithRow{get;private set;}
		public int brithPriority{get;private set;}
		public int Tokenid{get;private set;}
		public int Tokennum{get;private set;}
		public int Transform{get;private set;}
		public string skillstar{get;private set;}
		public int war{get;private set;}
		public int garrison{get;private set;}
		public int commander{get;private set;}
		public string model{get;private set;}
		public string shadow{get;private set;}
		public string iconAtlas{get;private set;}
		public string iconSprite{get;private set;}
		public float minsize{get;private set;}
		public float maxsize{get;private set;}
		public float inisize{get;private set;}
		public string spine{get;private set;}
		public int fps{get;private set;}
		public string heroScene{get;private set;}
		public string showact{get;private set;}
		public float showzoom{get;private set;}
		public int quality{get;private set;}
		public string attackrange{get;private set;}
		public float Yoffset{get;private set;}
		public string battleiconAtlas{get;private set;}
		public string battleiconSprite{get;private set;}

        public void Read(string line)
        {
            string[] columnStrings = line.Split('\t');
            int _idx=0;
            try{
			gid = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			name = columnStrings[_idx];_idx++;
			type = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			battleSpeed = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			atkMaxDis = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			scanDis = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			defFirst = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			comSkill = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			brithRow = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			brithPriority = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			Tokenid = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			Tokennum = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			Transform = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			skillstar = columnStrings[_idx];_idx++;
			war = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			garrison = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			commander = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			model = columnStrings[_idx];_idx++;
			shadow = columnStrings[_idx];_idx++;
			iconAtlas = columnStrings[_idx];_idx++;
			iconSprite = columnStrings[_idx];_idx++;
			minsize = string.IsNullOrEmpty(columnStrings[_idx]) ? default:float.Parse((columnStrings[_idx]));_idx++;
			maxsize = string.IsNullOrEmpty(columnStrings[_idx]) ? default:float.Parse((columnStrings[_idx]));_idx++;
			inisize = string.IsNullOrEmpty(columnStrings[_idx]) ? default:float.Parse((columnStrings[_idx]));_idx++;
			spine = columnStrings[_idx];_idx++;
			fps = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			heroScene = columnStrings[_idx];_idx++;
			showact = columnStrings[_idx];_idx++;
			showzoom = string.IsNullOrEmpty(columnStrings[_idx]) ? default:float.Parse((columnStrings[_idx]));_idx++;
			quality = string.IsNullOrEmpty(columnStrings[_idx]) ? default:int.Parse((columnStrings[_idx]));_idx++;
			attackrange = columnStrings[_idx];_idx++;
			Yoffset = string.IsNullOrEmpty(columnStrings[_idx]) ? default:float.Parse((columnStrings[_idx]));_idx++;
			battleiconAtlas = columnStrings[_idx];_idx++;
			battleiconSprite = columnStrings[_idx];_idx++;

            }
            catch(Exception e){
                Debug.LogError( $"Parse Error at Index:{_idx-1}. value:{columnStrings[_idx-1]} Line:{line}" );
                throw e;
            }

	    }
	     int  ITableLine< int >.Id() {
		    return gid;
	    }
    };
    public partial class Tables
    {
        public TableReader< int , Hero_typeLine > Hero_type = new TableReader<int, Hero_typeLine>("Hero_type.txt");
    }

}
    