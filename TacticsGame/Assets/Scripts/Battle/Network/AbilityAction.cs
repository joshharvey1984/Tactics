using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Units;

namespace TacticsGame.Battle.Network {
    public class AbilityAction {
        public Dictionary<string, string> AbilityActionData { get; private set; }

        public AbilityAction() {
            AbilityActionData = new Dictionary<string, string> {
                { "AbilityName", Unit.ActiveUnit.selectedAbility.Name }, 
                { "UnitID", Unit.ActiveUnit.unitId.ToString() },
                { "DamageDone", Unit.ActiveUnit.damageCalculation.ToString() }
            };
            if (Unit.ActiveUnit.targetUnit)
                AbilityActionData.Add("TargetUnitID", Unit.ActiveUnit.targetUnit.unitId.ToString());
            
            if (Unit.ActiveUnit.selectedMapTile != null) {
                AbilityActionData.Add("SelectedMapTileX", Unit.ActiveUnit.selectedMapTile.MapPosX.ToString());
                AbilityActionData.Add("SelectedMapTileZ", Unit.ActiveUnit.selectedMapTile.MapPosZ.ToString());
            }

            if (Unit.ActiveUnit.aoeTargetTiles.ContainsKey(Unit.ActiveUnit.selectedAbility)) {
                var aoeTiles = Unit.ActiveUnit.aoeTargetTiles.First(x => x.Key == Unit.ActiveUnit.selectedAbility);
                var tileCount = 0;
                foreach (var mapTile in aoeTiles.Value) {
                    AbilityActionData.Add("aoeMapTileX" + tileCount, mapTile.MapPosX.ToString());
                    AbilityActionData.Add("aoeMapTileZ" + tileCount, mapTile.MapPosZ.ToString());
                    tileCount++;
                }
            }
        }

        public static void AbilityActionDeserialize(Dictionary<string, string> abilityAction) {
            var unit = Unit.GetUnitById(Convert.ToInt32(abilityAction["UnitID"]));
            Unit.ActiveUnit = unit;
            unit.selectedAbility = unit.abilities.First(ability => ability.Name == abilityAction["AbilityName"]);
            unit.damageCalculation = Convert.ToInt32(abilityAction["DamageDone"]);
            
            if (abilityAction.ContainsKey("SelectedMapTileX")) {
                var posX = Convert.ToInt32(abilityAction["SelectedMapTileX"]);
                var posZ = Convert.ToInt32(abilityAction["SelectedMapTileZ"]);
                unit.selectedMapTile = MapTile.GetMapTileFromPos(posX, posZ);
            }
            if (abilityAction.ContainsKey("TargetUnitID"))
                unit.targetUnit = Unit.GetUnitById(Convert.ToInt32(abilityAction["TargetUnitID"]));

            if (abilityAction.ContainsKey("aoeMapTileX1")) {
                var aoeMapCount = abilityAction.Keys.Count(x => x.Contains("aoeMapTile")) / 2;
                var mapTiles = new List<MapTile>();
                for (var i = 1; i < aoeMapCount; i++) {
                    var mapX = Convert.ToInt32(abilityAction["aoeMapTileX" + i]);
                    var mapZ = Convert.ToInt32(abilityAction["aoeMapTileZ" + i]);
                    mapTiles.Add(MapTile.GetMapTileFromPos(mapX, mapZ));
                }

                if (!Unit.ActiveUnit.aoeTargetTiles.ContainsKey(Unit.ActiveUnit.selectedAbility)) {
                    Unit.ActiveUnit.aoeTargetTiles.Add(Unit.ActiveUnit.selectedAbility, mapTiles);
                }
            }
        }
    }
}