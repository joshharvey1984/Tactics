using System.Collections.Generic;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class PlayerGang : MonoBehaviour {
        public int gangNumber;
        public List<Unit> playerUnits = new List<Unit>();
        public List<Unit> enemyUnits = new List<Unit>();

        public void SetGangNumber(int gang) {
            gangNumber = gang;
            foreach (var unit in Unit.All) {
                if (unit.gang == gangNumber) {
                    playerUnits.Add(unit);
                }
                else enemyUnits.Add(unit);
            }
        }

        private List<Unit> GetAllEnemiesInSight() {
            var returnList = new List<Unit>();
            foreach (var playerUnit in playerUnits) {
                returnList.AddRange(playerUnit.EnemiesInLineOfSight());
            }
            return returnList;
        }
        
        public void UnitVisibilityUpdate() {
            foreach (var enemyUnit in enemyUnits) {
                if (GetAllEnemiesInSight().Contains(enemyUnit)) {
                    enemyUnit.ShowUnit();
                }
                else {
                    enemyUnit.HideUnit();
                }
            }
        }
        
    }
}
