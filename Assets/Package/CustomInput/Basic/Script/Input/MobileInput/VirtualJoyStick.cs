using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom.InputSystem
{
    public class VirtualJoyStick : MonoBehaviour, IDragHandler, IEndDragHandler, IVJoyStick
    {
        [Header("Axes Name")]
        [SerializeField]
        private string _Horizontal = "Horizontal";
        [SerializeField]
        private string _Vertical = "Vertical";

        private Transform content;

        public float Angle { get; private set; }
        public bool IsOnDrag { get; private set; }

        private void Awake()
        {
            this.content = this.GetComponent<ScrollCircle>()?.content;
        }

        private void Start()
        {
            InputClient.GetAxes<JoyStickAxes>(this._Horizontal).SetJoyStick(this);
            InputClient.GetAxes<JoyStickAxes>(this._Vertical).SetJoyStick(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.IsOnDrag = true;

            if (this.content)
            {
                var direction = this.content.localPosition.normalized;
                
                this.Angle = Vector2.SignedAngle(Vector2.right, direction);
            }
        }

        public void OnEndDrag(PointerEventData eventData) 
        {
            this.IsOnDrag = false;
        }
    }
}