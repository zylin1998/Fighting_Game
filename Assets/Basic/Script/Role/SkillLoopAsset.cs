using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Battle
{
    [CreateAssetMenu(fileName = "Skill Loop", menuName = "Role Data/Skill/Skill Loop", order = 1)]
    public class SkillLoopAsset : ScriptableObject
    {
        [SerializeField]
        private float _SpaceTime;
        [SerializeField]
        private List<Loop> _Loops = new List<Loop>() { new Loop(ELoopType.Entry), new Loop(ELoopType.Loop)};

        public float SpaceTime => this._SpaceTime;
        public List<Loop> Loops => this._Loops;
        public Loop this[ELoopType loopType] => this._Loops.Find(l => l.LoopType == loopType);

        [System.Serializable]
        public enum ELoopType 
        {
            None = 0,
            Entry = 1,
            Loop = 2
        }

        [System.Serializable]
        public struct Loop
        {
            [SerializeField]
            private ELoopType _LoopType;
            [SerializeField]
            private List<string> _List;

            public ELoopType LoopType => this._LoopType;
            public IEnumerable<string> List => this._List;

            public Loop(ELoopType loopType) : this(loopType, new string[0]) 
            {

            }

            public Loop(ELoopType loopType, IEnumerable<string> list)
            {
                this._LoopType = loopType;
                this._List = new List<string>(list);
            }
        }
    }
}