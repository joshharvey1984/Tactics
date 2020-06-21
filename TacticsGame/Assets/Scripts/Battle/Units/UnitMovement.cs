using TacticsGame.Battle.Map;
using UnityEngine;

namespace TacticsGame.Battle.Units {
    public class UnitMovement : MonoBehaviour {
        private Animator _animator;
        
        private const float MoveSpeed = 2.5f;
        private MapTile _nextTile;
        private Vector3 _nextPos;
        
        public Unit unit;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void SetNextTile(MapTile tile) {
            if (_nextTile == null) SetAnimation("Sprint", true);
            _nextTile = tile;
            _nextPos = GetNextMovePosition();
            transform.LookAt(_nextPos);
        }

        public void EndMove() {
            _nextTile = null;
            SetAnimation("Sprint", false);
        }

        private Vector3 GetNextMovePosition() {
            var tPos = _nextTile.UiTile.transform.position;
            return new Vector3(tPos.x, 0, tPos.z);
        }

        private void Update() {
            if (_nextTile != null) {
                if (Vector3.Distance(transform.position, _nextPos) > 0.01f) {
                    transform.position = Vector3.MoveTowards(transform.position, _nextPos, MoveSpeed * Time.deltaTime);
                    //_cameraScript.CentreOnGameObject(ut.position);
                }
                else {
                    transform.position = new Vector3(_nextPos.x, _nextPos.y, _nextPos.z);
                    unit.MoveNextTile();
                }
            }
        }

        public void SetAnimation(string anim, bool tf) {
            _animator.SetBool(anim, tf);
        }
    }
}