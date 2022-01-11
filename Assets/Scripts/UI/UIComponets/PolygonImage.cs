using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NeoOPM
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonImage : Image
    {
        private PolygonCollider2D _pc;

        public PolygonCollider2D Pc
        {
            get
            {
                if (_pc == null)
                {
                    _pc = GetComponent<PolygonCollider2D>();
                }

                return _pc;
            }
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector3 point;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
            return Pc.OverlapPoint(point);
        }
    }
}