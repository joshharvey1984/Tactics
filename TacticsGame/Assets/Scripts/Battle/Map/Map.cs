using System;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Map {
    public class Map : MonoBehaviour {
        public const int MapSizeX = 16, MapSizeZ = 16;
        public GameObject uiTilePrefab;
        public Canvas canvas;


        private void Awake() {
            CreateMapTiles();
        }

        private void Start() {
            Unit.SetUnitId();
        }

        private void CreateMapTiles() {
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