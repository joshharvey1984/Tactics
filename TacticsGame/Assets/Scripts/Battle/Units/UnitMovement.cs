using TacticsGame.Battle.Map;
using TacticsGame.Data.Abilities;
using UnityEngine;

namespace TacticsGame.Battle.Units {
    public class UnitMovement : MonoBehaviour {
        private Animator _animator;
        
        private const float MoveSpeed = 2.5f;
        private MapTile _nextTile;
        private Vector3 _nextPos;
        
        public Unit _unit;
        private MapTile _lastMapTile;
        

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _lastMapTile = _unit.GetCurrentMapTile();
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
                    if (_unit.GetCurrentMapTile() != _lastMapTile) {
                        _lastMapTile = _unit.GetCurrentMapTile();
                        CheckForTileWatch();
                    }
                    //_cameraScript.CentreOnGameObject(ut.position);
                }
                else {
                    transform.position = new Vector3(_nextPos.x, _nextPos.y, _nextPos.z);
                    _unit.MoveNextTile();
                }
            }
        }

        private void CheckForTileWatch() {
            foreach (var unit in Unit.All) {
                if (unit.gang == _unit.gang) continue;
                if (unit.watchingTiles.Count == 0) continue;
                foreach (var unitWatchingTile in unit.watchingTiles) {
                    if (!unitWatchingTile.Value.Contains(_lastMapTile)) continue;
                    unitWatchingTile.Key.TileWatchTrigger(unit);
                    unit.watchingTiles.Remove(unitWatchingTile.Key);
                    break;
                }
            }
        }

        public void SetAnimation(string anim, bool tf) {
            _animator.SetBool(anim, tf);
        }
    }
}