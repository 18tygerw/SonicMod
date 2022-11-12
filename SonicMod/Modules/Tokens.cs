using R2API;
using System;

namespace SonicMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Sonic
            string prefix = SonicPlugin.DEVELOPER_PREFIX + "_SONIC_BODY_";

            string desc = "Sonic uses his mobility to his advantage, dealing heavy blows at mach speeds.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Homing attacks send Sonic flying towards enemies, great for retaining momentum and serving as powerful anti air." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sonic Combo unleashes a flurry of strikes and kicks to deal with foes quickly. " + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Boost through hordes of foes with lightning speed." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Stomp can be used to destroy enemies below or rush down to the ground quickly." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bounce Pad lets you reach great heights." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, escaping from the city.";
            string outroFailure = "..and so he vanished, forever falling in a lost world.";

            LanguageAPI.Add(prefix + "NAME", "Sonic the Hedgehog");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Blue Blur");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Homing Attack");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Fly into enemies for <style=cIsDamage>200% damage</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_STRIKE_NAME", "Sonic Combo");
            LanguageAPI.Add(prefix + "PRIMARY_STRIKE_DESCRIPTION", Helpers.agilePrefix + $"Strike 4 times for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_DASH_NAME", "Boost");
            LanguageAPI.Add(prefix + "SECONDARY_DASH_DESCRIPTION", Helpers.agilePrefix + Helpers.heavyPrefix + $"Dash through enemies for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_MOVE_NAME", "Stomp");
            LanguageAPI.Add(prefix + "UTILITY_MOVE_DESCRIPTION", "Crash downwards, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_MOVE_NAME", "Bounce Pad");
            LanguageAPI.Add(prefix + "SPECIAL_MOVE_DESCRIPTION", $"Spring upwards for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Sonic: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Sonic, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Sonic: Mastery");
            #endregion
            #endregion
        }
    }
}