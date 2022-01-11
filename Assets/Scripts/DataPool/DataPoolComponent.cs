///
///用来读取和存放LUA端的客户端缓存数据
/// 

using System;
using System.Collections.Generic;
using SLG;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace NeoOPM
{
    public class DataPoolComponent : GameFrameworkComponent
    {
        [CSharpCallLua]
        public interface IBagInfoData
        {
            void PushItem(int itemId,int Count);
        }
        [CSharpCallLua]
        public interface IPlayerData
        {
            ulong RoleId { get; set; }
            uint ServerId{ get; set; }
            string AccId{ get; set; }
            string RoleName{ get; set; }
            uint Level{ get; set; }
            int Food{ get; set; }
            int Stone{ get; set; }
            int Iron{ get; set; }
            int Crystal{ get; set; }
            int Gold{ get; set; }
            int ActionToken{ get; set; }
            int KillMonsterLevel { get; set; }
            bool TroopAfterStop { get; set; }
            IBagInfoData BagInfoData { get; }
        }
        private IPlayerData m_PlayerData;
        public IPlayerData PlayerData
        {
            get
            {
                if (m_PlayerData == null)
                {
                    LuaEnv luaEnv = XluaManager.instance.LuaEnv;
                    m_PlayerData = luaEnv.Global.GetInPath<IPlayerData>("PlayerData");
                }
                return m_PlayerData;
            }
        }

        [CSharpCallLua]
        public interface IPlayerSceneData
        {
            WsTroopBasicInfo GetSelfTroopDataById(ulong entityId);
            WsObjInfo GetCastleInfo();
            bool? IsSelfTroopInBattle(ulong entityId);
            bool? IsSelfCityInBattle();
            ulong GetCastleEntityId();
        }
        private IPlayerSceneData m_PlayerSceneData;
        public IPlayerSceneData PlayerSceneData
        {
            get
            {
                if (m_PlayerSceneData == null)
                {
                    LuaEnv luaEnv = XluaManager.instance.LuaEnv;
                    m_PlayerSceneData = luaEnv.Global.GetInPath<IPlayerSceneData>("PlayerSceneData");
                }
                return m_PlayerSceneData;
            }
        }

        [CSharpCallLua]
        public interface IBuildingPlaneData
        {
            void ChangeOneBlockPos(int posx, int posy , int id);
        }

            //临时使用
        private IBuildingPlaneData m_BuildingPlaneData;
        public IBuildingPlaneData BuildingPlaneData
        {
            get
            {
                if (m_BuildingPlaneData == null)
                {
                    LuaEnv luaEnv = XluaManager.instance.LuaEnv;
                    m_BuildingPlaneData = luaEnv.Global.GetInPath<IBuildingPlaneData>("BuildingPlaneData");
                }
                return m_BuildingPlaneData;
            }
        }
        //临时使用end
    }
}