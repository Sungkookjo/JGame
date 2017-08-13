using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JGame.Pool;
using JGame.Data;

namespace JGame
{
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

    [System.Serializable]
    public struct IntRect
    {
        public int x;
        public int y;
    }

    public class Map : MonoBehaviour
    {
        // x * y
        public IntRect tileNum;
                
        // x spacing
        public Vector3 tileSpacingX = new Vector3(0, 0, 0);
        // y spacing
        public Vector3 tileSpacingY = new Vector3(0, 0, 0);

        // tile prefab
        public List<GameObject> tilePrefabs = new List<GameObject>();
        public GameObject spawnPointPrefab = null;

        [HideInInspector]
        // created tiles
        public List<Tile> tiles = new List<Tile>();
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        // instance
        private static Map _instance = null;
        public static Map instance
        {
            get
            {
                return _instance;
            }
        }

        // Awake
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // initialize
        public void Initialize(string dataPath)
        {
            MapData data = DataController.LoadJson<MapData>(dataPath); 

            Initialize(data);
        }

        public void Initialize(MapData newData)
        {
            Vector3 top = new Vector3();
            tiles.Clear();

            tileNum.x = newData.tileX;
            tileNum.y = newData.tileY;

            top += tileSpacingX * tileNum.x * -0.5f;
            top += tileSpacingY * tileNum.y * -0.5f;

            for (int y = 0; y < tileNum.y; ++y)
            {
                for (int x = 0; x < tileNum.x; ++x)
                {
                    var i = newData.datas[tileNum.x * y + x];

#if UNITY_EDITOR
                    GameObject obj = null;
                    if ( Application.isEditor )
                    {
                        obj = Instantiate(tilePrefabs[i]);
                    }
                    else
                    {
                        obj = ObjectPoolManager.instance.Pop(tilePrefabs[i]);
                    }
#else
                    GameObject obj = ObjectPoolManager.instance.Pop(tilePrefabs[i]);
#endif

                    if (obj == null)
                    {
                        Debug.LogError("obj == null.");
                    }

                    // set game object properties
                    obj.name = "tile_" + (tileNum.x * y + x ) + "_" + y + "," + x; // name
                    obj.transform.SetParent(transform); // parent
                    obj.transform.position = top + (tileSpacingX * x) + (tileSpacingY * y); // position

                    var tile = obj.GetComponent<Tile>();
                    tile.position.x = x;
                    tile.position.y = y;

                    var sr = obj.GetComponent<SpriteRenderer>();
                    sr.sortingOrder = tileNum.x * y + x;

                    tiles.Add(tile);
                }
            }

            // add spawn point
            for(int i=0; i< newData.SpawnPoints.Length; ++i)
            {
#if UNITY_EDITOR
                GameObject obj = null;
                if (Application.isEditor)
                {
                    obj = Instantiate(spawnPointPrefab);
                }
                else
                {
                    obj = ObjectPoolManager.instance.Pop(spawnPointPrefab);
                }
#else
                    GameObject obj = ObjectPoolManager.instance.Pop(spawnPointPrefab);
#endif
                if (obj == null)
                {
                    Debug.LogError("obj == null.");
                }

                // set game object properties
                obj.name = "SpawnPoint_" + i + "_team"+ newData.SpawnPoints[i].teamIndex; // name
                obj.transform.SetParent(transform); // parent
                obj.transform.position = top + (tileSpacingX * newData.SpawnPoints[i].tileX) + (tileSpacingY * newData.SpawnPoints[i].tileY); // position

                var spawnPoint = obj.GetComponent<SpawnPoint>();
                spawnPoint.position.x = newData.SpawnPoints[i].tileX;
                spawnPoint.position.y = newData.SpawnPoints[i].tileY;
                spawnPoint.SetTeamIndex(newData.SpawnPoints[i].teamIndex);

                spawnPoints.Add(spawnPoint);
            }
        }

        // get tile
        public Tile GetTile(IntRect pos)
        {
            return GetTile(pos.x, pos.y);
        }

        public Tile GetTile(int x, int y)
        {
            if( x < 0 || y < 0 || x >= tileNum.x || y > tileNum.y )
            {
                Debug.LogError("Wrong index.");
            }

            return tiles[y * tileNum.x + x];
        }

        public void LeaveFrom( IntRect pos, GameObject obj )
        {
            LeaveFrom(pos.x, pos.y, obj);
        }

        public void LeaveFrom( int x, int y, GameObject obj )
        {
            Tile t = GetTile(x,y);

            if (t == null) return;

            if (t.actor == obj)
            {
                t.actor = null;
            }
        }

        public void MoveTo( IntRect pos, GameObject obj )
        {
            MoveTo(pos.x, pos.y, obj);
        }

        public void MoveTo( int x, int y, GameObject obj )
        {
            Tile t = GetTile(x, y);

            if (t == null) return;

            if (t.actor == null)
            {
                t.actor = obj;
            }
        }
    }
}
