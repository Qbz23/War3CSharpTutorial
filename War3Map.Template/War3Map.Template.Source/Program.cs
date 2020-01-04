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
            Helpers.DebugPrint("Hello War3 C#!");
        }
    }
}