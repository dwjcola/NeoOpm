using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ProHA
{
    public class TableTools : Singleton<TableTools>
    {
        private static Tables m_Tables;
        public static Tables Tables
        {
            get
            {
                if (Application.isPlaying)
                {
                   instance.InitTables();
                }
                else
                {
                    instance.EditorInit();///在editor并且没有运行时走这个读表，快一些
                }
                return m_Tables;
            }
        }
        public void EditorInit()
        {
            if (m_Tables == null)
            {
                m_Tables = new Tables();
                m_Tables.EditorLoad();
            }
        }
        public async Task InitTables()
        {
            if (m_Tables == null)
            {
                m_Tables = new Tables();
                await m_Tables.LoadAllTables();
            }
        }
        public void PreLoadTables(IList<TextAsset> texts)
        {
            if (texts == null)
            {
                return;
            }
            if (m_Tables == null)
            {
                m_Tables = new Tables();
            }
            for (int i = 0; i < texts.Count; i++)
            {
                m_Tables.LoadTable(texts[i]);
            }
        }
        public void PreLoadTable(TextAsset text)
        {
            if (text == null)
            {
                return;
            }
            if (m_Tables == null)
            {
                m_Tables = new Tables();
            }
            m_Tables.LoadTable(text);
        }
        public override void Dispose()
        {

        }
        public void ReInitTables()
        {
            Tables.ReLoadAllTables();
        }
    }

}
