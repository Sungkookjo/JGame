using System.Collections.Generic;
using UnityEngine;
using System.IO;
using JGame.Pool;
using JGame.Data;

namespace JGame
{
    [System.Serializable]
    public struct IntRect
    {
        public int x;
        public int y;

        public int magnitude { get { return Mathf.Abs(x) + Mathf.Abs(y); } }

        public void Normalize()
        {
            x = Mathf.Clamp(x, -1, 1);
            y = Mathf.Clamp(y, -1, 1);
        }

        public static IntRect operator -(IntRect left, IntRect right)
        {
            IntRect retval = new IntRect();
            retval.x = left.x - right.x;
            retval.y = left.y - right.y;
            return retval;
        }

        public static IntRect operator +(IntRect left, IntRect right)
        {
            IntRect retval = new IntRect();
            retval.x = left.x + right.x;
            retval.y = left.y + right.y;
            return retval;
        }
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

        public GameObject arlramObj;
        public List<GameObject> arlramlist;

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
            arlramObj = Instantiate(Resources.Load<GameObject>("Prefab/Map/Arlram"));
            arlramObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
            arlramObj.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            arlramObj.SetActive(false);
        }

        void CreateArlram( Vector3 position )
        {
            var newObj = ObjectPoolManager.instance.Pop(arlramObj);
            newObj.transform.position = position;
            arlramlist.Add(newObj);
        }

        void ClearArlramList()
        {
            foreach( GameObject obj in arlramlist )
            {
                ObjectPoolManager.instance.Push(obj);
            }
            arlramlist.Clear();
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
            if( x < 0 || y < 0 || x >= tileNum.x || y >= tileNum.y )
            {
                return null;
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

        public void DisableAllTiles()
        {
            ClearArlramList();
            for ( int i = 0; i < tiles.Count; ++i )
            {
                if( tiles[i] != null )
                {
                    tiles[i].SetSelectable(false);
                }
            }
        }

        public void EnableAllTiles()
        {
            ClearArlramList();
            for (int i = 0; i < tiles.Count; ++i)
            {
                if (tiles[i] != null)
                {
                    tiles[i].SetSelectable(true);
                }
            }
        }

        public void EnableRangeTiles( IntRect position, int range )
        {
            DisableAllTiles();

            for (int y = (position.y - range); y <= (position.y + range); ++y)
            {
                var gab = range - Mathf.Abs(y - position.y);
                for ( int x = position.x - gab; x <= position.x + gab;++x )
                {
                    var tile = GetTile(x, y);

                    if( tile != null )
                    {
                        CreateArlram(tile.transform.position);
                        tile.SetSelectable(true);
                    }
                }
            }
        }
    }
}
