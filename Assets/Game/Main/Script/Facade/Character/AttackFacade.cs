using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FightingGame.Character;
using FightingGame.PlayerControl;

namespace FightingGame.Character
{
    public class AttackFacade : MonoBehaviour, IAttackFacade
    {
        [SerializeField]
        private float _AwakeTime;
        [SerializeField]
        private float _SleepTime;

        public float AwakeTime => _AwakeTime;
        public float SleepTime => _SleepTime;
    }
}