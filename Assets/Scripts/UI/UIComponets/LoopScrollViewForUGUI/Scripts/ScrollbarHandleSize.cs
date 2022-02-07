using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// explain：循环列表组件 UGUI版
/// author：王云飞
/// email:1172906928@qq.com
/// </summary>
namespace lufeigame {

    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollbarHandleSize : UIBehaviour
    {
        public float        maxSize = 1.0f;
        public float        minSize = 0.1f;

        private ScrollRect  scrollRect;

        protected override void Awake() {
            base.Awake();
            this.scrollRect = this.GetComponent<ScrollRect>();
        }

        protected override void OnEnable() {
            this.scrollRect.onValueChanged.AddListener( this.onValueChanged );
        }


        protected override void OnDisable() {
            this.scrollRect.onValueChanged.RemoveListener( this.onValueChanged );
        }


        public void onValueChanged( Vector2 value ) {

            var hScrollbar = this.scrollRect.horizontalScrollbar;
            if( hScrollbar!=null ) {
                if( hScrollbar.size > this.maxSize ) {
                    hScrollbar.size = this.maxSize;
                }
                else
                if( hScrollbar.size < this.minSize ) {
                    hScrollbar.size = this.minSize;
                }
            }

            var vScrollbar = this.scrollRect.verticalScrollbar;
            if( vScrollbar!=null ) {
                if( vScrollbar.size > this.maxSize ) {
                    vScrollbar.size = this.maxSize;
                }
                else
                if( vScrollbar.size < this.minSize ) {
                    vScrollbar.size = this.minSize;
                }
            }
        }
    }
}