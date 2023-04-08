using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

namespace FightingGameDemo
{
    [CreateAssetMenu(fileName = "Ignore Collision Action", menuName = "Event/Game Action/Ignore Collision Action", order = 2)]
    public class IgnoreCollisionAction : GameAction
    {
        [SerializeField]
        private List<Pair> _Ignores;

        public List<Pair> Ignores => this._Ignores;

        public override void Initialize() 
        {
            this._Ignores.ForEach(i => i.IgnoreCollision());
        }

        public override void Invoke<TVariable>(TVariable role)
        {
            
        }

        [System.Serializable]
        public struct Pair 
        {
            [SerializeField]
            private string _Collision1;
            [SerializeField]
            private string _Collision2;

            public string Collision1 => this._Collision1;
            public string Collision => this._Collision2;

            public void IgnoreCollision() 
            {
                var colli1 = LayerMask.NameToLayer(this._Collision1);
                var colli2 = LayerMask.NameToLayer(this._Collision2);

                Physics2D.IgnoreLayerCollision(colli1, colli2);
            }
        }
    }
}