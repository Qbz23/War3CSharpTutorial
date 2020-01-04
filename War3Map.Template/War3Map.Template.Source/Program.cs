using static War3Api.Common;

namespace War3Map.Template.Source
{
    internal static class Program
    {
        internal class Helpers
        {
            public static void DebugPrint(string s)
            {
            #if DEBUG
                DisplayTextToPlayer(GetLocalPlayer(), 0, 0, s);
            #endif
            }
        }

        static bool spellCondition()
        {
            // Get SpellAbilityId returns the id of the spell that was cast to activate the trigger
            return GetSpellAbilityId() == FourCC("A000");
        }

        static void spellActions()
        {
            Helpers.DebugPrint("My spell was cast!");
        }

        private static void Main()
        {
            // Disable Fog
            FogEnable(false);
            FogMaskEnable(false);

            // get id of custom unit                                 
            int customUnitId = FourCC("O000");
            // save spawned unit into a variable
            unit myUnit = CreateUnit(GetLocalPlayer(), customUnitId, 0.0f, 0.0f, 0.0f);
            // get id of custom ability 
            int customSpellId = FourCC("A000");
            // give custom ability to spawned unit
            UnitAddAbility(myUnit, customSpellId);

            // Create the trigger 
            trigger spellTrigger = CreateTrigger();
            // Register the event that activates the trigger
            TriggerRegisterPlayerUnitEvent(spellTrigger, GetLocalPlayer(),
                                           EVENT_PLAYER_UNIT_SPELL_EFFECT, null);
            TriggerAddCondition(spellTrigger, Condition(spellCondition));
            TriggerAddAction(spellTrigger, spellActions);

            Helpers.DebugPrint("Hello War3 C#!");
        }
    }
}