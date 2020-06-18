using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.Battle.Effects {
    public class BulletTracerGenerator : MonoBehaviour {
        public GameObject bulletTracePrefab;
        public float rate = 8.0F;
        public Vector3 velocity;
        public bool on = false;
        public float accuracy = 1.0F;

        private float _nextBulletTracerTime;

        private void Update() {
            accuracy = Mathf.Clamp01(accuracy);
            if (!on) return;
            if(Time.time > _nextBulletTracerTime){
                rate = Mathf.Max(rate, 1.0F);
                _nextBulletTracerTime = Time.time + (1.0F / rate);
                var newBulletTrace = Instantiate(bulletTracePrefab,transform.position,transform.rotation);
                var bulletVelocity = newBulletTrace.GetComponent<BulletTracer>()._velocity;
                var badAim= (1.0F - accuracy);
                badAim *= newBulletTrace.GetComponent<BulletTracer>().bulletSpeed * 0.05F;
                bulletVelocity += newBulletTrace.transform.right * Random.Range(-badAim,badAim);
                bulletVelocity += newBulletTrace.transform.up * Random.Range(-badAim,badAim);
                newBulletTrace.GetComponent<BulletTracer>()._velocity = bulletVelocity;
            }
        }
    }
}
