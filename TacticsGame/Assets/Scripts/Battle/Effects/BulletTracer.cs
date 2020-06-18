using System;
using UnityEngine;

namespace TacticsGame.Battle.Effects {
    public class BulletTracer : MonoBehaviour {
        public float life = 0.5F;
        public float bulletSpeed = 100.0F;

        private float _destroyTime;
        public Vector3 _velocity;

        private void Start() {
            _destroyTime = Time.time + life;
            _velocity += transform.forward * bulletSpeed;
        }

        private void Update() {
            if (Time.time > _destroyTime) Destroy(gameObject);
            else {
                transform.position += _velocity * Time.deltaTime;
                transform.LookAt(transform.position + _velocity);
            }
        }
    }
}
