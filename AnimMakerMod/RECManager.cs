using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimMakerMod
{
    public class RECManager
    {
        public static RECManager Instance
        {
            get
            {
                if (_instance == null) _instance = new RECManager();
                return _instance;
            }
        }
        static RECManager _instance = null;
        public static bool isREC = false;
        public static List<RECData> datas = new List<RECData>();
        public static float REC_StartTime = 0;
        public static bool AwalysShow = false;
        public static void Clear()
        {
            foreach (var v in UnityEngine.Object.FindObjectsOfType<RECScript>()) v.Clear();
            foreach (var v in datas) v.Destroy();
            datas.Clear();
        }
        public static RECData LoadData(string name)
        {
            string p = Path.Combine(Application.dataPath, "AnimData", name + ".json");
            if (!File.Exists(p)) return null;
            RECData r = RECData.LoadData(File.ReadAllText(p));
            datas.Add(r);
            return r;
        }
        public static void LoadDataAll()
        {
            string p = Path.Combine(Application.dataPath, "AnimData");
            foreach (var v in Directory.GetFiles(p)) LoadData(Path.GetFileNameWithoutExtension(v));
        }
        public static void BeginREC()
        {
            if (!isREC)
            {
                isREC = true;
                REC_StartTime = Time.time;
            }
        }
        public static void EndREC()
        {
            isREC = false;
        }

        public static void PlayREC()
        {
            datas.RemoveAll(x => x.isDestroy);
            foreach (var v in datas)
            {
                v.Play(true);
            }
        }
        public static void StopPlay()
        {
            foreach (var v in UnityEngine.Object.FindObjectsOfType<PlayScript>())
            {
                UnityEngine.Object.Destroy(v.gameObject);
            }
        }
        public static void PlayRECInScene()
        {
            datas.RemoveAll(x => x.isDestroy);
            foreach (var v in datas)
            {

                v.Play(true).jumpToCurrentScene = true;
            }
        }

        public static void RECAll()
        {
            foreach (var v in UnityEngine.Object.FindObjectsOfType<tk2dSprite>())
            {
                if (v.gameObject.layer == (int)GlobalEnums.PhysLayers.ENEMIES |
                    v.gameObject.layer == (int)GlobalEnums.PhysLayers.ENEMY_ATTACK |
                    v.gameObject.layer == (int)GlobalEnums.PhysLayers.ENEMY_DETECTOR |
                    v.gameObject.layer == (int)GlobalEnums.PhysLayers.HERO_ATTACK |
                    v.gameObject.layer == (int)GlobalEnums.PhysLayers.HERO_BOX |
                    v.gameObject.layer == (int)GlobalEnums.PhysLayers.HERO_DETECTOR |
                    v.gameObject.layer == (int)GlobalEnums.PhysLayers.PLAYER
                    )
                {
                    if (v.GetComponent<RECScript>() == null && v.GetComponent<PlayScript>() == null)
                    {
                        RECScript r = v.gameObject.AddComponent<RECScript>();
                        r.isTemp = true;
                        r.AddOnSpawn();
                        r.AddToChildren();
                    }
                }
            }
        }
    }
}
