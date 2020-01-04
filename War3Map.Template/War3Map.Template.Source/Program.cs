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

            //
            // Set up custom unit
            //
            // get id of custom unit                                 
            int customUnitId = FourCC("O000");
            // save spawned unit into a variable
            unit myUnit = CreateUnit(GetLocalPlayer(), customUnitId, 0.0f, 0.0f, 0.0f);
            // get id of custom ability 
            int customSpellId = FourCC("A000");
            // give custom ability to spawned unit
            UnitAddAbility(myUnit, customSpellId);

            //
            // Set up spell trigger
            //
            // Create the trigger 
            trigger spellTrigger = CreateTrigger();
            // Register the event that activates the trigger
            TriggerRegisterPlayerUnitEvent(spellTrigger, GetLocalPlayer(),
                                           EVENT_PLAYER_UNIT_SPELL_EFFECT, null);
            TriggerAddCondition(spellTrigger, Condition(spellCondition));
            TriggerAddAction(spellTrigger, spellActions);

            //
            // Spawn a circle of wisps to attack 
            //
            // The player that will own the spawned units
            player neutralAggressive = Player(PLAYER_NEUTRAL_AGGRESSIVE);
            // How many wisps to spawn 
            const uint kNumWisps = 12;
            // The radius of the circle to spawn wisps in 
            const float circleRadius = 500.0f;
            // The angle between each wisp 
            const float angleDelta = (2.0f * 3.1415f) / kNumWisps;
            // A wisp's unitId 
            int wispId = FourCC("ewsp");
            // for each wisp we want to spawn...
            for (uint i = 0; i < kNumWisps; ++i)
            {
                // Calculate position in the circle
                float x = circleRadius * Cos(angleDelta * i);
                float y = circleRadius * Sin(angleDelta * i);

                // Spawn wisp 
                CreateUnit(neutralAggressive, wispId, x, y, 0.0f);
            }

            Helpers.DebugPrint("Hello War3 C#!");
        }
    }
}