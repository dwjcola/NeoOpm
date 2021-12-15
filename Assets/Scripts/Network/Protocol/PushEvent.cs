using Pomelo.DotNetClient;
using SLG;
//===== @Head start============================================================
//===== @Head end==============================================================
namespace Rpc {
    public static class PushEvent {
        public
        const uint OnChat = 0;
        public
        const uint OnPlayerViewChg = 1;
        public
        const uint OnWsObjUpdate = 2;
        public
        const uint OnTroopStateChg = 3;
        public
        const uint OnBattleStart = 4;
        public
        const uint OnQueueFinish = 5;
        public
        const uint OnRecruitInfoChange = 6;
        public
        const uint OnRecruitFinish = 7;
        public
        const uint OnMoneyChange = 8;
        public
        const uint OnTradeLevelChange = 9;
        public
        const uint OnArrivalatTradePort = 10;
        public
        const uint OnCombatEffectChange = 11;
        public
        const uint OnBuildingInfoChange = 12;
        public
        const uint OnTechInfoChange = 13;
        public
        const uint OnHeroUpdate = 14;
        public
        const uint OnSoldierUpdate = 15;
        public
        const uint OnItemUpdate = 16;
        public
        const uint OnApplyFriend = 17;
        public
        const uint OnUnionUpdate = 18;
        public
        const uint OnUnionLose = 19;
        public
        const uint OnInviteJoinUnion = 20;
        public
        const uint OnUnionJobUpdate = 21;
        public
        const uint OnUnionTitleUpdate = 22;
        public
        const uint OnGroupCreate = 23;
        public
        const uint OnTransferGateInvalid = 24;
        public
        const uint taskInfoUpdate = 25;
        public
        const uint OnWsCastleUpdate = 26;
        public
        const uint OnNewMail = 27;
        public
        const uint OnAddCommodity = 28;
        public
        const uint OnViewBuildingsInfo = 29;
        public
        const uint OnWsPath = 30;
        public
        const uint OnHeroLevelInfoChange = 31;
        public
        const uint OnBuildReachVolumeLimit = 32;
        public
        const uint OnBattleAttrIncFromBuildChange = 33;
        public
        const uint OnBarriersProgressChange = 34;
        public
        const uint OnExpeditionBattleEnd = 35;
        public
        const uint OnPhaseBattleResult = 36;
        public
        const uint OnVerifyKeyFrameRet = 37;
        public
        const uint OnReqFrameDataOnlyRet = 38;
        public
        const uint OnBoardcastFrames = 39;
        public
        const uint OnActiveSkillsChange = 40;
        public
        const uint OnTurretBagChange = 41;
        public
        const uint OnBuildingSceneChange = 42;
        public
        const uint OnKillMonsterLevelChange = 43;

