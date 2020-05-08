using TacticsGame.Battle.Map.UI;
using UnityEngine;

namespace TacticsGame.Battle.Map
{
    public class Map : MonoBehaviour
    {
        private const int MapSizeX = 16, MapSizeZ = 16;
        public Canvas canvas;
        
        
        private void Awake()
        {
            CreateMapTiles();
        }

        private static void CreateMapTiles()
        {
            for (var x = 0; x < MapSizeX; x++) {
                for (var z = 0; z < MapSizeZ; z++) {
                    var mapTile = new MapTile(x, z);
                }
            }
        }

        private void Start()
        {
            MoveGridLine.DrawMoveGrid(MoveGrid.MoveGridTiles(MapTile.GetTile(5, 5), 5));
        }

    }
}
