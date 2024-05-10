using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.Character;

namespace FightingGame.PlayerControl
{
    public class PlayerControl2D : Loyufei.Character.CharacterController
        , IMove, IJump, ISprint, IDash, IAttack, IHurt
    {
        public PlayerControl2D() : base()
        {
            _Move   = _Bindings.GetorAdd("Move"  , () => new("Move"));
            _Jump   = _Bindings.GetorAdd("Jump"  , () => new("Jump"));
            _Sprint = _Bindings.GetorAdd("Sprint", () => new("Sprint"));
            _Dash   = _Bindings.GetorAdd("Dash"  , () => new("Dash"));
            _Attack = _Bindings.GetorAdd("Attack", () => new("Attack"));
            _Hurt   = _Bindings.GetorAdd("Hurt"  , () => new("Hurt")); 
            _Death  = _Bindings.GetorAdd("Death" , () => new("Death")); 
        }

        private RepositBinding _Move;
        private RepositBinding _Jump;
        private RepositBinding _Sprint;
        private RepositBinding _Dash;
        private RepositBinding _Attack;
        private RepositBinding _Hurt;
        private RepositBinding _Death;

        public Vector2 Direction 
        {
            get => new Vector2((float)_Move.Data, 0); 
            
            set => _Move.Preserve(value.x); 
        }
        public bool    Jump      
        { 
            get => (bool)_Jump.Data; 
            
            set => _Jump.Preserve(value); 
        }
        public bool    Sprint    
        {
            get => (bool)_Sprint.Data;

            set => _Sprint.Preserve(value);
        }
        public bool    Dash      
        {
            get => (bool)_Dash.Data;

            set => _Dash.Preserve(value);
        }
        public bool    Attack    
        {
            get => (bool)_Attack.Data;

            set => _Attack.Preserve(value);
        }
        public bool    Hurt      
        {
            get => (bool)_Hurt.Data;

            set => _Hurt.Preserve(value);
        }
    }
}