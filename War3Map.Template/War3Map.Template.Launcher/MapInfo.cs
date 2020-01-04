using War3Net.Build.Info;

namespace War3Map.Template.Launcher
{
    internal static class Info
    {
        public static MapInfo GetMapInfo()
        {
            var mapInfo = MapInfo.Default;

            mapInfo.MapName = "Warcraft 3 C# Tutorial";
            mapInfo.MapDescription = "My Map Description";
            mapInfo.MapAuthor = "Qbz";
            mapInfo.RecommendedPlayers = "Any";

            mapInfo.MapFlags &= ~MapFlags.MeleeMap;
            mapInfo.ScriptLanguage = ScriptLanguage.Lua;

            PlayerAndForceSettings.ApplyToMapInfo(mapInfo);

            return mapInfo;
        }
    }
}