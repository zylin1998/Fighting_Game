using Loyufei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FightingGame.Player;

namespace FightingGame
{
    public class DataTransferStation : BindableAsset
    {
        [SerializeField]
        private PlayerSave _PlayerSave;

        [SerializeField]
        private int _QuestId;

        public int QuestId { get => _QuestId; set=> _QuestId = value; }

        public override void BindToContainer(DiContainer container, object group = null)
        {
            container
                .Bind<SaveSystem>()
                .WithId("Player")
                .FromInstance(_PlayerSave).
                AsCached();

            var data = _PlayerSave.Saveable.To<PlayerData>();

            container
                .Bind<IRepository>()
                .WithId("Inventory")
                .FromInstance(data._Inventory)
                .AsCached();

            container
                .Bind<IRepository>()
                .WithId("Quest")
                .FromInstance(data._Quest)
                .AsCached();

            container
                .Bind<int>()
                .WithId("QuestId")
                .FromInstance(_QuestId)
                .AsCached();
        }
    }
}