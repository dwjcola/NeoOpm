using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.AddressableResource;
using UnityEngine;
using XLua;

namespace ProHA
{
    public class HeroSceneMgr : MonoBehaviour
    {
        private const float ScenePosX = -0.6f; 
        
        private enum ESceneSlide
        {
            INSTANT,        // 无滑动效果，立即切换
            SLIDE_TO_LEFT,    // 滑向右侧
            SLIDE_TO_RIGHT,    // 滑向左侧

        }
        
        
        private static IAddressableResourceManager s_addressableResourceManager;

        
        public static HeroSceneMgr instance
        {
            get;
            private set;
        }
        
        private const float DRAG_RANGE = 2.9f;
        private const float SWITCH_LIMIT = 1.35f;
        private const float SCREEN_TO_WORLD_FACTOR = 0.005f;
        
        // 场景相机
        public Camera camera_;

        private List<int> m_List;     // lua发送过来的，排序后的英雄列表
        
        // 当前场景所有英雄列表，会按照某种规则排序
        public List<__HeroData> heroDatas;
        
        // 当前英雄的列表id
        private int _curListIndex = -1;
        
        // 当前英雄rid
        // private int _curHeroRID = 0;
        
        // 各英雄spine实例
        private Dictionary<int, GameObject> _heroInstances = new Dictionary<int, GameObject>();

        // 各英雄对应场景实例
        private Dictionary<int, GameObject> _sceneInstances = new Dictionary<int, GameObject>();

        // 当前场景
        private GameObject _curSceneGO;
        
        // 上一场景
        private GameObject _lastSceneGO;
        
        // Touch起始位置
        // private Vector2 _touchStartPos;

        private bool _isDragging = false;
        private Coroutine _sceneSlidingcroutine;
        
        //#if UNITY_EDITOR
        // 上一帧鼠标位置
        private Vector3 _lastMousePostion;
       
