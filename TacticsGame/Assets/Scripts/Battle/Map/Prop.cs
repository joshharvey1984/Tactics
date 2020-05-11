using System.Collections.Generic;
using UnityEngine;

namespace TacticsGame.Battle.Map.Enums
{
    public class Prop : MonoBehaviour
    {
        public static List<Prop> All = new List<Prop>();
        
        [SerializeField]
        private CoverType coverType = CoverType.NoCover;
        
        [SerializeField]
        private bool moveBlocker = false;
        
        private Bounds Bounds { get; set; }
        public GameObject boundsCalc;

        private void Awake()
        {
            All.Add(this);
        }

        private void Start()
        {
            FindBlockingTiles();
            boundsCalc.GetComponent<MeshRenderer>().enabled = false;
        }

        private void FindBlockingTiles()
        {
            Bounds = boundsCalc.GetComponent<MeshRenderer>().bounds;

            foreach (var tile in MapTile.All)
            {
                if (tile.MapPosX < Bounds.min.x || tile.MapPosX > Bounds.max.x) continue;
                if (tile.MapPosZ < Bounds.min.z || tile.MapPosZ > Bounds.max.z) continue;
                
                tile.TileProp = this;
                
                var adjTiles = tile.GetNSEWTiles();
                foreach (var adjTile in adjTiles) {
                    adjTile.Value.SetBlocks(adjTile.Key, coverType, moveBlocker);
                }
            }
        }
    }
}
