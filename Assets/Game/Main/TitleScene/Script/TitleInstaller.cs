using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei;
using FightingGame.ViewManagement;

namespace FightingGame.TitleScene
{
    public class TitleInstaller : MonoInstaller
    {
        [SerializeField]
        private QuestInfoAsset _QuestInfoAsset;

        [SerializeField]
        private List<BindableAsset> _BindableAssets;

        public override void InstallBindings()
        {
            Container
                .Bind<ViewManager>()
                .AsSingle();

            Container
                .Bind<IEntityForm>()
                .WithId("Quest")
                .FromInstance(_QuestInfoAsset)
                .AsSingle();

            _BindableAssets.ForEach(asset => asset.BindToContainer(Container));
        }
    }
}