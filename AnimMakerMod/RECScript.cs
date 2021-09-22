using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimMakerMod
{
    public class RECScript : MonoBehaviour
    {
        static float frame_time = 1f / 60f;
        public static int FPS
        {
            get
            {
                return (int)(1f / frame_time);
            }
            set
            {
                if(value == 0)
                {
                    frame_time = 1f / 60;
                    return;
                }
                frame_time = 1f / value;
                if (frame_time == 0) frame_time = 1f / 60f;
            }
        }
        public static RECManager RECManager => RECManager.Instance;
        public List<RECData> datas = new List<RECData>();
        public RECData Data = null;
        public tk2dSprite sprite = null;
        public MeshRenderer renderer = null;
        public bool isTemp = false;
        private Color _color = new Color();
        public void Clear()
        {
            foreach(var v in datas)
            {
                v.Destroy();
            }
            RECManager.datas.RemoveAll(x => x.isDestroy);
            datas.Clear();
        }
        public void AddToChildren()
        {
            void Add(GameObject go)
            {
                if (go.GetComponent<RECScript>() == null) go.AddComponent<RECScript>().isTemp = isTemp;
                for(int i = 0; i < go.transform.childCount; i++)
                {
                    Add(go.transform.GetChild(i).gameObject);
                }
            }
            Add(gameObject);
        }
        public void AddOnSpawn()
        {
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter += SpawnFromPool_OnEnter;
        }

        private void SpawnFromPool_OnEnter(On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.orig_OnEnter orig,
            HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool self)
        {
            orig(self);
            if(self.Fsm.GameObject.transform.IsChildOf(transform) || self.Fsm.GameObject == gameObject && RECManager.isREC)
            {
                GameObject go = Instantiate(self.storeObject.Value);
                RECScript rs = go.AddComponent<RECScript>();
                rs.AddToChildren();
                rs.isTemp = true;
                self.storeObject.Value.Recycle();
                self.storeObject.Value = go;
            }
        }

        void Awake()
        {

        }
        void OnEnable()
        {
            sprite = GetComponent<tk2dSprite>();
            
            renderer = GetComponent<MeshRenderer>();
            if (sprite == null || renderer == null) Destroy(this);
        }

        float lastFrame = 0;
        void Update()
        {
            if (!RECManager.isREC) lastFrame = 0;
            if(!RECManager.isREC && isTemp)
            {
                Destroy(this);
                return;
            }
            if (Time.time - lastFrame < frame_time) return;
            lastFrame = Time.time;
            RECFrame();
        }
        void OnDisable()
        {
            if (RECManager.isREC && Data != null)
            {
                Data.FrameDatas.Add(new FrameData()
                {
                    meshEnable = false
                });
            }
        }
        void OnDestroy()
        {
            sprite.color = Color.white;
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter -= SpawnFromPool_OnEnter;
            datas.Clear();
        }
        void RECFrame()
        {
            if (!RECManager.isREC)
            {
                if (Data != null)
                {
                    Data = null;
                    sprite.color = Color.white;
                }
                
                return;
            }
            if (sprite.color != Color.blue)
            {
                _color = sprite.color;
            }
            sprite.color = Color.blue;
            if (Data == null)
            {
                Data = new RECData();
                RECManager.datas.Add(Data);
                RECManager.datas.RemoveAll(x => x.isDestroy);
                datas.RemoveAll(x => x.isDestroy);
                datas.Add(Data);
                
            }
            if (Data.SpriteLibrary == null)
            {
                Data.SpriteLibrary = sprite.Collection;
            }
            if (Data.StartTime == -1) Data.StartTime = RECManager.REC_StartTime;

            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            FrameData frame = new FrameData
            {
                SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                time = Time.time - RECManager.REC_StartTime,

                posX = pos.x,
                posY = pos.y,
                posZ = pos.z,
                scaleX = GetSX(transform),
                scaleY = GetSY(transform),

                rotX = rot.x,
                rotY = rot.y,
                rotZ = rot.z,
                rotW = rot.w,

                cr = _color.r,
                cg = _color.g,
                cb = _color.b,
                ca = _color.a,

                meshEnable = renderer.enabled,
                spriteId = sprite.spriteId
            };


            Data.FrameDatas.Add(frame);
        }

        float GetSX(Transform t)
        {
            float r = t.localScale.x;
            while((t = t.parent) != null)
            {
                r *= t.localScale.x;
            }
            return r;
        }
        float GetSY(Transform t)
        {
            float r = t.localScale.y;
            while ((t = t.parent) != null)
            {
                r *= t.localScale.y;
            }
            return r;
        }
    }
}