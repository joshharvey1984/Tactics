using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Map.Enums;
using UnityEngine;
using static TacticsGame.Battle.Map.Enums.CoverType;
using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map.UI
{
    public class CoverIcons : MonoBehaviour
    {
        [SerializeField] private GameObject coverIcon;
        private MapTile _hoverTile;
        private IEnumerable<MapTile> _iconTiles;
        [SerializeField] private Texture fullCover;

        public void DrawIcons(MapTile hoverTile)
        {
            _hoverTile = hoverTile;
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
            
            foreach(var mapTile in MapTile.All) {
                foreach (var icon in mapTile.CoverIcons)
                {
                    if (mapTile== _hoverTile)
                        icon.GetComponent<Renderer>().material.color = new Color32(121, 187, 191, 255);
                    else
                        icon.GetComponent<Renderer>().material.color = new Color32(121, 187, 191, 50);
                }
            }
        }

        public void DestroyAll()
        {
            foreach (var mapTile in MapTile.All) {
                if (_iconTiles.Contains(mapTile)) continue;
                foreach (var icon in mapTile.CoverIcons) {
                    Destroy(icon);
                }
                mapTile.CoverIcons.Clear();
            }
        }

        private void CreateIcon(MapTile mapTile, Direction direction, CoverType coverType)
        {
            if (coverType == NoCover) return;
            var instPos = mapTile.UiTile.transform.position;
            var instRot = mapTile.UiTile.transform.rotation;
            instRot.y = 90;
            instPos.y = -0.3F;
            if (direction == North) {
                instRot.w += 90;
                instPos.x += 0.45F;
            }
            if (direction == South) {
                instRot.w += 90;
                instPos.x -= 0.45F;
            }
            if (direction == East) {
                instPos.z -= 0.45F;
            }
            if (direction == West) {
                instPos.z += 0.45F;
            }
            
            var instIcon = Instantiate(coverIcon, instPos, instRot, transform);

            if (coverType == FullCover) instIcon.GetComponent<MeshRenderer>().material.mainTexture = fullCover;
            
            mapTile.CoverIcons.Add(instIcon);
        }
    }
}