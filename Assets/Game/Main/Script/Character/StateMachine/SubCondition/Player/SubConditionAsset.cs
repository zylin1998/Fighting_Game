using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.Character;

namespace FightingGame.Character
{
    public abstract class SubConditionAsset : VisualSubConditionAsset, ISetup<CharacterStateMachine>
    {
        public abstract void Setup(CharacterStateMachine stateMachine);

        public override void Setup(params object[] args)
        {
            Setup(args.FirstOrDefault()?.To<CharacterStateMachine>());
        }
    }
}