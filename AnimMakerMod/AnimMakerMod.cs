using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;
using UnityEngine;

namespace AnimMakerMod
{
    public class AnimMakerMod : Mod
    {
        public static GameObject HUD = null;
        public override void Initialize()
        {
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
        }

        public static void HideHUD()
        {
            if (HUD != null)
            {
                FSMUtility.SendEventToGameObject(HUD, "OUT");
            }
        }
        public static void ShowHUD()
        {
            if (HUD != null)
            {
                FSMUtility.SendEventToGameObject(HUD, "IN");
            }
        }
        private void ModHooks_HeroUpdateHook()
        {
            if (HUD == null)
            {
                HUD = GameObject.Find("Hud Canvas");
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                HideHUD();
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                ShowHUD();
            }
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                RECManager.PlayREC();
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                RECManager.StopPlay();
            }
            if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            {
                RECManager.PlayRECInScene();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (!RECManager.isREC)
                {
                    RECManager.BeginREC();
                }
                else
                {
                    RECManager.EndREC();
                }
            }
            if (RECManager.isREC)
            {
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    RECManager.RECAll();
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                RECManager.Clear();
            }
        }
    }
}
