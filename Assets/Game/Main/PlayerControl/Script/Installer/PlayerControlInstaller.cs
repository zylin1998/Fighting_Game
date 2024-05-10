using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei;
using Loyufei.Character;
using Loyufei.DomainEvents;

namespace FightingGame.PlayerControl.Sample
{
    public class PlayerControlInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _RepositSlotFacade;

        public override void InstallBindings()
        {
            BindUI();
            BindPlayer();
            BindEvent();
        }

        private void BindUI() 
        {
            SignalBusInstaller.Install(Container);

            var type = _RepositSlotFacade.GetComponent<IFacade>().GetType();
            
            Container.BindFactory<IFacade, Loyufei.Factory<Type, IFacade>>()
                .WithFactoryArguments(type)
                .To(type)
                .FromComponentInNewPrefab(_RepositSlotFacade);

            Container.BindFactory<Type, IFacade, FacadeFactory>()
                .FromFactory<Factories<Type, IFacade>>();

            Container
                .BindInterfacesAndSelfTo<SampleOperationUI>()
                .AsSingle()
                .NonLazy();
        }

        private void BindPlayer()
        {
            Container
                .BindInterfacesAndSelfTo<SampleOperation>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<CharacterService>()
                .AsSingle();
        }

        private void BindEvent()
        {
            Container
                .Bind<DomainEventService>()
                .AsSingle() 
                .NonLazy();
        }
    }
}