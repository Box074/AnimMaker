using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

namespace AnimMakerMod
{
    [Serializable]
    public class RECData
    {
        [NonSerialized]
        public tk2dSpriteCollectionData SpriteLibrary = null;
        public string Name = "";
        public string SLGuid = "";
        public string SLCGuid = "";
        
        public List<FrameData> FrameDatas = new List<FrameData>();
        public float StartTime = -1;
        [NonSerialized]
        public bool isDestroy = false;
        public void Destroy() => isDestroy = true;
        public PlayScript Play(bool destroyOnFinished = false)
        {
            
            GameObject player = new GameObject("REC Player");
            PlayScript pl = player.AddComponent<PlayScript>();
            pl.Play(this, destroyOnFinished);
            return pl;
        }
        public static RECData LoadData(string data)
        {
            RECData r = JsonConvert.DeserializeObject<RECData>(data);
            r.SpriteLibrary = Resources.FindObjectsOfTypeAll<tk2dSpriteCollectionData>()
                .FirstOrDefault(x => x.dataGuid == r.SLGuid || x.spriteCollectionGUID == r.SLCGuid);
            return r;
        }
        public string SaveDate()
        {
            SLCGuid = SpriteLibrary.spriteCollectionGUID;
            SLGuid = SpriteLibrary.dataGuid;
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        public void Save(string name)
        {
            string p = Path.Combine(Application.dataPath, "AnimData");
            if (!Directory.Exists(p)) Directory.CreateDirectory(p);
            File.WriteAllText(Path.Combine(p, name + ".json"), SaveDate());
        }
    }
    [Serializable]
    public class FrameData
    {
        public string SceneName = "";
        public int spriteId = 0;
        public float scaleX = 1;
        public float scaleY = 1;
        public float posX = 0;
        public float posY = 0;
        public float posZ = 0;
        public float rotX = 0;
        public float rotY = 0;
        public float rotZ = 0;
        public float rotW = 0;

        public float cr = 0;
        public float cg = 0;
        public float cb = 0;
        public float ca = 0;

        public bool meshEnable = false;

        public float time = 0;
    }
}
