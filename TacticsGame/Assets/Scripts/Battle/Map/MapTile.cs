using System.Collections.Generic;

namespace TacticsGame.Battle.Map
{
    public class MapTile
    {

        public static List<MapTile> All;
        
        private int _mapPosX, _mapPosZ;

        public MapTile(int mapPosX, int mapPosZ)
        {
            _mapPosX = mapPosX;
            _mapPosZ = mapPosZ;
        }
    }
}