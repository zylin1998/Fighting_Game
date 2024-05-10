using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [Serializable]
    public class QuestInfo : IEnumerable<EnemySpawn>
    {
        [SerializeField, Min(1f)]
        private float                _LimitTime;
        [SerializeField]
        private float                _SpawnDuration;
        [SerializeField]
        private List<Vector3>        _SpawnPositions;
        [SerializeField]
        private List<EnemyInfo>      _EnemyInfos;
        [SerializeField]
        private List<EnemySpawnInfo> _EnemySpawnInfos;

        public IEnumerator<EnemySpawn> GetEnumerator() => _EnemySpawnInfos
            .ConvertAll((info) => new EnemySpawn(_EnemyInfos[info.Enemy], _SpawnPositions[info.Position]))
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class EnemySpawn 
    {
        public EnemySpawn(EnemyInfo info, Vector3 position) 
        {
            Info     = info;
            Position = position;
        }

        public EnemyInfo Info     { get; }
        public Vector3   Position { get; }
    }

    [Serializable]
    public class EnemyInfo 
    {
        [SerializeField, Min(1f)]
        private float _Health;
        [SerializeField, Min(1f)]
        private float _WalkSpeed;
        [SerializeField, Min(1f)]
        private float _RunSpeed;
        [SerializeField, Min(1f)]
        private float _Damage;
        [SerializeField]
        private int   _Score;

        public float Health    => _Health;
        public float WalkSpeed => _WalkSpeed;   
        public float RunSpeed  => _RunSpeed;
        public float Damage    => _Damage;
        public int   Score     => _Score;
    }

    [Serializable]
    public class EnemySpawnInfo 
    {
        [SerializeField]
        private int _Enemy;
        [SerializeField]
        private int _Position;

        public int Enemy    => _Enemy;
        public int Position => _Position;
    }
}