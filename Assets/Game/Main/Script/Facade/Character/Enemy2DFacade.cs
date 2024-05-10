using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.Character;

namespace FightingGame.Character
{
    public class Enemy2DFacade : MonoBehaviour, ICharacter2DFacade, IEnemyFacade
    {
        [Header("�~�[����")]
        [SerializeField]
        private Animator    _Animator;
        [SerializeField]
        private Rigidbody2D _Rigidbody;
        [SerializeField]
        private Transform   _CameraFocus;

        public Animator    Animator    => _Animator;
        public Rigidbody2D Rigidbody   => _Rigidbody;
        public Transform   CameraFocus => _CameraFocus;
        public Mark        Mark        { get; set; }
        public Transform   Target      { get; set; }
    }
}