using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI;
using UnityEngine;
using static TacticsGame.Battle.Map.MapTile;

namespace TacticsGame.Battle.Core
{
    public class MouseControls : MonoBehaviour
    {
        private Camera _mainCamera;
        private const int WallBitMask = ~(1 << 8);
        public GameObject hoverTileObject;

        private void HoverTileChange(GameObject tile)
        {
            hoverTileObject = tile;
            MouseHoverTile = GetMapTileFromUi(hoverTileObject);
            if (MouseHoverTile.MoveNum > 0)
            {
                var rTiles = MoveRoute.GetMoveRoute(GetMapTileFromPos(0, 0), MouseHoverTile);
                RouteLine.DrawMoveLine(rTiles);
                var ci = GameObject.Find("Map").GetComponent<CoverIcons>();
                ci.DrawIcons(MouseHoverTile);
            }
            else
            {
                var ci = GameObject.Find("Map").GetComponent<CoverIcons>();
                ci.DrawIcons(MouseHoverTile);
                RouteLine.DestroyMoveLine();
            }
        }

        private void Awake()
        {
            if (!Camera.current) _mainCamera = Camera.main;
        }

        private void Update()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 200.0f, WallBitMask)) {
                if (hit.collider.gameObject != hoverTileObject) HoverTileChange(hit.collider.gameObject);
            } 
            else {
                MouseHoverTile = null;
                hoverTileObject = null;
            }
        }
    }
}