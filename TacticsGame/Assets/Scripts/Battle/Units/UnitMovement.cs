using TacticsGame.Battle.Map;
using UnityEngine;

namespace TacticsGame.Battle.Units {
    public class UnitMovement : MonoBehaviour {
        private Animator _animator;
        
        private const float MoveSpeed = 2.5f;
        private MapTile _nextTile;
        private Vector3 _nextPos;
        
        public Unit unit;
        private MapTile _lastMapTile;
        
        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _lastMapTile = unit.GetCurrentMapTile();
        }

        public void SetNextTile(MapTile tile) {
            if (_nextTile == null) SetAnimationBool("Sprint", true);
            _nextTile = tile;
            _nextPos = GetNextMovePosition();
            transform.LookAt(_nextPos);
        }

        public void EndMove() {
            _nextTile = null;
            SetAnimationBool("Sprint", false);
        }

        private Vector3 GetNextMovePosition() {
            var tPos = _nextTile.UiTile.transform.position;
            return new Vector3(tPos.x, 0, tPos.z);
        }

        private void Update() {
            if (_nextTile == null) return;
            if (Vector3.Distance(transform.position, _nextPos) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, _nextPos, MoveSpeed * Time.deltaTime);
                if (unit.GetCurrentMapTile() != _lastMapTile) {
                    _lastMapTile = unit.GetCurrentMapTile();
                    unit.UpdateVisibility();
                    CheckForTileWatch();
                }
                //_cameraScript.CentreOnGameObject(ut.position);
            }
            else {
                transform.position = new Vector3(_nextPos.x, _nextPos.y, _nextPos.z);
                unit.MoveNextTile();
            }
        }

        private void CheckForTileWatch() {
            foreach (var checkUnit in Unit.All) {
                if (checkUnit.gang == unit.gang) continue;
                if (checkUnit.aoeTargetTiles.Count == 0) continue;
                if (checkUnit.watchingUnit != unit & checkUnit.watchingUnit != null) continue;
                foreach (var unitWatchingTile in checkUnit.aoeTargetTiles) {
                    if (!unitWatchingTile.Value.Contains(_lastMapTile)) continue;
                    //unitWatchingTile.Key.TileWatchTrigger(checkUnit);
                    checkUnit.aoeTargetTiles.Remove(unitWatchingTile.Key);
                    checkUnit.watchingUnit = null;
                    break;
                }
            }
        }

        public void SetAnimationBool(string anim, bool tf) {
            _animator.SetBool(anim, tf);
        }

        public void SetAnimationTrigger(string anim) {
            _animator.SetTrigger(anim);
        }
    }
}