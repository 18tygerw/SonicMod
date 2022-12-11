using System;

namespace SonicMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "Sonic uses his mobility to his advantage, dealing heavy blows at mach speeds.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Homing attacks send Sonic flying towards enemies, great for retaining momentum and serving as powerful anti air." + Environment.NewLine + Environment.NewLine
             + "< ! > Sonic Combo unleashes a flurry of strikes and kicks to deal with foes quickly." + Environment.NewLine + Environment.NewLine
             + "< ! > Boost through hordes of foes with lightning speed." + Environment.NewLine + Environment.NewLine
             + "< ! > Stomp can be used to destroy enemies below or rush down to the ground quickly." + Environment.NewLine + Environment.NewLine
             + "< ! > Bounce Pad lets you reach great heights." + Environment.NewLine + Environment.NewLine;

        internal const float comboDamageCoefficient = 2.1f;

        internal const float comboFinisherCoefficient = 3.2f;

        internal const float boostDamageCoefficient = 5f;

        internal const float bombDamageCoefficient = 16f;
    }
}