        public static void Init(PomeloClient client) {

            #region OnChat
            client.on < SingleChatInfo > (OnChat, (args) => {
                //===== OnChat start============================================================
                LC.SendEvent("EVENT_ONCHAT", args);
                //===== OnChat end==============================================================
            });
            #endregion



     

            #region OnTroopStateChg
            client.on < WsTroopBasicInfo > (OnTroopStateChg, (args) => {
                //===== OnTroopStateChg start============================================================
                Logger.Message("===OnTroopStateChg===", args.EntityId, ",state:", args.State);
                //if(args.State==3)
                //{
                //    WsObjId Id = new WsObjId();
                //    Id.EntityId = 2;
                //    Rpc.wbScene_Handler.GetResInfo(Id);
                //}
                //===== OnTroopStateChg end==============================================================
            });
            #endregion



            #region OnQueueFinish
            client.on < QueueInfo > (OnQueueFinish, (args) => {
                //===== OnQueueFinish start============================================================
                //===== OnQueueFinish end==============================================================
            });
            #endregion

            #region OnRecruitInfoChange
            client.on < RecruitInfo > (OnRecruitInfoChange, (args) => {
                //===== OnRecruitInfoChange start============================================================
                //===== OnRecruitInfoChange end==============================================================
            });
            #endregion

            #region OnRecruitFinish
            client.on < RecruitRes > (OnRecruitFinish, (args) => {
                //===== OnRecruitFinish start============================================================
                //===== OnRecruitFinish end==============================================================
            });
            #endregion

            #region OnMoneyChange
            client.on < ResourceValue > (OnMoneyChange, (args) => {
                //===== OnMoneyChange start============================================================
                //===== OnMoneyChange end==============================================================
            });
            #endregion

            #region OnTradeLevelChange
            client.on < SimpleMsg > (OnTradeLevelChange, (args) => {
                //===== OnTradeLevelChange start============================================================
                //===== OnTradeLevelChange end==============================================================
            });
            #endregion

            #region OnArrivalatTradePort
            client.on < SimpleMsg > (OnArrivalatTradePort, (args) => {
                //===== OnArrivalatTradePort start============================================================
                //===== OnArrivalatTradePort end==============================================================
            });
            #endregion

            #region OnCombatEffectChange
            client.on < PlayerCombatEffectInfo > (OnCombatEffectChange, (args) => {
                //===== OnCombatEffectChange start============================================================
                //===== OnCombatEffectChange end==============================================================
            });
            #endregion

            #region OnBuildingInfoChange
            client.on < BuildingInfo > (OnBuildingInfoChange, (args) => {
                //===== OnBuildingInfoChange start============================================================
                //===== OnBuildingInfoChange end==============================================================
            });
            #endregion

            #region OnTechInfoChange
            client.on < TechnologyInfo > (OnTechInfoChange, (args) => {
                //===== OnTechInfoChange start============================================================
                //===== OnTechInfoChange end==============================================================
            });
            #endregion

            #region OnHeroUpdate
            client.on < PlayerHeroInfo > (OnHeroUpdate, (args) => {
                //===== OnHeroUpdate start============================================================
                //===== OnHeroUpdate end==============================================================
            });
            #endregion

            #region OnSoldierUpdate
            client.on < PlayerSoldierInfo > (OnSoldierUpdate, (args) => {
                //===== OnSoldierUpdate start============================================================
                //===== OnSoldierUpdate end==============================================================
            });
            #endregion

            #region OnItemUpdate
            client.on < ItemInfo > (OnItemUpdate, (args) => {
                //===== OnItemUpdate start============================================================
                //===== OnItemUpdate end==============================================================
            });
            #endregion

            #region OnApplyFriend
            client.on < Friend > (OnApplyFriend, (args) => {
                //===== OnApplyFriend start============================================================
                //===== OnApplyFriend end==============================================================
            });
            #endregion

            #region OnUnionUpdate
            client.on < UnionInfo > (OnUnionUpdate, (args) => {
                //===== OnUnionUpdate start============================================================
                //===== OnUnionUpdate end==============================================================
            });
            #endregion

            #region OnUnionLose
            client.on(OnUnionLose, (buffer, offset) => {
                int ret = MsgProtocol.ReadInt32BE(buffer, offset);
                //===== OnUnionLose start============================================================
                //===== OnUnionLose end==============================================================
            });
            #endregion

            #region OnInviteJoinUnion
            client.on < UnionInfo > (OnInviteJoinUnion, (args) => {
                //===== OnInviteJoinUnion start============================================================
                //===== OnInviteJoinUnion end==============================================================
            });
            #endregion

            #region OnUnionJobUpdate
            client.on(OnUnionJobUpdate, (buffer, offset) => {
                int ret = MsgProtocol.ReadInt32BE(buffer, offset);
                //===== OnUnionJobUpdate start============================================================
                //===== OnUnionJobUpdate end==============================================================
            });
            #endregion

            #region OnUnionTitleUpdate
            client.on(OnUnionTitleUpdate, (buffer, offset) => {
                int ret = MsgProtocol.ReadInt32BE(buffer, offset);
                //===== OnUnionTitleUpdate start============================================================
                //===== OnUnionTitleUpdate end==============================================================
            });
            #endregion

            #region OnGroupCreate
            client.on < SingleChannelData > (OnGroupCreate, (args) => {
                //===== OnGroupCreate start============================================================
                LC.SendEvent("EVENT_CREATE_CHANNEL", args);
                //===== OnGroupCreate end==============================================================
            });
            #endregion

            #region OnTransferGateInvalid
            client.on < WsObjId > (OnTransferGateInvalid, (args) => {
                //===== OnTransferGateInvalid start============================================================
                //===== OnTransferGateInvalid end==============================================================
            });
            #endregion

            #region taskInfoUpdate
            client.on < SingleTaskInfo > (taskInfoUpdate, (args) => {
                //===== taskInfoUpdate start============================================================
                //===== taskInfoUpdate end==============================================================
            });
            #endregion

            #region OnWsCastleUpdate
            client.on < WsObjInfo > (OnWsCastleUpdate, (args) => {
                //===== OnWsCastleUpdate start============================================================

                //===== OnWsCastleUpdate end==============================================================
            });
            #endregion

            #region OnNewMail
            client.on < NotifyNewMail > (OnNewMail, (args) => {
                //===== OnNewMail start============================================================
                //===== OnNewMail end==============================================================
            });
            #endregion

            #region OnAddCommodity
            client.on < CommodityList > (OnAddCommodity, (args) => {
                //===== OnAddCommodity start============================================================
                //===== OnAddCommodity end==============================================================
            });
            #endregion

  

            #region OnHeroLevelInfoChange
            client.on < PlayerHeroesLevel > (OnHeroLevelInfoChange, (args) => {
                //===== OnHeroLevelInfoChange start============================================================
                //===== OnHeroLevelInfoChange end==============================================================
            });
            #endregion

            #region OnBuildReachVolumeLimit
            client.on(OnBuildReachVolumeLimit, (buffer, offset) => {
                int ret = MsgProtocol.ReadInt32BE(buffer, offset);
                //===== OnBuildReachVolumeLimit start============================================================
                //===== OnBuildReachVolumeLimit end==============================================================
            });
            #endregion

            #region OnBattleAttrIncFromBuildChange
            client.on < PlayerAttrsInc > (OnBattleAttrIncFromBuildChange, (args) => {
                //===== OnBattleAttrIncFromBuildChange start============================================================
                //===== OnBattleAttrIncFromBuildChange end==============================================================
            });
            #endregion

            #region OnBarriersProgressChange
            client.on < PlayerAllBarriers > (OnBarriersProgressChange, (args) => {
                //===== OnBarriersProgressChange start============================================================
                //===== OnBarriersProgressChange end==============================================================
            });
            #endregion

            #region OnExpeditionBattleEnd
            client.on < ExpeditionBattleEnd > (OnExpeditionBattleEnd, (args) => {
                //===== OnExpeditionBattleEnd start============================================================
                //===== OnExpeditionBattleEnd end==============================================================
            });
            #endregion

            #region OnPhaseBattleResult
            client.on < PhaseBattleResult > (OnPhaseBattleResult, (args) => {
                //===== OnPhaseBattleResult start============================================================
                //===== OnPhaseBattleResult end==============================================================
            });
            #endregion

   

            #region OnActiveSkillsChange
            client.on < ConfigTroop > (OnActiveSkillsChange, (args) => {
                //===== OnActiveSkillsChange start============================================================
                //===== OnActiveSkillsChange end==============================================================
            });
            #endregion

  

            #region OnKillMonsterLevelChange
            client.on < SimpleMsg > (OnKillMonsterLevelChange, (args) => {
                //===== OnKillMonsterLevelChange start============================================================
                //===== OnKillMonsterLevelChange end==============================================================
            });
            #endregion

        }

    }
}