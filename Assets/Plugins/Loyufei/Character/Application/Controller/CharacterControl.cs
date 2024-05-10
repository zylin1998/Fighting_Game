using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public interface IMove 
    {
        public Vector2 Direction { get; set; }
    }

    public interface IJump 
    {
        public bool Jump { get; set; }
    }

    public interface ISprint 
    {
        public bool Sprint { get; set; }
    }

    public interface IDash 
    {
        public bool Dash { get; set; }
    }

    public interface IHurt 
    {
        public bool Hurt { get; set; }
    }

    public interface IAttack 
    {
        public bool Attack { get; set; }
    }

    public interface GroundCheck
    {
        public bool IsGround { get; set; }
    }
}