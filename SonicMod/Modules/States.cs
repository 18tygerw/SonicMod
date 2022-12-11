using SonicMod.SkillStates;
using SonicMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace SonicMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));

            Modules.Content.AddEntityState(typeof(BaseBoostAttack));

            Modules.Content.AddEntityState(typeof(SonicCombo));

            Modules.Content.AddEntityState(typeof(BoostWIP));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}