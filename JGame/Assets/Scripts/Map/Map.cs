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
        public Vector2 tileSize = new Vector2(1.4142f, 1.4142f);
        
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
        }

        // initialize
        public void Initialize(string dataPath)
        {
            MapData data = DataController.LoadJson<MapData>(dataPath); 

            Initialize(data);
        }

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

                    tiles.Add(obj);
                }
            }
        }

        // get tile
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
