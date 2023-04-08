using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Custom.Navigation;

namespace Custom.InputSystem
{
    [RequireComponent(typeof(EventCollect))]
    public class MobileButton : MonoBehaviour, IMobileButton
    {
        [SerializeField]
        private string _AxesName;

        public string AxesName => this._AxesName;

        public IEventCollect EventCollect { get; private set; }
        public bool GetKeyDown { get; private set; }
        public bool GetKey { get; private set; }
        public bool GetKeyUp { get; private set; }

        public void Initialize()
        {
            
        }

        private void Awake()
        {
            this.EventCollect = this.GetComponent<IEventCollect>();
        }

        private void Start()
        {
            this.EventCollect.AddEvent(EventTriggerType.PointerDown, (data) => OnPointerDown(data));
            this.EventCollect.AddEvent(EventTriggerType.PointerUp, (data) => OnPointerUp(data));

            InputClient.GetAxes<ButtonAxes>(this._AxesName).SetButton(this);
        }

        private void OnPointerDown(BaseEventData data)
        {
            this.GetKeyDown = true;

            IEnumerator GetKey()
            {
                yield return new WaitForEndOfFrame();

                this.GetKeyDown = false;
                this.GetKey = true;
            }

            StartCoroutine(GetKey());
        }

        private void OnPointerUp(BaseEventData data)
        {
            this.GetKey = false;
            this.GetKeyUp = true;

            IEnumerator GetKeyUp()
            {
                yield return new WaitForEndOfFrame();

                this.GetKeyUp = false;
            }

            StartCoroutine(GetKeyUp());
        }
    }
}