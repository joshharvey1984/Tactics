using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.Battle.Effects {
    public class MuzzleFlash : MonoBehaviour
    {
        public float minLife = 0.01F;
        public float maxLife = 0.02f;

        private float _destroyTime;
        private float _angle;

        private void Start() {
            _destroyTime = Time.time + Random.Range(minLife, maxLife);
            _angle = 90 * Mathf.Round(Random.Range(0,3));
        }

        private void Update() {
            if (Time.time > _destroyTime){
                Destroy(gameObject);
            }
        }
    }
}
