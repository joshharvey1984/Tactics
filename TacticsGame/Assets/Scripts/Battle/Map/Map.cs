using UnityEngine;

namespace TacticsGame.Battle.Map
{
    public class Map : MonoBehaviour
    {
        private const int MapSizeX = 10, MapSizeZ = 10;
        
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
        
    }
}
