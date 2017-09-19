
namespace JGame
{
    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }

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

    [System.Serializable]
    public class MapData
    {
        public int tileX;
        public int tileY;
        public SpawnData[] SpawnPoints;
        public int[] datas;
    }

    [System.Serializable]
    public class SpawnData
    {
        public int tileX;
        public int tileY;
        public int teamIndex;
    }
}