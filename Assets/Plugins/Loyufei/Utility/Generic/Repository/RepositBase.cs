using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei
{
    public class RepositBase<TData> : RepositBase<string, TData> 
    {
        public RepositBase() : base()
        {

        }

        public RepositBase(string id, TData data) : base(id, data) 
        {

        }

        public override void Release()
        {
            _Identity = string.Empty;

            _Data = default;
        }

        public override object Clone()
        {
            return new RepositBase<TData>(_Identity, _Data);
        }
    }

    [Serializable]
    public class RepositBase<TId, TData> : Entity<TId, TData>, IReposit<TData>, ICloneable
    {
        #region Constructor

        public RepositBase() : this(default, default) 
        {

        }

        public RepositBase(TId identity, TData data) : base(identity, data)
        {
            
        }

        #endregion

        public virtual void Preserve(TData data)
        {
            _Data = data;
        }

        public virtual void SetIdentify(object identify)
        {
            _Identity = identify.To<TId>();
            _Data     = default;
        }

        public virtual void SetIdentify(IIdentity identify)
        {
            SetIdentify(identify.Identity);
        }

        public virtual void Release() 
        {
            _Identity = default;

            _Data = default;
        }

        public virtual object Clone() 
        {
            return new RepositBase<TId, TData>(_Identity, _Data);
        }
    }
}
