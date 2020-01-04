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
            // Range around the caster that targets can be hit from 
            const float kSpellRange = 750;
            // The amount of damage each strike deals 
            const float kDamage = 250;
            // Max number of targets the spell can hit 
            const uint kMaxTargets = 6;
            // a variable to decrement each time we hit a target 
            uint count = kMaxTargets;

            // Gets the unit that cast the spell associated with  
            // this trigger and saves it into a variable 
            unit caster = GetSpellAbilityUnit();
            // Gets the location of the caster 
            float startX = GetUnitX(caster);
            float startY = GetUnitY(caster);

            // Create a group variable to hold the units the spell will hit 
            group targets = CreateGroup();
            // Put all units within spellRange of caster into the targets group 
            GroupEnumUnitsInRange(targets, startX, startY, kSpellRange, null);
            Helpers.DebugPrint(BlzGroupGetSize(targets).ToString());
            // This variable will store the target we're currently hitting 
            // Start with the first unit in the group 
            unit currentTarget = FirstOfGroup(targets);

            // While there's still a target to hit and we have't yet hit max targets
            while (currentTarget != null && count > 0)
            {
                // GroupEnumUnitsInRangeOfLoc includes allied and dead units 
                // Check that the unit we're currently considering to hit 
                // is both an enemy and alive 
                if (IsUnitEnemy(currentTarget, GetOwningPlayer(caster)) &&
                    BlzIsUnitSelectable(currentTarget))   // Could use UnitAlive instead BlzUnitSelectable
                {                                         // But units are still considered alive when playing 
                                                          // death animation.
                    // Get the position of the enemy we're targeting 
                    float targetX = GetUnitX(currentTarget);
                    float targetY = GetUnitY(currentTarget);
                    // Teleport our caster to the enemy's position
                    SetUnitPosition(caster, targetX, targetY);

                    // Have the caster deal damage to the enemy 
                    UnitDamageTarget(caster, currentTarget, kDamage, true, false,
                        ATTACK_TYPE_CHAOS, DAMAGE_TYPE_NORMAL, null);

                    // Decrement the count, as we hit a target 
                    count -= 1;

                    // Take a brief pause before teleporting to the next target 
                    TriggerSleepAction(0.35f);
                }

                // Remove the unit we just considered from the group 
                GroupRemoveUnit(targets, currentTarget);

                // Get the next unit in the group to consider. If the group is
                // empty, this will return null and break out of the while loop
                currentTarget = FirstOfGroup(targets);
            }

            // Certain Warcraft 3 types, like groups, need to be cleaned up 
            DestroyGroup(targets);
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
            const float kCircleRadius = 500.0f;
            // The angle between each wisp 
            const float kAngleDelta = (2.0f * 3.1415f) / kNumWisps;
            // A wisp's unitId 
            int wispId = FourCC("ewsp");
            // for each wisp we want to spawn...
            for (uint i = 0; i < kNumWisps; ++i)
            {
                // Calculate position in the circle
                float angle = kAngleDelta * i;
                float x = kCircleRadius * Cos(angle);
                float y = kCircleRadius * Sin(angle);

                // Spawn wisp 
                CreateUnit(neutralAggressive, wispId, x, y, 0.0f);
            }

            Helpers.DebugPrint("Hello War3 C#!");
        }
    }
}