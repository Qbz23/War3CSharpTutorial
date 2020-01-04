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

            // save id into an int 
            int grassId = FourCC("Lgrs");
            // Spawn a 3 tile wide patch of grass at the center of the map
            SetTerrainType(0, 0, grassId, 0, 3, 0);

            Helpers.DebugPrint("Hello War3 C#!");
        }
    }
}