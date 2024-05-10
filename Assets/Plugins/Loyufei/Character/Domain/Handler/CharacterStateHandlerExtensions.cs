using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character
{
    public static class CharacterStateHandlerExtensions
    {
        public static void CharacterStatesCreate(this CharacterStateHandler self, CharacterStateCreate create)
        {
            self.CharacterStatesCreate(create.Mark.GuidHash, create.Mark.CharacterID, create.Bindings);
        }

        public static void CharacterStatesRelease(this CharacterStateHandler self, CharacterStateRelease create)
        {
            self.CharacterStatesRelease(create.Mark.GuidHash);
        }
    }
}
