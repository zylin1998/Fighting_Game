using System.Collections;
using System.Collections.Generic;

namespace Loyufei.Character 
{
    public static class CharacterFacadeHandlerExtensions
    {
        public static void Spawn(this CharacterFacadeHandler self, CharacterFacadeSpawn spawn)
        {
            var parent = spawn.Option.Parent;
            var position = spawn.Option.Position;
            var rotation = spawn.Option.Rotation;

            self.Spawn(spawn.Mark, parent, position, rotation);
        }

        public static void Despawn(this CharacterFacadeHandler self, CharacterFacadeDespawn despawn)
        {
            self.Despawn(despawn.Mark);
        }
    }
}
