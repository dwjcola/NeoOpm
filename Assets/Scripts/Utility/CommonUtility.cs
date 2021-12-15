using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System;

namespace ProHA
{
    public static class CommonUtility
    {
    
        public static Vector3 getMousePosition(float cPlane)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cPlane));
            Debug.LogError(cPlane);
            Debug.LogError(Input.mousePosition);
            worldPosition.y = -1;
            return worldPosition;
        }

        public static void DestroyChildren(Transform trans)
        {
            int count = trans.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                var itemTemp = trans.GetChild(i);
                itemTemp.transform.parent = null;
                GameObject.DestroyImmediate(itemTemp.gameObject);
            }
        }

        public static string[] GetSplitStr(string _str, string _symbol)
        {
            char[] tempChar = _symbol.ToCharArray();
            char tempSymbol = tempChar[0];
            string[] tempStr = _str.Split(tempSymbol);
            return tempStr;
        }

        //string转换为float Arr
        //eg 1,1,1 
        public static float[] GetSplitStrFloat(string _str, string _symbol)
        {
            char[] tempChar = _symbol.ToCharArray();
            char tempSymbol = tempChar[0];
            string[] tempStr = _str.Split(tempSymbol);
            float[] tempfloat = Array.ConvertAll(tempStr, s => float.Parse(s));

            return tempfloat;
        }

        //string转换为Vector3
        //eg 1,1,1 
        public static Vector3 GetSplitStrVector3(string _str, string _symbol)
        {
            char[] tempChar = _symbol.ToCharArray();
            char tempSymbol = tempChar[0];
            string[] tempStr = _str.Split(tempSymbol);

            if (tempStr.Length == 3)
            {
                float[] tempfloat = Array.ConvertAll(tempStr, s => float.Parse(s));
                return new Vector3(tempfloat[0], tempfloat[1], tempfloat[2]);
            }
            else
            {
                return Vector3.zero;
            }
            
        }

        /// <summary>
        /// 通过文件路径 读取内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFromFile(string fileName)
        {
            FileStream fstream = new FileStream(fileName, FileMode.OpenOrCreate);
            fstream.Position = 0;
            StringBuilder logBuilder = new StringBuilder();
            if (fstream != null)
            {
                int num = 0;
                byte[] buffer = new byte[1024];
                while ((num = fstream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    logBuilder.Append(Encoding.UTF8.GetString(buffer, 0, num));//使用utf-8编码格式
                }//返回本次实际读取到的有效字节数

                fstream.Close();
                return logBuilder.ToString();
            }
            return "";
        }


        public static bool CheckGuiRaycastObjects()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == 5)
                {
                    return true;
                }
            }
            return false;
        }
        

        public static void SetParent(Transform trans, Transform parent)
        {
            if (trans != null && parent != null && trans.parent != parent)
            {
                trans.SetParent(parent);
                ResetTransform(trans);
            }
        }
        public static void ResetTransform(Transform trans)
        {
            if (trans == null)
                return;
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }

        public static void SetGray(Transform form)
        {
            Image sp = form.GetComponent<Image>();
            if (sp != null)
            {
                sp.color = Color.gray;
            }

            if (form.childCount > 0)
            {
                for (int i = 0; i < form.childCount; i++)
                {
                    SetGray(form.GetChild(i));
                }

            }
        }

        public static void SetWhite(Transform form)
        {
            Image sp = form.GetComponent<Image>();
            if (sp != null)
            {
                sp.color = Color.white;
            }

            if (form.childCount > 0)
            {
                for (int i = 0; i < form.childCount; i++)
                {
                    SetWhite(form.GetChild(i));
                }

            }
        }

        /// <summary>
        /// 世界坐标转换UI坐标 
        /// </summary>
        /// <param name="worldPosition">世界坐标</param>
        /// <param name="m_ParentCanvas">UI父节点</param>
        /// <param name="Target">目标UI节点</param>
        public static void WorldPos2ScreenPos(Vector3 worldPosition,RectTransform m_ParentCanvas, Transform Target) 
        {
            Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ParentCanvas, screenPosition,null, out position))
            {
                Target.localPosition = position;
            }
        }


        //持有一个缓存 关闭界面时候清除
        private static int CaptureIndex = -1;
        private static Texture2D Texture2DCache = null;
        /// <summary>
        /// 截取主相机截图
        /// </summary>
        /// <param name="DirName"></param>
        /// <param name="fileName"></param>
        public static void CaptureCamera(string DirName, string fileName,int index) 
        {
            var dir = System.IO.Path.Combine(Application.persistentDataPath, DirName);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            var filePath = System.IO.Path.Combine(dir, fileName);

            RenderTexture renTex = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4);
            Camera.main.targetTexture = renTex;
            Camera.main.Render();
            RenderTexture.active = renTex;

            Texture2D tex = new Texture2D(Screen.width / 4, Screen.height / 4);
            tex.ReadPixels(new Rect(0, 0, Screen.width / 4, Screen.height / 4), 0, 0);
            tex.Apply();

            CaptureIndex = index;
            if (Texture2DCache != null)
            {
                GameObject.DestroyImmediate(Texture2DCache);
                Texture2DCache = tex;
            }

            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renTex);
            System.IO.File.WriteAllBytes(filePath, tex.EncodeToPNG());
        }

        /// <summary>
        /// 通过文件名载入图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string fileName,int index) 
        {
            if (index == CaptureIndex && Texture2DCache != null)
            {
                return Texture2DCache;
            }
            var filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(filePath))
            {
                return null;
            }
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var texNew = new Texture2D(Screen.width / 4, Screen.height / 4, TextureFormat.ASTC_6x6,false);
            texNew.LoadImage(bytes);
            return texNew;
        }

        public static void SaveTexture(string DirName, string fileName, Texture2D Texture,int index)
        {
            var dir = System.IO.Path.Combine(Application.persistentDataPath, DirName);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            var filePath = System.IO.Path.Combine(dir, fileName);

            CaptureIndex = index;
            if (Texture2DCache != null)
            {
                GameObject.DestroyImmediate(Texture2DCache);
                Texture2DCache = Texture;
            }

            System.IO.File.WriteAllBytes(filePath, Texture.EncodeToPNG());
        }

        public static void ClearTexture2DCache() 
        {
            CaptureIndex = -1;
            if (Texture2DCache != null)
            {
                GameObject.DestroyImmediate(Texture2DCache);
                Texture2DCache = null;
            }
        }
    }
    
}
