using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;

namespace FightingGame.PlayerControl.Sample
{
    public class SampleOperation
    {
        public SampleOperation(CharacterService service) 
        {
            Service = service;
        }

        public CharacterService Service { get; }
        public PlayerControl2D  Player  { get; protected set; }

        private Action<VariableResponse> _Plus;

        public event Action<VariableResponse> PlusCallBack
        {
            add    => _Plus += value;

            remove => _Plus -= value;
        }

        private Action<VariableResponse> _Minus;

        public event Action<VariableResponse> MinusCallBack
        {
            add    => _Minus += value;

            remove => _Minus -= value;
        }

        public PlayerControl2D CreatePlayer() 
        {
            var position   = new Vector3(0, -3, 0);
            var layerMask  = LayerMask.NameToLayer("Player");
            var controller = Service
                .CreateController<PlayerControl2D>("SwordMan", layerMask, position, Quaternion.identity, "Player");

            Player = controller;

            return Player;
        }

        public void BindCamera() 
        {
            
        }

        public void Plus(StatVariable variable) 
        {
            Service.CalculatStatIncrease(Player.Mark, variable, _Plus);
        }

        public void Minus(StatVariable variable)
        {
            Service.CalculatStatDecrease(Player.Mark, variable, _Minus);
        }

        public void Fetch(string statId, Action<VariableResponse> onResponse) 
        {
            Service.StatFetch(Player.Mark, statId, onResponse);
        }
    }
}