using System.Collections.Generic;
using UnityEngine;
using JGame.Pool;

namespace JGame.Map
{
    [System.Serializable]
    public class MapData
    {
        public int tileX;
        public int tileY;
        public int[] datas;
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

        // scale
        public Vector2 tileSize;
        
        // x spacing
        public Vector3 tileSpacingX = new Vector3(0, 0, 0);
        // y spacing
        public Vector3 tileSpacingY = new Vector3(0, 0, 0);

        // tile prefab
        public List<GameObject> tilePrefabs = new List<GameObject>();

        [HideInInspector]
        // created tiles
        public List<GameObject> tiles = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        // initialize
        public void Initialize(MapData newData)
        {
            tiles.Clear();

            tileNum.x = newData.tileX;
            tileNum.y = newData.tileY;

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
                    obj.transform.position = transform.position + (tileSpacingX * x) + (tileSpacingY * y); // position
                    obj.transform.Rotate(0, 0, 45.0f); // rotation
                    obj.transform.localScale = tileSize; // scale

                    var tile = obj.GetComponent<Tile>();

                    tile.position.x = x;
                    tile.position.y = y;
                }
            }
        }

        // initialize
        void Initialize()
        {
            tiles.Clear();

            for (int y = 0; y < tileNum.y; ++y)
            {
                for( int x =0;x<tileNum.x;++x)
                {
                    var i = Random.Range(0, tilePrefabs.Count );

                    var obj = ObjectPoolManager.instance.Pop(tilePrefabs[i]);

                    if( obj == null )
                    {
                        Debug.LogError("obj == null.");
                    }

                    // set game object properties
                    obj.name = "tile" + y + "," + x; // name
                    obj.transform.SetParent(transform); // parent
                    obj.transform.position = transform.position + (tileSpacingX * x) + (tileSpacingY * y); // position
                    obj.transform.Rotate(0, 0, 45.0f); // rotation
                    obj.transform.localScale = tileSize; // scale

                    var tile = obj.GetComponent<Tile>();

                    tile.position.x = x;
                    tile.position.y = y;
                }
            }
        }

        public Tile GetTile(int x, int y)
        {
            if( x < 0 || y < 0 || x >= tileNum.x || y > tileNum.y )
            {
                Debug.LogError("Wrong index.");
            }

            return tiles[y * tileNum.x + x].GetComponent<Tile>();
        }
    }
}
