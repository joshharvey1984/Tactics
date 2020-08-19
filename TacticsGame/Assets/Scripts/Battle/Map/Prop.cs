using System;
using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map.Enums;
using UnityEngine;

namespace TacticsGame.Battle.Map {
    public class Prop : MonoBehaviour {
        public static List<Prop> All = new List<Prop>();

        [SerializeField] private bool wallType = false;
        [SerializeField] private CoverType coverType = CoverType.NoCover;
        [SerializeField] private bool moveBlocker = false;
        [SerializeField] private bool transparentHover = false;
        [SerializeField] private int lifeSpan = -1;

        private GameManager _gameManager;
        private MouseHoverTile _mouseHoverTile;
        private Camera _mainCamera;
        private MeshRenderer _meshRenderer;
        private Bounds Bounds { get; set; }
        public GameObject boundsCalc;

        private void Awake() {
            All.Add(this);
            _mainCamera = Camera.main;
            _mouseHoverTile = FindObjectOfType<MouseHoverTile>();
            _gameManager = FindObjectOfType<GameManager>();
            _mouseHoverTile.OnHoverTileChanged += CheckForTransparency;
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (lifeSpan > 0) _gameManager.OnNewRound += ReduceLifeSpan;
        }

        private void Start() {
            if (moveBlocker) {
                if (!wallType) FindBlockingTiles();
                else FindWallBLockingTiles();
            }
            else {
                if (!wallType) boundsCalc.GetComponent<MeshRenderer>().enabled = false;
            }
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
            }
        }

        private void CheckForTransparency(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {
            if (!transparentHover) return;
            if (args.HoverTileUi == null) {
                TurnOpaque();
                return;
            }

            var camPos = _mainCamera.transform.position;
            var propPos = transform.position;
            var mousePos = args.HoverTileUi.transform.position;
            
            if (camPos.x < propPos.x && mousePos.x > propPos.x && Vector3.Distance(mousePos, propPos) < 3.0F ||
                camPos.z < propPos.z && mousePos.z > propPos.z && Vector3.Distance(mousePos, propPos) < 3.0F) {
                TurnTransparent();
            }
            else {
                TurnOpaque();
            }
        }

        private void TurnOpaque() {
            _meshRenderer.material.shader = Shader.Find("Standard");
        }

        private void TurnTransparent() {
            _meshRenderer.material.shader = Shader.Find("Outlined/Silhouette Only");
            _meshRenderer.material.SetColor("_OutlineColor", new Color(0.213F, 0.925F, 0.943F));
        }

        private void ReduceLifeSpan(object sender, EventArgs e) {
            lifeSpan--;
            if (lifeSpan == 0) Destroy(gameObject);
        }
        
    }
}