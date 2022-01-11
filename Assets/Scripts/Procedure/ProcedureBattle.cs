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

namespace NeoOPM
{
    public class ProcedureBattle : ProcedureBase
    {
        private bool m_GoToWorld = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GoToWorld()
        {
            m_GoToWorld = true;
        }
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
          
            m_GoToWorld = false;

           // LC.CallLuaFunc("BattleScene", "enter");

        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_GoToWorld)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, Constant.Scene.World);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
    }
}
