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

        private static void Main()
        {
            // Disable Fog
            FogEnable(false);
            FogMaskEnable(false);

            // get id of custom unit                                 
            int customUnitId = FourCC("O000");
            // create custom unit for local player at map center (0, 0), facing 0 degrees                     
            CreateUnit(GetLocalPlayer(), customUnitId, 0.0f, 0.0f, 0.0f);

            Helpers.DebugPrint("Hello War3 C#!");
        }
    }
}