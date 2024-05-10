using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei 
{
    public class RepositBinding
    {
        public RepositBinding(object identity) : this(identity, new IReposit[0])
        {

        }

        public RepositBinding(object identity, IEnumerable<IReposit> reposits)
        {
            Identity = identity;
            Reposits = reposits.ToList();
        }

        public object Identity { get; protected set; }
        public object Data { get; protected set; }
        public List<IReposit> Reposits { get; }

        public void Preserve(object data)
        {
            Data = data;

            Reposits.ForEach(r => r.Preserve(Data));
        }

        public void Bind(IReposit reposit)
        {
            Reposits.Add(reposit);
        }

        public void UnBind()
        {
            Reposits.Clear();
        }
    }
}
