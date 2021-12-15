using LitJson;
using Pomelo.DotNetClient;
using SLG;
using System.Threading.Tasks;
namespace Rpc {

    public static class wbBuilding_Handler {
        enum ServiceId {
            UpdateAllLocation = 0,
                UpdateLocation = 1,
                UpdateBlocksLocation = 2,
                UpdateLocationForTest = 3,
                UpdateBlocksLocationForTest = 4,
                GetBuildingsByType = 5,
                GetAllBuildings = 6,
                DoResearch = 7,
                PubRecruit = 8,
                SetNotShowFlag = 9,
                TrainSoldier = 10,
                BuildingLevelUp = 11,
                treatWounded = 12,
                setActiveMainSkills = 13,
                StartAndFinishQueue = 14,
                FinishStartedQueue = 15,
                AccelerateQueue = 16,
                CancelQueue = 17,
                PlayerGetProduct = 18,
                PlayerGetProductByType = 19,
                GetBuildingAttr = 20,
                RecoveryTurret = 21,
                ComposeTurret = 22,
                ComposeManyTurret = 23,
                GetInitEditBuildingScene = 24,
                BuildingSceneSave = 25,
                ChangeNewScene = 26,

        }


        public static Task < ReturnMsg > UpdateAllLocation(BuildingsInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UpdateAllLocation, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > UpdateLocation(BlockInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UpdateLocation, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > UpdateBlocksLocation(BuildingsInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UpdateBlocksLocation, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > UpdateLocationForTest(BlockInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UpdateLocationForTest, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > UpdateBlocksLocationForTest(BuildingsInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UpdateBlocksLocationForTest, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < BuildingsInfo > GetBuildingsByType(SimpleMsg arg0) {
            TaskCompletionSource < BuildingsInfo > r = new TaskCompletionSource < BuildingsInfo > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetBuildingsByType, buff, (bytes, offset) => {
                BuildingsInfo ret = MsgProtocol.decode < BuildingsInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < BuildingsInfo > GetAllBuildings(SimpleMsg arg0) {
            TaskCompletionSource < BuildingsInfo > r = new TaskCompletionSource < BuildingsInfo > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetAllBuildings, buff, (bytes, offset) => {
                BuildingsInfo ret = MsgProtocol.decode < BuildingsInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > DoResearch(SimpleMsg arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.DoResearch, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > PubRecruit(RecruitOption arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.PubRecruit, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > SetNotShowFlag(SimpleMsg arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SetNotShowFlag, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > TrainSoldier(TrainInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.TrainSoldier, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > BuildingLevelUp(int arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.BuildingLevelUp, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > treatWounded(SoldiersInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.treatWounded, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > setActiveMainSkills(KeyValuePair arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.setActiveMainSkills, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > StartAndFinishQueue(QueueInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartAndFinishQueue, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > FinishStartedQueue(int arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.FinishStartedQueue, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > AccelerateQueue(AccelerateInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.AccelerateQueue, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > CancelQueue(int arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.CancelQueue, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetProductResult > PlayerGetProduct(int arg0) {
            TaskCompletionSource < GetProductResult > r = new TaskCompletionSource < GetProductResult > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.PlayerGetProduct, buff, (bytes, offset) => {
                GetProductResult ret = MsgProtocol.decode < GetProductResult > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetProductResultAll > PlayerGetProductByType(int arg0) {
            TaskCompletionSource < GetProductResultAll > r = new TaskCompletionSource < GetProductResultAll > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.PlayerGetProductByType, buff, (bytes, offset) => {
                GetProductResultAll ret = MsgProtocol.decode < GetProductResultAll > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < BuildingAttrs > GetBuildingAttr(int arg0) {
            TaskCompletionSource < BuildingAttrs > r = new TaskCompletionSource < BuildingAttrs > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.GetBuildingAttr, buff, (bytes, offset) => {
                BuildingAttrs ret = MsgProtocol.decode < BuildingAttrs > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > RecoveryTurret(int arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.RecoveryTurret, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ComposeTurret(ComposeTurretInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ComposeTurret, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ComposeBackList > ComposeManyTurret(ComposeInfo arg0) {
            TaskCompletionSource < ComposeBackList > r = new TaskCompletionSource < ComposeBackList > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ComposeManyTurret, buff, (bytes, offset) => {
                ComposeBackList ret = MsgProtocol.decode < ComposeBackList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < BuildingsAllPlan > GetInitEditBuildingScene() {
            TaskCompletionSource < BuildingsAllPlan > r = new TaskCompletionSource < BuildingsAllPlan > ();

            PomeloClient.Instance.request((uint) ServiceId.GetInitEditBuildingScene, (bytes, offset) => {
                BuildingsAllPlan ret = MsgProtocol.decode < BuildingsAllPlan > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > BuildingSceneSave(BuildingPlan arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.BuildingSceneSave, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ChangeNewScene(int arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.ChangeNewScene, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbBuilding_GMHandler {
        enum ServiceId {
            AddBlock = 27,
                AddTurret = 28,
                TechLevelTo = 29,
                IncBuildingLevel = 30,

        }


        public static Task < BlockInfo > AddBlock(SimpleMsg arg0) {
            TaskCompletionSource < BlockInfo > r = new TaskCompletionSource < BlockInfo > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.AddBlock, buff, (bytes, offset) => {
                BlockInfo ret = MsgProtocol.decode < BlockInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > AddTurret(GmGetNewTurret arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.AddTurret, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > TechLevelTo(KeyValuePair arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.TechLevelTo, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > IncBuildingLevel(KeyValuePair arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.IncBuildingLevel, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wChat_Handler {
        enum ServiceId {
            ChatEntry = 31,
                GetChatDataByRequest = 32,
                CreateGroup = 33,
                ModifyGroupName = 34,
                OperateChannelGroup = 35,
                GetPlayerAllChannelData = 36,
                SetGroupFlagBit = 37,
                SetFlagBitToPlayer = 38,
                GetAllMembersInfo = 39,
                GetOnePlayerFlag = 40,

        }


        public static Task < ChatReturn > ChatEntry(SingleChatInfo arg0) {
            TaskCompletionSource < ChatReturn > r = new TaskCompletionSource < ChatReturn > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ChatEntry, buff, (bytes, offset) => {
                ChatReturn ret = MsgProtocol.decode < ChatReturn > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ChatDatas > GetChatDataByRequest(ChatRequest arg0) {
            TaskCompletionSource < ChatDatas > r = new TaskCompletionSource < ChatDatas > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetChatDataByRequest, buff, (bytes, offset) => {
                ChatDatas ret = MsgProtocol.decode < ChatDatas > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ChatReturn > CreateGroup(CreateGroupInfo arg0) {
            TaskCompletionSource < ChatReturn > r = new TaskCompletionSource < ChatReturn > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CreateGroup, buff, (bytes, offset) => {
                ChatReturn ret = MsgProtocol.decode < ChatReturn > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ChatReturn > ModifyGroupName(ModifyGroupInfo arg0) {
            TaskCompletionSource < ChatReturn > r = new TaskCompletionSource < ChatReturn > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ModifyGroupName, buff, (bytes, offset) => {
                ChatReturn ret = MsgProtocol.decode < ChatReturn > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ChatReturn > OperateChannelGroup(GroupOperationInfo arg0) {
            TaskCompletionSource < ChatReturn > r = new TaskCompletionSource < ChatReturn > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.OperateChannelGroup, buff, (bytes, offset) => {
                ChatReturn ret = MsgProtocol.decode < ChatReturn > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < PlayerChannelData > GetPlayerAllChannelData(SimpleMsg arg0) {
            TaskCompletionSource < PlayerChannelData > r = new TaskCompletionSource < PlayerChannelData > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetPlayerAllChannelData, buff, (bytes, offset) => {
                PlayerChannelData ret = MsgProtocol.decode < PlayerChannelData > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > SetGroupFlagBit(BitSetInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SetGroupFlagBit, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > SetFlagBitToPlayer(BitSetInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SetFlagBitToPlayer, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetMembersInfoRsp > GetAllMembersInfo(GetMembersInfoReq arg0) {
            TaskCompletionSource < GetMembersInfoRsp > r = new TaskCompletionSource < GetMembersInfoRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetAllMembersInfo, buff, (bytes, offset) => {
                GetMembersInfoRsp ret = MsgProtocol.decode < GetMembersInfoRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetOnePlrFlagRsp > GetOnePlayerFlag(GetOnePlrFlagReq arg0) {
            TaskCompletionSource < GetOnePlrFlagRsp > r = new TaskCompletionSource < GetOnePlrFlagRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetOnePlayerFlag, buff, (bytes, offset) => {
                GetOnePlrFlagRsp ret = MsgProtocol.decode < GetOnePlrFlagRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wfGate_Handler {
        enum ServiceId {
            EnterServer = 41,
                CreateRole = 42,
                ReconnectServer = 43,

        }


        public static Task < EnterServerRsp > EnterServer(EnterServerReq arg0) {
            TaskCompletionSource < EnterServerRsp > r = new TaskCompletionSource < EnterServerRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.EnterServer, buff, (bytes, offset) => {
                EnterServerRsp ret = MsgProtocol.decode < EnterServerRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < CreateRoleRsp > CreateRole(CreateRoleReq arg0) {
            TaskCompletionSource < CreateRoleRsp > r = new TaskCompletionSource < CreateRoleRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CreateRole, buff, (bytes, offset) => {
                CreateRoleRsp ret = MsgProtocol.decode < CreateRoleRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < EnterServerRsp > ReconnectServer(PlayerId arg0) {
            TaskCompletionSource < EnterServerRsp > r = new TaskCompletionSource < EnterServerRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ReconnectServer, buff, (bytes, offset) => {
                EnterServerRsp ret = MsgProtocol.decode < EnterServerRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wfGate_GMHandler {
        enum ServiceId {
            GMCreateRoles = 44,

        }


        public static Task < int > GMCreateRoles(CreateRoleReq arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMCreateRoles, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_Handler {
        enum ServiceId {
            StartBattle = 45,
                ViewNewPos = 46,
                CreateTroop = 47,
                StartTroopMarch = 48,
                TroopReturnHome = 49,
                GetResInfo = 50,
                GetTroopDetail = 51,
                LeaveWorldScene = 52,
                ItemInfo = 53,
                UseItem = 54,
                SynthetizeItem = 55,
                BuyAndUseItem = 56,
                MoveCastle = 57,
                StopMovement = 58,
                GetTransferGateState = 59,
                StartCameraFollow = 60,
                StopCameraFollow = 61,
                GetCastlePos = 62,
                GetBlockInfo = 63,
                ViewBattleDetail = 64,
                SyncBattleOps = 65,
                StartNextPhaseBattle = 66,
                SetNewbieGuideFlag = 67,
                SearchObject = 68,
                FindPlayerbyName = 69,

        }


        public static Task < ReturnMsg > StartBattle(AttackData arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartBattle, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ViewNewPos(CurPos arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ViewNewPos, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > CreateTroop(Troop arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CreateTroop, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StartTroopMarch(WsMarch arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartTroopMarch, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > TroopReturnHome(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.TroopReturnHome, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < WsResInfo > GetResInfo(WsObjId arg0) {
            TaskCompletionSource < WsResInfo > r = new TaskCompletionSource < WsResInfo > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetResInfo, buff, (bytes, offset) => {
                WsResInfo ret = MsgProtocol.decode < WsResInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < WsTroopDetail > GetTroopDetail(WsObjId arg0) {
            TaskCompletionSource < WsTroopDetail > r = new TaskCompletionSource < WsTroopDetail > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetTroopDetail, buff, (bytes, offset) => {
                WsTroopDetail ret = MsgProtocol.decode < WsTroopDetail > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > LeaveWorldScene() {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            PomeloClient.Instance.request((uint) ServiceId.LeaveWorldScene, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ItemInfoRsp > ItemInfo(ItemInfoReq arg0) {
            TaskCompletionSource < ItemInfoRsp > r = new TaskCompletionSource < ItemInfoRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ItemInfo, buff, (bytes, offset) => {
                ItemInfoRsp ret = MsgProtocol.decode < ItemInfoRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < UseItemRsp > UseItem(UseItemReq arg0) {
            TaskCompletionSource < UseItemRsp > r = new TaskCompletionSource < UseItemRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UseItem, buff, (bytes, offset) => {
                UseItemRsp ret = MsgProtocol.decode < UseItemRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < SynthetizeItemRsp > SynthetizeItem(SynthetizeItemReq arg0) {
            TaskCompletionSource < SynthetizeItemRsp > r = new TaskCompletionSource < SynthetizeItemRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SynthetizeItem, buff, (bytes, offset) => {
                SynthetizeItemRsp ret = MsgProtocol.decode < SynthetizeItemRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < BuyAndUseItemRsp > BuyAndUseItem(BuyAndUseItemReq arg0) {
            TaskCompletionSource < BuyAndUseItemRsp > r = new TaskCompletionSource < BuyAndUseItemRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.BuyAndUseItem, buff, (bytes, offset) => {
                BuyAndUseItemRsp ret = MsgProtocol.decode < BuyAndUseItemRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > MoveCastle(WsCastleMove arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.MoveCastle, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StopMovement(WsObjId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StopMovement, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < WsTransferGateList > GetTransferGateState(WsObjId arg0) {
            TaskCompletionSource < WsTransferGateList > r = new TaskCompletionSource < WsTransferGateList > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetTransferGateState, buff, (bytes, offset) => {
                WsTransferGateList ret = MsgProtocol.decode < WsTransferGateList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StartCameraFollow(WsObjId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartCameraFollow, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StopCameraFollow() {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            PomeloClient.Instance.request((uint) ServiceId.StopCameraFollow, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < vect2 > GetCastlePos() {
            TaskCompletionSource < vect2 > r = new TaskCompletionSource < vect2 > ();

            PomeloClient.Instance.request((uint) ServiceId.GetCastlePos, (bytes, offset) => {
                vect2 ret = MsgProtocol.decode < vect2 > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < BattleBlockInfo > GetBlockInfo(WsObjId arg0) {
            TaskCompletionSource < BattleBlockInfo > r = new TaskCompletionSource < BattleBlockInfo > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetBlockInfo, buff, (bytes, offset) => {
                BattleBlockInfo ret = MsgProtocol.decode < BattleBlockInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ViewBattleDetail(int arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.ViewBattleDetail, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > SyncBattleOps(BattleSyncFrameOps arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SyncBattleOps, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StartNextPhaseBattle(NextPhaseBattle arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartNextPhaseBattle, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > SetNewbieGuideFlag(KeyValuePair arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SetNewbieGuideFlag, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < SearchedObjectList > SearchObject(SearchPara arg0) {
            TaskCompletionSource < SearchedObjectList > r = new TaskCompletionSource < SearchedObjectList > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SearchObject, buff, (bytes, offset) => {
                SearchedObjectList ret = MsgProtocol.decode < SearchedObjectList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < SearchedPlayerInfo > FindPlayerbyName(string arg0) {
            TaskCompletionSource < SearchedPlayerInfo > r = new TaskCompletionSource < SearchedPlayerInfo > ();

            byte[] buff = new byte[arg0.Length + 2];
            int index = 0;
            MsgProtocol.WriteInt16BE(buff, index, arg0.Length);
            index += 2;
            for (int idx = 0; idx < arg0.Length; idx++) {
                buff[index + idx] = (byte) arg0[idx];
            }
            index += arg0.Length;
            PomeloClient.Instance.request((uint) ServiceId.FindPlayerbyName, buff, (bytes, offset) => {
                SearchedPlayerInfo ret = MsgProtocol.decode < SearchedPlayerInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_UnionHandler {
        enum ServiceId {
            GetUnionList = 70,
                GetApplyUnionList = 71,
                OperateUnion = 72,
                CreateUnion = 73,
                UnionJobChg = 74,
                GetApplylist = 75,
                SearchUnion = 76,
                ChangeUnionSet = 77,
                ChangeUnionName = 78,
                ChangeUnionAbbr = 79,
                ChangeUnionBadge = 80,
                GetUnionMembers = 81,
                GetInvitePlayers = 82,
                InvitePlayers = 83,

        }


        public static Task < UnionList > GetUnionList(int arg0) {
            TaskCompletionSource < UnionList > r = new TaskCompletionSource < UnionList > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.GetUnionList, buff, (bytes, offset) => {
                UnionList ret = MsgProtocol.decode < UnionList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < UnionList > GetApplyUnionList(int arg0) {
            TaskCompletionSource < UnionList > r = new TaskCompletionSource < UnionList > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.GetApplyUnionList, buff, (bytes, offset) => {
                UnionList ret = MsgProtocol.decode < UnionList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > OperateUnion(UnionOperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.OperateUnion, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > CreateUnion(CreateArgs arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CreateUnion, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > UnionJobChg(UnionJobMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.UnionJobChg, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ApplyPlayerList > GetApplylist() {
            TaskCompletionSource < ApplyPlayerList > r = new TaskCompletionSource < ApplyPlayerList > ();

            PomeloClient.Instance.request((uint) ServiceId.GetApplylist, (bytes, offset) => {
                ApplyPlayerList ret = MsgProtocol.decode < ApplyPlayerList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < UnionList > SearchUnion(SearchArgs arg0) {
            TaskCompletionSource < UnionList > r = new TaskCompletionSource < UnionList > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SearchUnion, buff, (bytes, offset) => {
                UnionList ret = MsgProtocol.decode < UnionList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ChangeUnionSet(CreateArgs arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ChangeUnionSet, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ChangeUnionName(string arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            byte[] buff = new byte[arg0.Length + 2];
            int index = 0;
            MsgProtocol.WriteInt16BE(buff, index, arg0.Length);
            index += 2;
            for (int idx = 0; idx < arg0.Length; idx++) {
                buff[index + idx] = (byte) arg0[idx];
            }
            index += arg0.Length;
            PomeloClient.Instance.request((uint) ServiceId.ChangeUnionName, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ChangeUnionAbbr(string arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            byte[] buff = new byte[arg0.Length + 2];
            int index = 0;
            MsgProtocol.WriteInt16BE(buff, index, arg0.Length);
            index += 2;
            for (int idx = 0; idx < arg0.Length; idx++) {
                buff[index + idx] = (byte) arg0[idx];
            }
            index += arg0.Length;
            PomeloClient.Instance.request((uint) ServiceId.ChangeUnionAbbr, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ChangeUnionBadge(WsObjId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ChangeUnionBadge, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < UnionMembers > GetUnionMembers() {
            TaskCompletionSource < UnionMembers > r = new TaskCompletionSource < UnionMembers > ();

            PomeloClient.Instance.request((uint) ServiceId.GetUnionMembers, (bytes, offset) => {
                UnionMembers ret = MsgProtocol.decode < UnionMembers > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < InvitePlayers > GetInvitePlayers() {
            TaskCompletionSource < InvitePlayers > r = new TaskCompletionSource < InvitePlayers > ();

            PomeloClient.Instance.request((uint) ServiceId.GetInvitePlayers, (bytes, offset) => {
                InvitePlayers ret = MsgProtocol.decode < InvitePlayers > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > InvitePlayers(ULongIdList arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.InvitePlayers, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_FriendHandler {
        enum ServiceId {
            GetFriendList = 84,
                ApplyasFriend = 85,
                AddasFriend = 86,
                DefuseasFriend = 87,
                DeleteFriend = 88,
                FindPlayer = 89,
                FindPlayerbyName = 90,

        }


        public static Task < FriendList > GetFriendList() {
            TaskCompletionSource < FriendList > r = new TaskCompletionSource < FriendList > ();

            PomeloClient.Instance.request((uint) ServiceId.GetFriendList, (bytes, offset) => {
                FriendList ret = MsgProtocol.decode < FriendList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ApplyasFriend(PlayerId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ApplyasFriend, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > AddasFriend(PlayerId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.AddasFriend, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > DefuseasFriend(PlayerId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.DefuseasFriend, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > DeleteFriend(PlayerId arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.DeleteFriend, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < Friend > FindPlayer(PlayerId arg0) {
            TaskCompletionSource < Friend > r = new TaskCompletionSource < Friend > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.FindPlayer, buff, (bytes, offset) => {
                Friend ret = MsgProtocol.decode < Friend > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < Friend > FindPlayerbyName(string arg0) {
            TaskCompletionSource < Friend > r = new TaskCompletionSource < Friend > ();

            byte[] buff = new byte[arg0.Length + 2];
            int index = 0;
            MsgProtocol.WriteInt16BE(buff, index, arg0.Length);
            index += 2;
            for (int idx = 0; idx < arg0.Length; idx++) {
                buff[index + idx] = (byte) arg0[idx];
            }
            index += arg0.Length;
            PomeloClient.Instance.request((uint) ServiceId.FindPlayerbyName, buff, (bytes, offset) => {
                Friend ret = MsgProtocol.decode < Friend > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_TaskHandler {
        enum ServiceId {
            ClickGetReward = 91,
                OpenActiveBox = 92,
                GMFinishTask = 93,

        }


        public static Task < int > ClickGetReward(SingleTaskInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ClickGetReward, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > OpenActiveBox(int arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.OpenActiveBox, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > GMFinishTask(SingleTaskInfo arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMFinishTask, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_GMHandler {
        enum ServiceId {
            GMOperatePlayers = 94,
                GMMovePlayerCastles = 95,
                MoveCastle = 96,
                CreateRes = 97,
                CreateMutiRes = 98,
                GMTransfer = 99,
                GMAddSolider = 100,
                GMAddLeader = 101,
                GMChangeCollectSpeed = 102,
                GMSetResource = 103,
                GMSetItem = 104,
                GMPassBarriers = 105,
                GMAddSysMail = 106,
                GMGetHeroInfo = 107,
                GMGetResource = 108,
                GMStartAutoTrade = 109,
                GMSetEquip = 110,

        }


        public static Task < ReturnMsg > GMOperatePlayers(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMOperatePlayers, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMMovePlayerCastles(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMMovePlayerCastles, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > MoveCastle(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.MoveCastle, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > CreateRes(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CreateRes, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > CreateMutiRes(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CreateMutiRes, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMTransfer(vect2 arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMTransfer, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMAddSolider(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMAddSolider, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMAddLeader(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMAddLeader, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMChangeCollectSpeed(OperateMsg arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMChangeCollectSpeed, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMSetResource(PlayerResourceInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMSetResource, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMSetItem(ItemInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMSetItem, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > GMPassBarriers(OperateMsg arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMPassBarriers, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMAddSysMail(GMSendMail arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMAddSysMail, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < PlayerHeroInfo > GMGetHeroInfo() {
            TaskCompletionSource < PlayerHeroInfo > r = new TaskCompletionSource < PlayerHeroInfo > ();

            PomeloClient.Instance.request((uint) ServiceId.GMGetHeroInfo, (bytes, offset) => {
                PlayerHeroInfo ret = MsgProtocol.decode < PlayerHeroInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < RepeatedInt32 > GMGetResource(RepeatedInt32 arg0) {
            TaskCompletionSource < RepeatedInt32 > r = new TaskCompletionSource < RepeatedInt32 > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMGetResource, buff, (bytes, offset) => {
                RepeatedInt32 ret = MsgProtocol.decode < RepeatedInt32 > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMStartAutoTrade(AutoTrade arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMStartAutoTrade, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > GMSetEquip(GMEquipInfo arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GMSetEquip, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_MailHandler {
        enum ServiceId {
            GetNewerMailTitle = 111,
                GetMailTitle = 112,
                GetMailContent = 113,
                ReadMail = 114,
                RecvMailAward = 115,
                ReadRecvAllMail = 116,
                CollectMail = 117,
                DeleteMail = 118,
                SendMail = 119,

        }


        public static Task < GetNewerMailTitleRsp > GetNewerMailTitle(GetNewerMailTitleReq arg0) {
            TaskCompletionSource < GetNewerMailTitleRsp > r = new TaskCompletionSource < GetNewerMailTitleRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetNewerMailTitle, buff, (bytes, offset) => {
                GetNewerMailTitleRsp ret = MsgProtocol.decode < GetNewerMailTitleRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetMailTitleRsp > GetMailTitle(GetMailTitleReq arg0) {
            TaskCompletionSource < GetMailTitleRsp > r = new TaskCompletionSource < GetMailTitleRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetMailTitle, buff, (bytes, offset) => {
                GetMailTitleRsp ret = MsgProtocol.decode < GetMailTitleRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetMailContentRsp > GetMailContent(GetMailContentReq arg0) {
            TaskCompletionSource < GetMailContentRsp > r = new TaskCompletionSource < GetMailContentRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetMailContent, buff, (bytes, offset) => {
                GetMailContentRsp ret = MsgProtocol.decode < GetMailContentRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReadMailRsp > ReadMail(ReadMailReq arg0) {
            TaskCompletionSource < ReadMailRsp > r = new TaskCompletionSource < ReadMailRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ReadMail, buff, (bytes, offset) => {
                ReadMailRsp ret = MsgProtocol.decode < ReadMailRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < RecvMailAwardRsp > RecvMailAward(RecvMailAwardReq arg0) {
            TaskCompletionSource < RecvMailAwardRsp > r = new TaskCompletionSource < RecvMailAwardRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.RecvMailAward, buff, (bytes, offset) => {
                RecvMailAwardRsp ret = MsgProtocol.decode < RecvMailAwardRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReadRecvAllMailRsp > ReadRecvAllMail(ReadRecvAllMailReq arg0) {
            TaskCompletionSource < ReadRecvAllMailRsp > r = new TaskCompletionSource < ReadRecvAllMailRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ReadRecvAllMail, buff, (bytes, offset) => {
                ReadRecvAllMailRsp ret = MsgProtocol.decode < ReadRecvAllMailRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < CollectMailRsp > CollectMail(CollectMailReq arg0) {
            TaskCompletionSource < CollectMailRsp > r = new TaskCompletionSource < CollectMailRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.CollectMail, buff, (bytes, offset) => {
                CollectMailRsp ret = MsgProtocol.decode < CollectMailRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < DeleteMailRsp > DeleteMail(DeleteMailReq arg0) {
            TaskCompletionSource < DeleteMailRsp > r = new TaskCompletionSource < DeleteMailRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.DeleteMail, buff, (bytes, offset) => {
                DeleteMailRsp ret = MsgProtocol.decode < DeleteMailRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < SendMailRsp > SendMail(SendMailReq arg0) {
            TaskCompletionSource < SendMailRsp > r = new TaskCompletionSource < SendMailRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SendMail, buff, (bytes, offset) => {
                SendMailRsp ret = MsgProtocol.decode < SendMailRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_TradingPortHandler {
        enum ServiceId {
            BuyCommodity = 120,
                SellCommodity = 121,
                GetPortInfo = 122,
                GetTradeMainInfo = 123,
                GetTradeMarketList = 124,
                GetTradeRouteInfo = 125,
                StartAutoTrade = 126,
                StopAutoTrade = 127,

        }


        public static Task < ReturnMsg > BuyCommodity(BuyItem arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.BuyCommodity, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > SellCommodity(SellItem arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SellCommodity, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < PortInfo > GetPortInfo(tradingPortId arg0) {
            TaskCompletionSource < PortInfo > r = new TaskCompletionSource < PortInfo > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetPortInfo, buff, (bytes, offset) => {
                PortInfo ret = MsgProtocol.decode < PortInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < TradingMainInfo > GetTradeMainInfo() {
            TaskCompletionSource < TradingMainInfo > r = new TaskCompletionSource < TradingMainInfo > ();

            PomeloClient.Instance.request((uint) ServiceId.GetTradeMainInfo, (bytes, offset) => {
                TradingMainInfo ret = MsgProtocol.decode < TradingMainInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < TradingMarket > GetTradeMarketList() {
            TaskCompletionSource < TradingMarket > r = new TaskCompletionSource < TradingMarket > ();

            PomeloClient.Instance.request((uint) ServiceId.GetTradeMarketList, (bytes, offset) => {
                TradingMarket ret = MsgProtocol.decode < TradingMarket > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < TradeRouteInfo > GetTradeRouteInfo() {
            TaskCompletionSource < TradeRouteInfo > r = new TaskCompletionSource < TradeRouteInfo > ();

            PomeloClient.Instance.request((uint) ServiceId.GetTradeRouteInfo, (bytes, offset) => {
                TradeRouteInfo ret = MsgProtocol.decode < TradeRouteInfo > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StartAutoTrade(AutoTrade arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartAutoTrade, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > StopAutoTrade() {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();

            PomeloClient.Instance.request((uint) ServiceId.StopAutoTrade, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_ExpeditionHandler {
        enum ServiceId {
            StartBattle = 128,
                GetExpeditionTroops = 129,
                EndBattle = 130,

        }


        public static Task < ReturnMsg > StartBattle(StartExpedition arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.StartBattle, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ConfigTroopList > GetExpeditionTroops(TroopListReqPara arg0) {
            TaskCompletionSource < ConfigTroopList > r = new TaskCompletionSource < ConfigTroopList > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetExpeditionTroops, buff, (bytes, offset) => {
                ConfigTroopList ret = MsgProtocol.decode < ConfigTroopList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > EndBattle(BattleResult arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.EndBattle, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_TransferHandler {
        enum ServiceId {
            TranferToDstServer = 131,

        }


        public static Task < PlayerTransferToDstSvrRsp > TranferToDstServer(PlayerTransferToDstSvrReq arg0) {
            TaskCompletionSource < PlayerTransferToDstSvrRsp > r = new TaskCompletionSource < PlayerTransferToDstSvrRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.TranferToDstServer, buff, (bytes, offset) => {
                PlayerTransferToDstSvrRsp ret = MsgProtocol.decode < PlayerTransferToDstSvrRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_PlayerHeroHandler {
        enum ServiceId {
            AddHeroExpByItems = 132,
                IncHeroStarStageByItem = 133,
                GetMainFightHeroes = 134,
                SetMainFightHeroes = 135,
                IncHeroSkillLevelByItem = 136,

        }


        public static Task < int > AddHeroExpByItems(UseItemsToHero arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.AddHeroExpByItems, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > IncHeroStarStageByItem(int arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.IncHeroStarStageByItem, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ConfigTroop > GetMainFightHeroes() {
            TaskCompletionSource < ConfigTroop > r = new TaskCompletionSource < ConfigTroop > ();

            PomeloClient.Instance.request((uint) ServiceId.GetMainFightHeroes, (bytes, offset) => {
                ConfigTroop ret = MsgProtocol.decode < ConfigTroop > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > SetMainFightHeroes(ConfigTroop arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SetMainFightHeroes, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > IncHeroSkillLevelByItem(int arg0) {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            byte[] buff = new byte[4];
            int index = 0;
            MsgProtocol.WriteInt32BE(buff, index, arg0);
            index += 4;
            PomeloClient.Instance.request((uint) ServiceId.IncHeroSkillLevelByItem, buff, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_BookmarkHandler {
        enum ServiceId {
            GetBookmarkList = 137,
                AddBookmark = 138,
                ChangeBookmark = 139,
                DeleteBookmark = 140,

        }


        public static Task < BookmarkList > GetBookmarkList() {
            TaskCompletionSource < BookmarkList > r = new TaskCompletionSource < BookmarkList > ();

            PomeloClient.Instance.request((uint) ServiceId.GetBookmarkList, (bytes, offset) => {
                BookmarkList ret = MsgProtocol.decode < BookmarkList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > AddBookmark(Bookmark arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.AddBookmark, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > ChangeBookmark(Bookmark arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ChangeBookmark, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ReturnMsg > DeleteBookmark(Bookmark arg0) {
            TaskCompletionSource < ReturnMsg > r = new TaskCompletionSource < ReturnMsg > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.DeleteBookmark, buff, (bytes, offset) => {
                ReturnMsg ret = MsgProtocol.decode < ReturnMsg > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

    public static class wbScene_EquipHandler {
        enum ServiceId {
            GetHeroEquipList = 141,
                SetHeroEquip = 142,
                GetStuffs = 143,
                GetAllEquip = 144,
                SynthetizeStuff = 145,
                ComposeEquip = 146,
                DecomposeEquip = 147,

        }


        public static Task < HeroEquipList > GetHeroEquipList() {
            TaskCompletionSource < HeroEquipList > r = new TaskCompletionSource < HeroEquipList > ();

            PomeloClient.Instance.request((uint) ServiceId.GetHeroEquipList, (bytes, offset) => {
                HeroEquipList ret = MsgProtocol.decode < HeroEquipList > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < int > SetHeroEquip() {
            TaskCompletionSource < int > r = new TaskCompletionSource < int > ();

            PomeloClient.Instance.request((uint) ServiceId.SetHeroEquip, (bytes, offset) => {

                int ret = MsgProtocol.ReadInt32BE(bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetStuffsRsp > GetStuffs(GetStuffsReq arg0) {
            TaskCompletionSource < GetStuffsRsp > r = new TaskCompletionSource < GetStuffsRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetStuffs, buff, (bytes, offset) => {
                GetStuffsRsp ret = MsgProtocol.decode < GetStuffsRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < GetAllEquipRsp > GetAllEquip(GetAllEquipReq arg0) {
            TaskCompletionSource < GetAllEquipRsp > r = new TaskCompletionSource < GetAllEquipRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.GetAllEquip, buff, (bytes, offset) => {
                GetAllEquipRsp ret = MsgProtocol.decode < GetAllEquipRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < SynthetizeStuffRsp > SynthetizeStuff(SynthetizeStuffReq arg0) {
            TaskCompletionSource < SynthetizeStuffRsp > r = new TaskCompletionSource < SynthetizeStuffRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.SynthetizeStuff, buff, (bytes, offset) => {
                SynthetizeStuffRsp ret = MsgProtocol.decode < SynthetizeStuffRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < ComposeEquipRsp > ComposeEquip(ComposeEquipReq arg0) {
            TaskCompletionSource < ComposeEquipRsp > r = new TaskCompletionSource < ComposeEquipRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.ComposeEquip, buff, (bytes, offset) => {
                ComposeEquipRsp ret = MsgProtocol.decode < ComposeEquipRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

        public static Task < DeComposeEquipRsp > DecomposeEquip(DeComposeEquipReq arg0) {
            TaskCompletionSource < DeComposeEquipRsp > r = new TaskCompletionSource < DeComposeEquipRsp > ();
            byte[] buff = MsgProtocol.encode(arg0);
            PomeloClient.Instance.request((uint) ServiceId.DecomposeEquip, buff, (bytes, offset) => {
                DeComposeEquipRsp ret = MsgProtocol.decode < DeComposeEquipRsp > (bytes, offset);
                r.SetResult(ret);
            });
            return r.Task;
        }

    }

}