        //#endif
        public float FovNor = 30f;
        public float FovNear = 19f;
        public float PosYNor = 1f;
        public float PosYNear = 1.5f;
        private bool Near = false;


        
        public static async void LoadHeroScene()
        {
            /*
            s_addressableResourceManager = GameFramework.GameFrameworkEntry.GetModule<IAddressableResourceManager>();
            GameObject heroSceneAsset = await s_addressableResourceManager.LoadAssetAsync<GameObject>("Assets/Resource_MS/Prefabs/HeroScenes/HeroSceneMgr.prefab").Task;
            GameObject heroScene = UnityEngine.Object.Instantiate(heroSceneAsset);
            */
        }
        
        
        
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("HeroSceneMgr存在多个实例");    
            }
                
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!camera_)
            {
                camera_ = GetComponentInChildren<Camera>();
                if (!camera_)
                    return;
            }
        }

        public void TweenNear(bool flag)
        {
            Near = flag;
            if (flag)
            {
                camera_.DOFieldOfView(FovNear, 1f);
                Vector3 pos = camera_.transform.localPosition;
                camera_.transform.DOLocalMove(new Vector3(pos.x, PosYNear, pos.z), 1f);
            }
            else
            {
                camera_.DOFieldOfView(FovNor, 1f);
                Vector3 pos = camera_.transform.localPosition;
                camera_.transform.DOLocalMove(new Vector3(pos.x, PosYNor, pos.z), 1f);
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Near)
            {
                return;//近景不可拖动
            }
            // 测试，用左右箭头键切换场景
            /*if (Input.GetKey(KeyCode.RightArrow))
            {
                SwtichToNextHero();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                SwtichToPreviousHero();
            }*/

            // 拖动场景
    //#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (CommonUtility.CheckGuiRaycastObjects())
                {
                    _isDragging = false;
                    return;
                }
                
                _lastMousePostion = Input.mousePosition;
                _isDragging = true;
            }
            else if (Input.GetMouseButton(0))
            {
                if (!_isDragging) 
                    return;
                
                // 根据鼠标delta移动场景
                Vector3 deltaPos = Input.mousePosition - _lastMousePostion;
                float sceneOffsetX = deltaPos.x * SCREEN_TO_WORLD_FACTOR;
                _curSceneGO.transform.localPosition += new Vector3(sceneOffsetX, 0, 0);
                
                // 拖动范围界限处理
                Vector3 localPos = _curSceneGO.transform.localPosition;
                if (localPos.x > DRAG_RANGE)
                {
                    _curSceneGO.transform.localPosition = new Vector3(DRAG_RANGE, localPos.y, localPos.z); 
                }
                else if (localPos.x < -1 * DRAG_RANGE)
                {
                    _curSceneGO.transform.localPosition = new Vector3(-1 * DRAG_RANGE, localPos.y, localPos.z);
                }
                
                _lastMousePostion = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!_isDragging)
                    return;
                
                // 判断是复位还是切换到下一个场景
                float sceneOffsetX = _curSceneGO.transform.localPosition.x;

                if (sceneOffsetX > SWITCH_LIMIT)
                {
                    // 如果已达到列表的首尾，则进行场景复位而不是切换
                    if (IsAtStartOfTheList())
                    {
                        _sceneSlidingcroutine = StartCoroutine(AnimateResetScene());
                    }
                    else
                    {
                        SwtichToPreviousHero();
                    }
                }
                else if (sceneOffsetX < -1 * SWITCH_LIMIT)
                {
                    if (IsAtEndOfTheList())
                    {
                        _sceneSlidingcroutine = StartCoroutine(AnimateResetScene());
                    }
                    else
                    {
                        SwtichToNextHero();
                    }
                }
                else
                {
                    // 复位场景位置
                    _sceneSlidingcroutine = StartCoroutine(AnimateResetScene());
                }
            }

            
    //#else
            /*
            // 拖动
            if (Input.touchCount > 0)
            {
                Touch touch0 = Input.GetTouch(0);
                
                float sceneOffsetX = 0;
                switch (touch0.phase)
                {
                    case TouchPhase.Began:
                        if (CommonUtility.CheckGuiRaycastObjects())
                        {
                            _isDragging = false;
                            return;
                        }
                        _lastMousePostion = touch0.position;
                        _isDragging = true;
                        break;
                    case TouchPhase.Moved:
                        if (!_isDragging) 
                            return;
                        
                        Vector2 deltaPos = touch0.deltaPosition;
                        sceneOffsetX = deltaPos.x * SCREEN_TO_WORLD_FACTOR; 
                        _curSceneGO.transform.localPosition += new Vector3(sceneOffsetX, 0, 0);  
                    
                        // 拖动范围界限处理
                        Vector3 localPos = _curSceneGO.transform.localPosition;
                        if (localPos.x > DRAG_RANGE)
                        {
                            _curSceneGO.transform.localPosition = new Vector3(DRAG_RANGE, localPos.y, localPos.z); 
                        }
                        else if (localPos.x < -1 * DRAG_RANGE)
                        {
                            _curSceneGO.transform.localPosition = new Vector3(-1 * DRAG_RANGE, localPos.y, localPos.z);
                        }
                
                        _lastMousePostion = Input.mousePosition;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (!_isDragging)
                            return;
                        // 判断是复位还是切换到下一个场景
                        sceneOffsetX = _curSceneGO.transform.localPosition.x;

                        if (sceneOffsetX > SWITCH_LIMIT)
                        {
                            // 如果已达到列表的首尾，则进行场景复位而不是切换
                            if (IsAtStartOfTheList())
                            {
                                _sceneSlidingcroutine = StartCoroutine(AnimateResetScene());
                            }
                            else
                            {
                                SwtichToPreviousHero();
                            }
                        }
                        else if (sceneOffsetX < -1 * SWITCH_LIMIT)
                        {
                            if (IsAtEndOfTheList())
                            {
                                _sceneSlidingcroutine = StartCoroutine(AnimateResetScene());
                            }
                            else
                            {
                                SwtichToNextHero();
                            }
                        }
                        else
                        {
                            // 复位场景位置
                            _sceneSlidingcroutine = StartCoroutine(AnimateResetScene());
                        }
                        
                        break;
                    default:

                        break;
                }
            }
            */

    //#endif

        }


        // 初始显示当前英雄，从无到有，无场景切换效果
        public void InitHeroScene(List<int> list)
        {
            m_List = list;
            
            // 取列表中第一个英雄
            int index = 0;
            if (index == _curListIndex || index >= m_List.Count)
                return;
            
            // ---获取数据，资源
            LuaTable lt = LC.GetTable("HeroType", m_List[index]);
            string heroScene = lt.Get<string>("heroScene");
            int gId = lt.Get<int>("gid");
            GetHeroSceneData(0, heroScene,gId, ESceneSlide.INSTANT, OnGetHeroSceneData);
        }

        public void UpdateHeroScene(List<int> list,int index)
        {
            m_List = list;
            _curListIndex = index;
        }
        private static int tempID = 0;
        private void GetHeroSceneData(int index, string heroScene,int tid, ESceneSlide eSlide, Action<int, int, __HeroData, ESceneSlide> callback)
        {
            // heroScene = "Ndge";

            // todo 先临时用ScriptObject，后边改成项目正式资源管理接口
            __HeroData heroData = null; 
            
            // todo 临时之举，后边应换成正式的资源加载写法
            if (heroScene == "Bdk")
            {
                heroData = heroDatas[0];
            }
            else if (heroScene == "Aefld")
            {
                heroData = heroDatas[1];
            }
            else if (heroScene == "Hanniba")
            {
                heroData = heroDatas[2];
            }
            else if (heroScene == "Zhende")
            {
                heroData = heroDatas[3];
            }
            else if (heroScene == "Napuolun")
            {
                heroData = heroDatas[4];
            }
            else if (heroScene == "Yalishanda")
            {
                heroData = heroDatas[5];
            }
            else if (heroScene == "Klwe")
            {
                heroData = heroDatas[6];
            }
            else if (heroScene == "Ndge")
            {
                heroData = heroDatas[7];
            }
            else
            {
                heroData = heroDatas[7];
            }

            /*
            // test 
            if (tempID < 3)
            {
                heroData = heroDatas[tempID++];
            }
            else
            {
                heroData = heroDatas[3];
            }
            */
            
            
            
            callback(index, tid, heroData, eSlide);
        }
        
        private void OnGetHeroSceneData(int index, int heroTId, __HeroData heroData, ESceneSlide eSlide)
        {
            GameObject sceneGo = SetupScene(heroTId, heroData);

            if (sceneGo == null) return;

            if (eSlide == ESceneSlide.INSTANT)
            {
                // ---布置场景
                sceneGo.transform.parent = transform;
                sceneGo.SetActive(true);
                sceneGo.transform.localPosition = new Vector3(ScenePosX, 1f, 0);

                _curListIndex = index;
                _lastSceneGO = _curSceneGO;
                _curSceneGO = sceneGo;
            }
            else 
            {
                // 放置新场景
                sceneGo.transform.parent = transform;
                sceneGo.SetActive(true);
                float xOffset = eSlide == ESceneSlide.SLIDE_TO_RIGHT ? 20 : -20; 
                sceneGo.transform.localPosition = new Vector3(_curSceneGO.transform.localPosition.x + xOffset, 1f, 0);

                _curListIndex = index;
                _lastSceneGO = _curSceneGO;
                _curSceneGO = sceneGo;
                
                _sceneSlidingcroutine = StartCoroutine(AnimateSwitchScene());
            }

        }
        
        
        public void SwtichToNextHero()
        {
            if (IsAtEndOfTheList())
                return;

            int nextIndex = _curListIndex + 1; 
            int heroTID = m_List[nextIndex];
            
            SwitchToHero(nextIndex, heroTID);
            
            // 通知lua
            LC.SendEvent("change_Hero_select", heroTID);
        }

        public void SwtichToPreviousHero()
        {
            if (IsAtStartOfTheList())
                return;

            int nextIndex = _curListIndex - 1; 
            int heroTID = m_List[nextIndex];
            
            SwitchToHero(nextIndex, heroTID);
            
            // 通知lua
            LC.SendEvent("change_Hero_select", heroTID);
        }

        public void SwitchToHero(int index, int heroTID)
        {
            if (index == _curListIndex || index >= m_List.Count)
                return;
            LuaTable lt = LC.GetTable("HeroType", heroTID);
            string heroScene = lt.Get<string>("heroScene");
            int gId = lt.Get<int>("gid");
            // 判断是要向右切换还是向左
            ESceneSlide eSlide = ESceneSlide.SLIDE_TO_RIGHT;
            if (index < _curListIndex)
            {
                eSlide  = ESceneSlide.SLIDE_TO_LEFT;
            }
            
            GetHeroSceneData(index, heroScene,gId, eSlide, OnGetHeroSceneData);
        }

        
        
        public void OnQuit()
        {
            if (_sceneSlidingcroutine != null)
            {
                StopCoroutine(_sceneSlidingcroutine);
            }
            
            _curListIndex = -1;
            
            if (_curSceneGO != null)
            {
                _curSceneGO.SetActive(false);
                _curSceneGO = null;
            }

            if (_lastSceneGO != null)
            {
                _lastSceneGO.SetActive(false);
                _lastSceneGO = null;
            }
            
            // TODO 释放资源
        }

        private IEnumerator AnimateSwitchScene()
        {
            // 先用纯代码实现个简单的飞入飞出动画
            // TODO 后边局部对象做淡入淡出要怎么搞？K动画？还是脚本？

            
            // 新场景飞入
            // 从当前位置快速飞到x=0处，视野正中
            Vector3 startPos = _curSceneGO.transform.localPosition;
            Vector3 endPos = new Vector3(ScenePosX, startPos.y, startPos.z);
            Vector3 distance = endPos - startPos;
            float duration = 0.2f;    // todo 临时写法
            Vector3 velocity = distance / duration;
            float startTime = Time.time;
            float elapsedTime = 0;
            
            yield return null;

            // 逐帧移动
            while (elapsedTime < duration)
            {
                elapsedTime = Time.time - startTime;
                Vector3 offset = velocity * Time.deltaTime;
                _curSceneGO.transform.localPosition = _curSceneGO.transform.localPosition + offset; 
                _lastSceneGO.transform.localPosition = _lastSceneGO.transform.localPosition + offset; 
                yield return null;
            }
            
            // 预计结束时间已到，直接移动到位
            _curSceneGO.transform.localPosition = endPos;
            _lastSceneGO.transform.localPosition = _lastSceneGO.transform.localPosition + distance;
            _lastSceneGO.SetActive(false);

        }

        private IEnumerator AnimateResetScene()
        {
            // 从当前位置移动回x=0处，视野正中
            Vector3 startPos = _curSceneGO.transform.localPosition;
            Vector3 endPos = new Vector3(ScenePosX, startPos.y, startPos.z);
            Vector3 distance = endPos - startPos;
            float duration = 0.4f;    // todo 临时写法
            Vector3 velocity = distance / duration;
            float startTime = Time.time;
            float elapsedTime = 0;
            
            yield return null;

            // 逐帧移动
            while (elapsedTime < duration)
            {
                elapsedTime = Time.time - startTime;
                Vector3 offset = velocity * Time.deltaTime;
                _curSceneGO.transform.localPosition = _curSceneGO.transform.localPosition + offset;
                yield return null;
            }
            
            // 预计结束时间已到，直接移动到位
            _curSceneGO.transform.localPosition = endPos;
            
        }

        
        private GameObject SetupScene(int heroTID, __HeroData heroData)
        {
            
            GameObject sceneGo;
            if (!_sceneInstances.TryGetValue(heroTID, out sceneGo))
            {
                sceneGo = Instantiate(heroData.bgSceneData.prefab);
                _sceneInstances[heroTID] = sceneGo;
                
                // 场景不存在，则其中的角色对象也不存在
                GameObject heroGo = Instantiate(heroData.prefab);
                _heroInstances[heroTID] = heroGo;
                
                // 添加英雄到场景
                Transform character_slot =  sceneGo.transform.Find("character_slot");
                if (character_slot)
                {
                    heroGo.transform.parent = character_slot;
                    heroGo.transform.localPosition = new Vector3(heroData.offset.x, heroData.offset.y, 0);;
                    heroGo.transform.localScale = new Vector3(heroData.scale.x, heroData.scale.y, 1);
                }
            }

            return sceneGo;
        }
        
        
        private bool IsAtEndOfTheList()
        {
            return _curListIndex == m_List.Count - 1;
        }

        private bool IsAtStartOfTheList()
        {
            return _curListIndex == 0;
        }

    }

}

