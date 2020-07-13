using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map.Enums;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace TacticsGame.Battle.Map.UI {
    public class CoverIcons : MonoBehaviour {
        private static MouseHoverTile _mouseHoverTile;
        [SerializeField] private GameObject coverIconPrefab;
        [SerializeField] private Texture fullCover;

        private List<GameObject> _allCoverIcons;

        public void Awake() {
            _mouseHoverTile = GameObject.Find("GameManager").GetComponent<MouseHoverTile>();
            _allCoverIcons = new List<GameObject>();
        }
        public void SubscribeToHoverTile() => _mouseHoverTile.OnHoverTileChanged += DrawIcons;
        public void UnsubscribeToHoverTile() => _mouseHoverTile.OnHoverTileChanged -= DrawIcons;

        private void DrawIcons(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {
            DestroyAll();
            if (args.HoverTileUi == null) return;
            
            var hoverTile = MapTile.GetMapTileFromUi(args.HoverTileUi);
            var iconTiles =hoverTile.GetAdjacentTiles(true).Values.ToList();
            iconTiles.Add(hoverTile);
            
            foreach (var iconTile in iconTiles) {
                if (iconTile == null) continue;
                var tileCover = iconTile.Cover;
                foreach (var coverType in tileCover) {
                    CreateIcon(iconTile, coverType.Key, coverType.Value,iconTile == hoverTile);
                }
                
            }
        }

        public void DestroyAll() => _allCoverIcons.ForEach(Destroy);

        private void CreateIcon(MapTile mapTile, Direction direction, CoverType coverType, bool highlight) {
            if (coverType == CoverType.NoCover) return;

            var instPos = mapTile.UiTile.transform.position;
            var instRot = mapTile.UiTile.transform.rotation;

            instRot.y = 90;
            instPos.y = 0.5F;

            if (direction == Direction.North) {
                instRot.w += 90;
                instPos.x += 0.40F;
            }

            if (direction == Direction.South) {
                instRot.w += 90;
                instPos.x -= 0.40F;
            }

            if (direction == Direction.East) {
                instPos.z -= 0.40F;
            }

            if (direction == Direction.West) {
                instPos.z += 0.40F;
            }

            var instIcon = Instantiate(coverIconPrefab, instPos, instRot, transform);
            _allCoverIcons.Add(instIcon);
            var coverIcon = instIcon.GetComponent<CoverIcon>();
            coverIcon.mapTile = mapTile;
            if (coverType == CoverType.FullCover) coverIcon.GetComponent<CoverIcon>().SetCoverIcon(fullCover);
            coverIcon.HighlightIcon(highlight);
        }
    }
}