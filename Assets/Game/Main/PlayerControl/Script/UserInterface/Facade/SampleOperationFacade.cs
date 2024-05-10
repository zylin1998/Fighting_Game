using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei;
using Loyufei.Character;

namespace FightingGame.PlayerControl.Sample
{
    public class SampleOperationFacade : MonoBehaviour, IEnumerable<StatSlotFacade>
    {
        public Dictionary<object, StatSlotFacade> SlotFacades { get; } = new();

        [Inject]
        public void CreateSlots(FacadeFactory factory) 
        {
            StatNames.AllNames.ForEach(n =>
            {
                SlotFacades.GetorAdd(n, () =>
                {
                    var slot = factory.Create<StatSlotFacade>();

                    slot.Variable = new(n, 1f);
                    slot.transform.SetParent(transform);
                    slot.Name.SetText(n);
                    slot.Value.SetText("0");

                    return slot;
                });
            });
        }

        public void UpdateSlot(VariableResponse response) 
        {
            var slot = SlotFacades.GetorReturn(response.StatId, () => default);

            if (slot)
            {
                slot.Value?.SetText(string.Format("{0}/{1}", response.Value, response.Standard));
            }
        }

        public IEnumerator<StatSlotFacade> GetEnumerator()
        {
            return SlotFacades.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}