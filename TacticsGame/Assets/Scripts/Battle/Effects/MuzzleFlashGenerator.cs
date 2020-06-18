using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.Battle.Effects {
    public class MuzzleFlashGenerator : MonoBehaviour {
        public GameObject muzzleFlashPrefab;
        public Material[] materials;
        public float rate = 8.0F;
        public bool on = false;

        private float _nextMuzzleFlashTime;

        private void Update() {
            if (on) {
                if (Time.time > _nextMuzzleFlashTime){
                    _nextMuzzleFlashTime = Convert.ToSingle(Time.time + (1.0F / rate));
                    var newMuzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
                    var materialId = Mathf.RoundToInt(Random.Range(0,materials.Length));
                    newMuzzleFlash.GetComponent<Renderer>().material = materials[materialId];
                    newMuzzleFlash.transform.parent = transform;
                }
            }
        }
    }
}
