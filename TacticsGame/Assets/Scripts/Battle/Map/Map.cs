using System;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Map
{
    public class Map : MonoBehaviour
    {
        private const int MapSizeX = 16, MapSizeZ = 16;
        public GameObject uiTilePrefab;
        public Canvas canvas;
        
        
        private void Awake()
        {
            CreateMapTiles();
            foreach (var unit in Unit.All) {
                unit.currentTile = MapTile.GetMapTileFromPos(Convert.ToInt32(unit.gameObject.transform.position.x), Convert.ToInt32(unit.gameObject.transform.position.z));
            }
        }

        private void CreateMapTiles()
        {
            for (var x = 0; x < MapSizeX; x++) {
                for (var z = 0; z < MapSizeZ; z++) {
                    var mapTile = new MapTile(x, z);
                    var uiTile = Instantiate(uiTilePrefab, new Vector3(x, 0.05F, z), Quaternion.identity, transform);
                    uiTile.transform.Rotate(90, 0, 0);
                    uiTile.GetComponent<Renderer>().enabled = false;
                    mapTile.UiTile = uiTile;
                }
            }
        }
    }
}
