using TacticsGame.Battle.Core;
using UnityEngine;

namespace TacticsGame.Battle.Map {
    public class FogOfWar : MonoBehaviour {
        public GameObject fogOfWarParticle;
        public ParticleSystem fowParticleSystem;
        public PlayerGang playerGang;

        private void Awake() {
            playerGang = FindObjectOfType<PlayerGang>();
        }

        public void DrawAllFogOfWar() {
            foreach (var mapTile in MapTile.All) {
                if (!mapTile.FogOfWar)
                    mapTile.FogOfWar = Instantiate(fogOfWarParticle, mapTile.GetWorldPosition(), Quaternion.identity, mapTile.UiTile.transform);
            }
        }
        
        public void UpdateFogOfWar() {
            DrawAllFogOfWar();
            var sightTiles = playerGang.GetAllTilesInSight();
            foreach (var sightTile in sightTiles) {
                Destroy(sightTile.FogOfWar);
            }
        }
    }
}