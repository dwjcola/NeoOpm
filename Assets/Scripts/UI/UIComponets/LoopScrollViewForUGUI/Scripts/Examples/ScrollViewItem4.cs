using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// explain：循环列表组件 UGUI版
/// author：王云飞
/// email:1172906928@qq.com
/// </summary>

namespace lufeigame
{
    public class ScrollViewItem4 : ScrollViewBaseItem
    {
        private Text text;
        public override void Awake()
        {
            base.Awake();
            text = transform.Find("Text").GetComponent<Text>();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override void UpdateData(int index)
        {
            text.text = index.ToString();
        }
        private void ChangeModel(string modelName)
        {

        }
    }

  
}
