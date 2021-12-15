using System.Collections;
using System.Collections.Generic;
using GameFramework;

namespace ProHA
{
    public class TableDic
    {
        public static string GetDicValue(string key,params object[] args)
        {
            DicLine dic = TableTools.Tables.Dic.GetLineById(key);
            if (dic == null)
            {
                return key;
            }
            string content = dic.Content;
            if (string.IsNullOrEmpty(content))
            {
                content = key;
            }
            else
            {
                content = Utility.Text.Format(content,args);
            }
            return content;
        }

        public static string GetDicFormateValue(string key,IList args)
        {
            DicLine dic = TableTools.Tables.Dic.GetLineById(key);
            if (dic == null)
            {
                return key;
            }
            string content = dic.Content;
            if (string.IsNullOrEmpty(content))
            {
                content = key;
            }
            else
            {
                FormateText(ref content,args);
            }
            return content;
        }
        public static void FormateText(ref string content,IList args )
        {
            if (string.IsNullOrEmpty(content)) return;
            object[] parms = new object[args.Count];
            string temp;
            for (int i = 0; i < args.Count; i++)
            {
                temp = args[i] as string;
                if (temp!=null)
                {
                    char type = temp[0];
                    switch (type)
                    {
                        case 'c':
                            parms[i] = "cityName";
                            break;
                        case 'b':
                            parms[i] = "playerName";
                            break;
                        default:
                            parms[i] = temp;
                            break;
                    }
                }
            }

            content = Utility.Text.Format(content, parms);
        }

        /*public static string GetO(string content,IList list)
        {
            object[] parms = new object[list.Count];
            string temp;
            for (int i = 0; i < list.Count; i++)
            {
                parms[i] = list[i];
            }

            return Utility.Text.Format(content, parms);
        }*/
    }
}