
namespace JGame
{
    // localization data class
    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }

    // localization item class
    [System.Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;

        public LocalizationItem( string k , string v )
        {
            key = k;
            value = v;
        }
    }

    // map data class
    [System.Serializable]
    public class MapData
    {
        public int tileX;
        public int tileY;
        public SpawnData[] SpawnPoints;
        public int[] datas;
    }

    // spawn data class
    [System.Serializable]
    public class SpawnData
    {
        public int tileX;
        public int tileY;
        public int teamIndex;
    }
}