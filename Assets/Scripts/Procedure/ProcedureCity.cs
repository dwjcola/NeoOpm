//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ProHA
{
    public class ProcedureCity : ProcedureBase
    {
        private bool m_GoToBattle = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GoToBattle()
        {
            m_GoToBattle = true;
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UI.OpenUI("MainUI", this);
            /*GameEntry.HeroScene.Open();
            m_GoToWorld = false;
            List<int> heros = new List<int>();
            heros.Add(201);
            heros.Add(202);
            heros.Add(203);
            heros.Add(204);
            heros.Add(205);
            
            HeroSceneMgr.instance.InitHeroScene(heros);*/
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_GoToBattle)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, Constant.Scene.Battle);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            
        }
    }
}
