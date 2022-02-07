using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// explain：循环列表组件 UGUI版
/// author：王云飞
/// email:1172906928@qq.com
/// </summary>
/// 
namespace lufeigame
{
    public class VScrollViewApi : MonoBehaviour
    {

        public Button bt_loadList;
        public Button bt_loadLis2;
        public Button bt_loadList3;
        public Button bt_jump1;
        public Button bt_jump2;
        public Button bt_changeItem;
        public Button bt_changeItem2;
        public Button bt_jump;
        public InputField index_input;
        public lufeigame.VScrollViewLoop scrollView;
        public RectTransform prefabItem1;
        private void Awake()
        {
            bt_loadList.onClick.AddListener(OnLoadList);
            bt_loadLis2.onClick.AddListener(OnLoadList2);
            bt_loadList3.onClick.AddListener(OnLoadList3);
            bt_jump1.onClick.AddListener(OnJump1);
            bt_jump2.onClick.AddListener(OnJump2);
            bt_changeItem.onClick.AddListener(OnChangeItem);
            bt_changeItem2.onClick.AddListener(OnChangeItem2);
            bt_jump.onClick.AddListener(OnJumIndex);
          
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnLoadList()
        {
            scrollView.SetColumn(13);
            scrollView.SetPadding(30, 0, 10, 10);
            scrollView.SetCellSieze(new Vector2(100, 100));
            scrollView.SetSpacing(new Vector2(10, 10));
            scrollView.SetItemPrefab(prefabItem1);
            scrollView.SetCount(1000);
            scrollView.DestroyAllItems(); //如果不切换Item  不需要全部清除Item
            scrollView.ReLoad();
        }
        private void OnLoadList2()
        {
            scrollView.SetColumn(13);
            scrollView.SetPadding(30, 0, 10, 10);
            scrollView.SetCellSieze(new Vector2(100, 100));
            scrollView.SetSpacing(new Vector2(10, 10));
            scrollView.SetItemPrefab(prefabItem1);
            scrollView.SetCount(500);
            scrollView.DestroyAllItems();//如果不切换Item  不需要全部清除Item
            scrollView.ReLoad();
        }
        private void OnLoadList3()
        {
            scrollView.SetColumn(13);
            scrollView.SetPadding(30, 0, 10, 10);
            scrollView.SetCellSieze(new Vector2(100, 100));
            scrollView.SetSpacing(new Vector2(10, 10));
            scrollView.SetItemPrefab(prefabItem1);
            scrollView.SetCount(50);
            scrollView.DestroyAllItems();//如果不切换Item  不需要全部清除Item
            scrollView.ReLoad();
        }
        private void OnJump1()
        {
            scrollView.Jump(489);
        }
        private void OnJump2()
        {
            scrollView.Jump(10);
        }
        private void OnJumIndex()
        {
            if (string.IsNullOrEmpty(index_input.text))
            {
                return;
            }
            scrollView.Jump(int.Parse(index_input.text));
        }
        private void OnChangeItem()
        {
            scrollView.SetColumn(7);
            scrollView.SetPadding(30, 0, 10, 10);
            scrollView.SetCellSieze(new Vector2(200, 100));
            scrollView.SetSpacing(new Vector2(10, 10));
            GameObject item2 = Resources.Load("item2") as GameObject;
            RectTransform item2Rect = item2.GetComponent<RectTransform>();
            scrollView.SetItemPrefab(item2Rect);
            scrollView.SetCount(1000);
            scrollView.DestroyAllItems();
            scrollView.ReLoad();
        }
        private void OnChangeItem2()
        {
            scrollView.SetColumn(7);
            scrollView.SetPadding(30, 0, 10, 10);
            scrollView.SetCellSieze(new Vector2(200, 300));
            scrollView.SetSpacing(new Vector2(10, 10));
            GameObject item2 = Resources.Load("item3") as GameObject;
            RectTransform item2Rect = item2.GetComponent<RectTransform>();
            scrollView.SetItemPrefab(item2Rect);
            scrollView.SetCount(1000);
            scrollView.DestroyAllItems();
            scrollView.ReLoad();
        }
    }
}
