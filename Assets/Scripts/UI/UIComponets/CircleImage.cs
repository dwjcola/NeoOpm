using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace ProHA
{
   public class CircleImage : Image
   {
      [Range(0, 360)] //把角度限制在0-360
      public float startAngleDegree = 0;

      [Range(0, 360)] //把角度限制在0-360
      public float angleDegree = 45;

      protected override void OnPopulateMesh(VertexHelper toFill)
      {
         toFill.Clear();
         float width = rectTransform.rect.width; //矩形宽
         float height = rectTransform.rect.height; //矩形高

         Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

         float uvWidth = uv.z - uv.x;
         float uvHeight = uv.w - uv.y;
         Vector2 uvCenter = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
         float ratioX = uvWidth / width; //宽高转换比例
         float ratioY = uvHeight / height;

         Vector2 originPos =
            new Vector2((0.5f - rectTransform.pivot.x) * width, (0.5f - rectTransform.pivot.y) * height);
         Vector2 vertPos = Vector2.zero;
         Color32 colorTemp = color;
         UIVertex origin = GetUIVertex(colorTemp, originPos, vertPos, uvCenter, ratioX, ratioY);
         toFill.AddVert(origin);


         bool fin = false;
         List<float> allAngles = new List<float>();
         allAngles.Add(startAngleDegree);
         if (45f > startAngleDegree)
         {
            if (45f < angleDegree)
            {
               allAngles.Add(45f);
            }
            else
            {
               allAngles.Add(angleDegree);
               fin = true;
            }
         }

         if (!fin && 135f > startAngleDegree)
         {
            if (135f < angleDegree)
            {
               allAngles.Add(135f);
            }
            else
            {
               allAngles.Add(angleDegree);
               fin = true;
            }
         }

         if (!fin && 225f > startAngleDegree)
         {
            if (225f < angleDegree)
            {
               allAngles.Add(225f);
            }
            else
            {
               allAngles.Add(angleDegree);
               fin = true;
            }
         }

         if (!fin && 315f > startAngleDegree)
         {
            if (315f < angleDegree)
            {
               allAngles.Add(315f);
            }
            else
            {
               allAngles.Add(angleDegree);
               fin = true;
            }
         }

         if (!fin)
         {
            allAngles.Add(angleDegree);
         }

         for (int i = 0; i < allAngles.Count; i++)
         {
            Vector2 pos = CalcPoint(allAngles[i]);
            float x = pos.x * width * 0.5f;
            float y = pos.y * height * 0.5f;
            Vector2 posTermp = new Vector2(x, y);
            UIVertex temp = GetUIVertex(color, posTermp + originPos, posTermp, uvCenter, ratioX, ratioY);
            toFill.AddVert(temp);
         }

         int id = 1;
         for (int i = 0; i < allAngles.Count - 1; i++)
         {
            toFill.AddTriangle(id, 0, id + 1);
            id++;
         }

      }

      private Vector2 CalcPoint(float angle)
      {
         //这个函数计算了非特殊角度射线在正方形上的点
         angle = angle % 360;
         if (angle == 0)
         {
            return new Vector2(1, 0);
         }
         else if (angle == 180)
         {
            return new Vector2(-1, 0);
         }

         //这里分别对应这个射线处于上图哪个三角形中, 分别计算
         if (angle <= 45 || angle > 315)
         {
            return new Vector2(1, Mathf.Tan(Mathf.Deg2Rad * angle));
         }
         else if (angle <= 135)
         {
            return new Vector2(1 / Mathf.Tan(Mathf.Deg2Rad * angle), 1);
         }
         else if (angle <= 225)
         {
            return new Vector2(-1, -Mathf.Tan(Mathf.Deg2Rad * angle));
         }
         else
         {
            return new Vector2(-1 / Mathf.Tan(Mathf.Deg2Rad * angle), -1);
         }
      }

      private UIVertex GetUIVertex(Color32 col, Vector3 pos, Vector2 uvPos, Vector2 uvCenter, float scaleX,
         float scaleY)
      {
         UIVertex vertexTemp = new UIVertex();
         vertexTemp.color = col;
         vertexTemp.position = pos;
         vertexTemp.uv0 = new Vector2(uvPos.x * scaleX + uvCenter.x, uvPos.y * scaleY + uvCenter.y);
         return vertexTemp;
      }
   }
}