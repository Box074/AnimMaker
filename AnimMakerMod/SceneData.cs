using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimMakerMod
{
    [Serializable]
    public class SceneData
    {
        public List<ActorData> actorDatas = new List<ActorData>();
    }
    [Serializable]
    public class ActorData
    {
        public string name = "";
        public float centerX = 0;
        public float centerY = 0;
        public float initX = 0;
        public float initY = 0;
        public float startTime = 0;
        private float _startTime = 0;
        [NonSerialized]
        public RECData data = null;

        public void Parse()
        {
            data = RECManager.LoadData(name);
            if(startTime > data.FrameDatas[0].time)
            {
                _startTime = data.FrameDatas[0].time;
            }
            else
            {
                _startTime = startTime;
            }
            foreach(var v in data.FrameDatas)
            {
                v.posX = v.posX - centerX + initX;
                v.posY = v.posY - centerY + initY;
                v.time = v.time + _startTime;
            }
        }
    }
}
