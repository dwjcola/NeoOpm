using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace ProHA
{
    public class HeroSceneComponent : GameFrameworkComponent
    {
        public HeroSceneMgr _heroSceneMgr;
        private Camera _mainCam;

        public void Open()
        {
            if (_heroSceneMgr == null)
            {
                _heroSceneMgr = GetComponentInChildren<HeroSceneMgr>();
            }
            
            _heroSceneMgr.gameObject.SetActive(true);
            _mainCam = Camera.main;
            _mainCam.gameObject.SetActive(false);
            
            // HeroSceneMgr.LoadHeroScene();
            
        }

        public void Close()
        {
            _heroSceneMgr.OnQuit();
            _heroSceneMgr.gameObject.SetActive(false);
            
            _mainCam.gameObject.SetActive(true);
            LC.SendEvent("Set_Back_Music");
        }
    }
}
