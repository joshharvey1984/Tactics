using System;
using System.Collections.Generic;
using TacticsGame.Battle.Map.Enums;
using UnityEngine;

namespace TacticsGame.Battle.Map {
    public class Prop : MonoBehaviour {
        public static List<Prop> All = new List<Prop>();

        [SerializeField] private bool wallType;
        [SerializeField] private CoverType coverType = CoverType.NoCover;
        [SerializeField] private bool moveBlocker = false;
        [SerializeField] private bool sightBlocker = false;
        private Bounds Bounds { get; set; }
        public GameObject boundsCalc;

        private void Awake() {
            All.Add(this);
        }

        private void Start() {
            if (!wallType) FindBlockingTiles();
            else FindWallBLockingTiles();
        }

        private void FindBlockingTiles() {
            Bounds = boundsCalc.GetComponent<MeshRenderer>().bounds;

            foreach (var tile in MapTile.All) {
                if (tile.MapPosX < Bounds.min.x || tile.MapPosX > Bounds.max.x) continue;
                if (tile.MapPosZ < Bounds.min.z || tile.MapPosZ > Bounds.max.z) continue;

                tile.TileProp = this;

                var adjTiles = tile.GetAdjacentTiles(false);
                foreach (var adjTile in adjTiles) {
                    adjTile.Value.SetBlocks(DirectionExtensions.Reverse(adjTile.Key), coverType, moveBlocker);
                }
            }
            
            boundsCalc.GetComponent<MeshRenderer>().enabled = false;
        }

        private void FindWallBLockingTiles() {
            var pos = transform.position;
            if (transform.rotation.y == 0) {
                MapTile.GetMapTileFromPos(pos.x + 0.5F, pos.z + 0.5F).SetBlocks(Direction.South, coverType, moveBlocker);
                MapTile.GetMapTileFromPos(pos.x - 0.5F, pos.z + 0.5F).SetBlocks(Direction.North, coverType, moveBlocker);
            }
            else {
                MapTile.GetMapTileFromPos(pos.x + 0.5F, pos.z + 0.5F).SetBlocks(Direction.East, coverType, moveBlocker);
                MapTile.GetMapTileFromPos(pos.x + 0.5F, pos.z - 0.5F).SetBlocks(Direction.West, coverType, moveBlocker);
                Debug.Log(MapTile.GetMapTileFromPos(pos.x + 0.5F, pos.z - 0.5F).MoveBlocked[Direction.West]);
            }
        }
    }
}