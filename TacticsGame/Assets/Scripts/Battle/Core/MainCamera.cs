using System;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class MainCamera : MonoBehaviour {
        private Transform _thisTransform;
        
        [SerializeField] private float moveSpeed = 0.1F;
        [SerializeField] private float rotateSpeed = 0.1F;
        [SerializeField] private float boundBuffer = 4.0F;
        
        private bool _rotating;
        private Vector3 _rotateTarget;

        private float _maxX, _maxZ;

        private void Awake() => _thisTransform = transform;

        private void Start() {
            _maxX = Map.Map.MapSizeX - boundBuffer;
            _maxZ = Map.Map.MapSizeZ - boundBuffer;
            _rotateTarget = _thisTransform.rotation.eulerAngles;
        }

        private void FixedUpdate() {
            if (Input.GetKey(KeyCode.D)) MoveCamera(moveSpeed, 0);
            if (Input.GetKey(KeyCode.A)) MoveCamera(-moveSpeed, 0);
            if (Input.GetKey(KeyCode.S)) MoveCamera(0, -moveSpeed);
            if (Input.GetKey(KeyCode.W)) MoveCamera(0, moveSpeed);
            
            if (Input.GetKey(KeyCode.Q) && !_rotating) {
                _rotating = true;
                _rotateTarget = new Vector3(0, _thisTransform.rotation.eulerAngles.y + 90.0F, 0);
                if (_rotateTarget.y > 360) _rotateTarget.y -= 360;
            }
            if (Input.GetKey(KeyCode.E) && !_rotating) {
                _rotating = true;
                _rotateTarget = new Vector3(0, _thisTransform.rotation.eulerAngles.y - 90.0F, 0);
                if (_rotateTarget.y < 0) _rotateTarget.y += 360;
            }

            if (Math.Abs(_thisTransform.rotation.eulerAngles.y - _rotateTarget.y) > 1.0F) {
                transform.rotation = Quaternion.RotateTowards
                    (transform.rotation, Quaternion.Euler(_rotateTarget), Time.deltaTime * rotateSpeed);
            }
            else _rotating = false;
            
        }

        private void MoveCamera(float x, float z) {
            _thisTransform.Translate(new Vector3(x * Time.deltaTime, 0, z * Time.deltaTime));
            
            var tempPos = _thisTransform.position;
            if (_thisTransform.position.x < boundBuffer) tempPos.x = boundBuffer;
            if (_thisTransform.position.z < boundBuffer) tempPos.z = boundBuffer;
            if (_thisTransform.position.x > _maxX) tempPos.x = _maxX;
            if (_thisTransform.position.z > _maxZ) tempPos.z = _maxZ;
            
            _thisTransform.position = tempPos;
        }
    }
}