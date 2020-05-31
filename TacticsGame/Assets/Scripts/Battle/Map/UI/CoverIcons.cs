using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map.Enums;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public class CoverIcons : MonoBehaviour {
        private static MouseHoverTile _mouseHoverTile;
        [SerializeField] private GameObject coverIcon;
        [SerializeField] private Texture fullCover;
        private IEnumerable<MapTile> _iconTiles;

        public void Awake() {
            _mouseHoverTile = GameObject.Find("GameManager").GetComponent<MouseHoverTile>();
        }

        public void SubscribeToHoverTile() => _mouseHoverTile.OnHoverTileChanged += DrawIcons;
        public void UnsubscribeToHoverTile() => _mouseHoverTile.OnHoverTileChanged -= DrawIcons;

        private void DrawIcons(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {

            if (args.HoverTileUi == null) {
                DestroyAll();
                return;
            }
            
            var hoverTile = MapTile.GetMapTileFromUi(args.HoverTileUi);
            _iconTiles = hoverTile.GetAdjacentTiles();
            DestroyAll();

            foreach (var iconTile in _iconTiles) {
                if (!iconTile.CanMoveInto()) continue;
                if (iconTile.CoverIcons.Count > 0) continue;
                var tileCover = iconTile.Cover;
                foreach (var coverType in tileCover) {
                    CreateIcon(iconTile, coverType.Key, coverType.Value);
                }
            }

            foreach (var mapTile in MapTile.All) {
                foreach (var icon in mapTile.CoverIcons) {
                    icon.GetComponent<CoverIcon>().HighlightIcon(mapTile == hoverTile);
                }
            }
        }

        public void DestroyAll() {
            foreach (var mapTile in MapTile.All.Where(mapTile => !_iconTiles.Contains(mapTile))) {
                foreach (var icon in mapTile.CoverIcons) {
                    Destroy(icon);
                }
                
                mapTile.CoverIcons.Clear(); 
            }
        }

        private void CreateIcon(MapTile mapTile, Direction direction, CoverType coverType) {
            if (coverType == CoverType.NoCover) return;

            var instPos = mapTile.UiTile.transform.position;
            var instRot = mapTile.UiTile.transform.rotation;

            instRot.y = 90;
            instPos.y = -0.3F;

            if (direction == Direction.North) {
                instRot.w += 90;
                instPos.x += 0.45F;
            }

            if (direction == Direction.South) {
                instRot.w += 90;
                instPos.x -= 0.45F;
            }

            if (direction == Direction.East) {
                instPos.z -= 0.45F;
            }

            if (direction == Direction.West) {
                instPos.z += 0.45F;
            }

            var instIcon = Instantiate(coverIcon, instPos, instRot, transform);
            if (coverType == CoverType.FullCover) instIcon.GetComponent<CoverIcon>().SetCoverIcon(fullCover);
            mapTile.CoverIcons.Add(instIcon);
        }
    }
}