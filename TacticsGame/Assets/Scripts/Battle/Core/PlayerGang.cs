using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class PlayerGang : MonoBehaviour {
        private int _gangNumber;

        public void SetGangNumber(int gang) => _gangNumber = gang;

        private IEnumerable<Unit> GetPlayerUnits() => Unit.All.Where(unit => unit.gang == _gangNumber).ToList();

        private IEnumerable<Unit> GetEnemyUnits() => Unit.All.Where(unit => unit.gang != _gangNumber).ToList();

        private List<Unit> GetAllEnemiesInSight() {
            var returnList = new List<Unit>();
            foreach (var playerUnit in GetPlayerUnits()) {
                returnList.AddRange(playerUnit.EnemiesInLineOfSight());
            }
            return returnList;
        }
        
        public void UnitVisibilityUpdate() {
            foreach (var enemyUnit in GetEnemyUnits()) {
                if (GetAllEnemiesInSight().Contains(enemyUnit)) {
                    enemyUnit.ShowUnit();
                }
                else {
                    enemyUnit.HideUnit();
                }
            }
        }
        
        public List<MapTile> GetAllTilesInSight() {
            var returnList = new List<MapTile>();
            foreach (var unit in GetPlayerUnits()) {
                returnList.AddRange(unit.AllTilesInSight());
            }
            return returnList;
        }
    }
}
