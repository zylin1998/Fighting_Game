using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.Role 
{
    public interface IRoleDetail
    {

    }


    public interface IDetailCollect
    {
        public IEnumerable<IRoleDetail> DetailCollect { get; }

        public TDetail GetDetail<TDetail>()
        {
            foreach (IRoleDetail detail in this.DetailCollect)
            {
                if (detail is TDetail result) { return result; }
            }

            return default(TDetail);
        }
    }
    public interface ILevel
    {
        public int Level { get; }
        public float Exp { get; }
    }

    public interface ILevelLimit
    {
        public int MaxLevel { get; }
        public float MaxExp { get; }
        public float InitExp { get; }
    }

    public interface IHealth
    {
        public float HP { get;  }
        public float MP { get;  }
        public float Damage { get;  }
        public float Defend { get;  }
    }
}
