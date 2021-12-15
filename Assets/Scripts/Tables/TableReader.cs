using GameFramework;
using GameFramework.AddressableResource;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ProHA
{
    public interface ITableLine<ID_t>
    {
        void Read(string line);
        ID_t Id();
    }

    public interface ILoad
    {
        void Load(byte[] bytes=null);
        void Reload();
        Task AsyncLoad(); 
    }

    public class TableReader<ID_t, Row_t> : ILoad where Row_t : ITableLine<ID_t>, new()
    {

        Dictionary<ID_t, Row_t> _datas = new Dictionary<ID_t, Row_t>();

        public string fname;
        public TableReader(string fname)
        {
            this.fname = fname;
            Tables.Register(this);

            //Load(fname);
        }
        public void Load(byte[] bytes=null)
        {
            //to do -- 资源管理
            StreamReader sr =bytes==null? new StreamReader(Path.Combine(Tables.TableDir, fname)):new StreamReader(new MemoryStream(bytes));
            string line;
            int count = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (++count <= Tables.HeadCount)
                {
                    continue;
                }
                Row_t r = new Row_t();
                r.Read(line);
                ID_t key = r.Id();
                if (_datas.ContainsKey(key))
                {
                    _datas.Remove(key);
                }
                _datas.Add(key, r);
            }
            sr.Dispose();
            sr.Close();
        }
        public Row_t GetLineById(ID_t key)
        {
            Row_t line;
            if (_datas.TryGetValue(key, out line))
            {
                return line;
            }
            return default(Row_t);
        }

        public Dictionary<ID_t, Row_t>.KeyCollection Keys()
        {
            return _datas.Keys;
        }

        public Dictionary<ID_t, Row_t>.ValueCollection Vaules()
        {
            return _datas.Values;
        }

        public void Reload()
        {
            _datas.Clear();
            Load();
        }

        public async Task AsyncLoad()
        {
            var resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
            var file = await resMgr.LoadAssetAsync<TextAsset>(Path.Combine(string.Format(Constant.FileName.DataTableRoot, "Assets"), fname)).Task;
            if (file != null)
            {
                StreamReader sr = new StreamReader(new MemoryStream(file.bytes));
                string line;
                int count = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (++count <= Tables.HeadCount)
                    {
                        continue;
                    }
                    Row_t r = new Row_t();
                    r.Read(line);
                    _datas.Add(r.Id(), r);
                }
                sr.Dispose();
                sr.Close();
            }
            resMgr.UnloadAsset(file);
        }
    }




    public partial class Tables
    {
        public const int HeadCount = 3;
        public static string TableDir = string.Format(Constant.FileName.DataTableRoot, UnityEngine.Application.dataPath);

       // public static List<ILoad> tableLoads = new List<ILoad>();
        public static Dictionary<string, ILoad> loadDic = new Dictionary<string, ILoad>();
        public static void Register<ID_t, Row_t>(TableReader<ID_t, Row_t> tableReader) where Row_t : ITableLine<ID_t>, new()
        {
            //tableLoads.Add(tableReader);".txt"
            loadDic.Add(tableReader.fname.Remove(tableReader.fname.Length-4), tableReader);
        }
#if UNITY_EDITOR && false
        public Tables()
        {
            //tblWar.Load();
            foreach (var t in loadDic)
            {
                t.Value.Load();
            }
        }
#else
        public Tables()
        {

        }
        public void EditorLoad()
        {
            foreach (var t in loadDic)
            {
                t.Value.Load();
            }
        }
       public async Task AddresableLoad()
        {
            foreach (var t in loadDic)
            {
                await t.Value.AsyncLoad();
            }
        }
        public async Task LoadAllTables()
        {
            var resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
          var list=await resMgr.LoadAssetsAsync<TextAsset>("Tables", (text) => {
              var filename = text.name;
              if (loadDic.ContainsKey(filename))
              {
                  loadDic[filename].Load(text.bytes);
              }
          }, true);
        }
        /// <summary>
        /// 在checkversion 预加载里调用了
        /// </summary>
        /// <param name="text"></param>
        public void LoadTable(TextAsset text)
        {
            if (text != null)
            {
                var filename = text.name;
                if (loadDic.ContainsKey(filename))
                {
                    loadDic[filename].Load(text.bytes);
                }
            }
        }
#endif
        public static void ReLoadAllTables()
        {
            foreach (var t in loadDic)
            {
                t.Value.Reload();
            }
        }
  
    }


}