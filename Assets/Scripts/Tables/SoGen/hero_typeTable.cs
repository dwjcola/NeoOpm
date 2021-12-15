using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace ProHA
{
    [Serializable]
    public class hero_typeRow
    {
         public int gid; // 英雄ID
         public string name; // 英雄名
         public int type; // 攻击类型1近战;2远程
         public int battleSpeed; // 战斗移动
         public int atkMaxDis; // 最大攻击范围
         public int scanDis; // 索敌范围
         public int defFirst; // 防御地块优先
         public int comSkill; // 普通攻击技能
         public int brithRow; // 出生排
         public int brithPriority; // 出生优先级
         public int Tokenid; // 获得的同名卡id
         public int Tokennum; // 获得的同名卡数量
         public int Transform; // 是否可使用通用同名卡
         public string skillstar; // 技能及解锁星级
         public string model; // 模型
         public string shadow; // 影子资源
         public string iconAtlas; // 头像图集
         public string iconSprite; // 头像贴图资源
         public float minsize; // 最小摄像机模型比例
         public float maxsize; // 最大摄像机模型比例
         public float inisize; // 默认摄像机模型比例
         public string spine; // 动画展示（暂时不用）
         public int fps; // 动画帧速帧/秒
         public string heroScene; // 英雄场景
         public string showact; // 展示动作
         public float showzoom; // 出征界面动作缩放
         public int quality; // 品质
         public string attackrange; // 攻击范围（显示用）

    }
    [CreateAssetMenu(menuName = "DataTable/hero_typeTable", fileName = "hero_typeTable")]
    public class hero_typeTable: DataTable
    {
        [SerializeField] 
        private hero_typeRow[] rows = default;
        private readonly Dictionary<int, hero_typeRow> _tables = new Dictionary<int, hero_typeRow>();
        public Dictionary<int, hero_typeRow> TableDic{
            get{
                return _tables;
            }
        }
        public hero_typeRow GetRow(int id)
        {
            if (_tables.TryGetValue(id, out hero_typeRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(hero_typeRow);
            }
        }

        public override void OnAfterDeserialize()
        {
            _tables.Clear();
            foreach (var v in rows)
            {
                _tables.Add(v.gid, v);
            }
        }
    }
}