using System;
using System.Collections;
using System.Collections.Generic;
// using Sirenix.OdinInspector;
using UnityEngine;

namespace NeoOPM
{
    public class CUIListObjsTool : MonoBehaviour
    {

        [Serializable]
        public class myClass
        {
            // [OnValueChanged ( "OnValueChange" )]
            // [ValueDropdown("ComponentsValues")]
            public Component componetType;

            private Component[] ComponentsValues;


            // [OnValueChanged ( "OnValueChange" )]
            public Component[] gameObjects;

            private void OnValueChange()
            {
                if (gameObjects.Length < 1)
                {
                    return;
                }

                if (gameObjects[0] == null)
                {
                    return;
                }

                ComponentsValues = gameObjects[0].GetComponents<Component>();
                if (componetType == null)
                {
                    componetType = ComponentsValues[0];
                }

                for (int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i] = gameObjects[i].GetComponent(componetType.GetType());
                }
            }

            // [Button ( "清除", ButtonSizes.Medium )]
            public void Clear()
            {
                gameObjects = new Component[] { };
                ComponentsValues = new Component[] { };
                componetType = null;
            }
        }

        public myClass[] myarrays;

        // [Button ( "清除", ButtonSizes.Medium )]
        public void Clear()
        {
            for (int i = 0; i < myarrays.Length; i++)
            {
                myarrays[i].Clear();
            }

            myarrays = null;
        }
    }
}