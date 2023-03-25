using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Role;

namespace Custom.Battle
{
    public class CreateSpot : MonoBehaviour, ISpot
    {
        [SerializeField]
        private string _SpotName;
        [SerializeField]
        private IFlipAction.EFlipSide _FlipSide;

        public string SpotName => this._SpotName;
        public Vector3 Position => this.transform.position;
        public Quaternion Rotation => this.transform.rotation;
        public IFlipAction.EFlipSide FlipSide => this._FlipSide;
    }

    public interface ISpot
    {
        public string SpotName { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public IFlipAction.EFlipSide FlipSide { get; }
    } }