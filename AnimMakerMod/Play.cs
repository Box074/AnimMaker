using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimMakerMod
{
    public class PlayScript : MonoBehaviour
    {
        public RECData Data = null;
        public tk2dSprite sprite = null;
        public MeshRenderer renderer = null;
        public float StartTime = 0;
        public bool jumpToCurrentScene = false;
        void Start()
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
            sprite = gameObject.AddComponent<tk2dSprite>();
            sprite.SetSprite(Data.SpriteLibrary, 0);
        }
        public void Play(RECData data,bool df = false)
        {
            StartTime = Time.time;
            Data = data;
            StartCoroutine(PlayAnim(df));
        }
        IEnumerator PlayAnim(bool df)
        {
            yield return null;
            float nextTime = 0;
            renderer.enabled = false;
            foreach(var v in Data.FrameDatas)
            {
                if (jumpToCurrentScene)
                {
                    if (v.SceneName != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) continue;
                }
                nextTime = v.time;
                while (Time.time - StartTime < nextTime) yield return null;
                LoadFrame(v);
                yield return null;
            }
            if (df) Destroy(gameObject);
        }
        public void LoadFrame(FrameData data)
        {
            transform.position = new Vector3(
                data.posX,
                data.posY,
                data.posZ
                );
            transform.SetScaleX(data.scaleX);
            transform.SetScaleY(data.scaleY);
            Quaternion rot = new Quaternion();
            rot.x = data.rotX;
            rot.y = data.rotY;
            rot.z = data.rotZ;
            rot.w = data.rotW;
            transform.rotation = rot;

            Color c = new Color(data.cr, data.cg, data.cb, data.ca);
            sprite.color = c;

            sprite.spriteId = data.spriteId;
            if (!RECManager.AwalysShow)
            {
                renderer.enabled = data.meshEnable;
            }
            else if(!data.meshEnable)
            {

                renderer.enabled = true;
                c.a = 0.5f;
                sprite.color = c;
            }
        }
    }
}
