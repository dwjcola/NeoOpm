//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace NeoOPM
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }
        public static DataPoolComponent DataPool
        {
            get;
            private set;
        }

        internal static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            DataPool = UnityGameFramework.Runtime.GameEntry.GetComponent<DataPoolComponent>();
        }

    }
    public class GameEntryCustom:MonoBehaviour
    {
        private void Start()
        {
            GameEntry.InitCustomComponents();
        }
    }
}
