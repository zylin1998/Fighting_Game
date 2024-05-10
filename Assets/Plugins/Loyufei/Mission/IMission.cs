using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Mission
{
    public interface IMission : IIdentity, IDescribe
    {
        public float NormalizedProgress { get; }

        public EMissionState Checking(object pending);
    }

    public interface IMission<TPending> : IMission
    {
        public EMissionState Checking(TPending pending);

        EMissionState IMission.Checking(object pending)
            => pending is TPending p ? Checking(p) : EMissionState.None;
    }

    [Serializable]
    public abstract class MissionBase<TPending> : IMission<TPending>
    {
        [SerializeField]
        private string _Identity;

        public object Identity => _Identity;

        public abstract string Describe { get; }

        public abstract float NormalizedProgress { get; }

        public abstract EMissionState Checking(TPending pending);
    }

    [Serializable]
    public class MissionReposit : RepositBase<EMissionState> 
    {

        public MissionReposit(string identity) 
            : this(identity, EMissionState.Progress)
        {

        }

        public MissionReposit(string identity, EMissionState data) 
            : base(identity, data) 
        {

        }
    }

    [Serializable]
    public enum EMissionState 
    {
        None     = 0,
        Progress = 1,
        Complete = 2,
        End      = 3,
    }
}