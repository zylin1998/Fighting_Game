using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Loyufei;
using Loyufei.Character;

namespace FightingGame.PlayerControl.Sample
{
    public class StatSlotFacade : MonoBehaviour, IFacade
    {
        [SerializeField]
        private TextMeshProUGUI _Name;
        [SerializeField]
        private TextMeshProUGUI _Value;
        [SerializeField]
        private Button          _Plus;
        [SerializeField]
        private Button          _Minus;

        public TextMeshProUGUI Name  => _Name;
        public TextMeshProUGUI Value => _Value;
        public Button          Plus  => _Plus;
        public Button          Minus => _Minus;

        public StatVariable Variable { get; set; }
    }
}