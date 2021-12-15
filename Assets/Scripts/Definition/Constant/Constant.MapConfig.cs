using DG.Tweening;

namespace ProHA
{
    public static partial class Constant
    {
        public static class MapConfig
        {
            public const int Width = 60;
            public const int Height = 60;

            public const int PveWidth = 600;
            public const int PveHeight = 600;

            public const int EdgeOffset = 12;

            public const float CellSize = 1.5f;//每個地塊所對應的比例尺
            public const float PI = 3.14f;
            public const int RotationAngle = 45;//地块的统一旋转角度
            public const int MoveCellCount = 1;//每次移動的地塊格子數

            public const float PlaneY = 2f;


            public const int EnemyWidth = 100;
            public const int EnemyHeight = 100;

            public static int TotalWidth = 0;
            public static int TotalHeight = 0;

            //public const int CoreBuildID = 1;
            public const int CoreBuildType = 2;

            public const int BattleFirstInSpace = 20;
            public static int BattleAniSpace = 6;//通过修改这个值 0 or 6 来处理边缘生成的位置偏移
            public static Ease MoveEase = Ease.InCubic;
            public static float BattleAniDuration = 1.6f;
            public static float BattleCamShakeDuration = 0.5f;

            //标记地块的x或y小于等于-1000为在背包中
            public const int PlaneBagPos = -1000;

            public const float MoveTop = 0.6f;//移动上限阈值
            public const float MoveBottom = 0.4f;//移动下限阈值

            public const float ComposeRange1 = 0.4f;
            public const float ComposeRange2 = 0.6f;

            public const int TurretcenterID = 27;
            public enum EJoinMapDir
            {
                EDir_Left = 1, // 从左拼
                EDir_Right = 2, // 从右拼
                EDir_Top = 3, // 从上拼
                EDir_Bottom = 4, // 从下拼

                EDir_Left_First = 5, // 第一次进入的时候从左拼，传给模拟器为EDir_Left
            };
        }
    }
}
