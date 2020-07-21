using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Effects {
    public class Projectile : MonoBehaviour {
        public Vector3[] trajectory;
        private int _nextPos = 1;

        public void GetTrajectory() {
            var endPos = Unit.ActiveUnit.selectedMapTile.GetWorldPosition();
            trajectory = ThrowTarget.CalculateArcArray(transform.position, endPos);
        }

        private void Update() {
            if (trajectory.Length == 0) return;
            MoveProjectile();
        }

        private void MoveProjectile() {
            if (Vector3.Distance(transform.position, trajectory[_nextPos]) > 0.01f) {
                transform.position = Vector3.MoveTowards(transform.position, trajectory[_nextPos], 10 * Time.deltaTime);
            }
            else {
                _nextPos++;
                if (_nextPos == trajectory.Length) {
                    EndJourney();
                }
            }
        }

        private void EndJourney() {
            Unit.ActiveUnit.ThrowExecute();
            Destroy(gameObject);
        }
    }
}