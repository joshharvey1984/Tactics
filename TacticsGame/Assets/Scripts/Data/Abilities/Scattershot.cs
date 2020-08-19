using System.Linq;
using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class Scattershot : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }

        public Scattershot() {
            Name = "Scattershot";
            AbilityType = AbilityTypes.Active;
            Description = "Graze all units in cone target for half damage.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Tex_skill_44");
            TargetingType = TargetingTypes.Self;
            SpecialTarget = SpecialTargeting.Cone;
        }
        
        public override void Targeting() {
            var coneTarget = new ConeTarget();
            coneTarget.SubscribeToHoverTile();
        }

        public override void Execute() {
            Unit.ActiveUnit.FireBullets(1);
            
            foreach (var activeUnitWatchingTile in Unit.ActiveUnit.watchingTiles) {
                if (activeUnitWatchingTile.Key.Name == "Scattershot") {
                    foreach (var mapTile in activeUnitWatchingTile.Value) {
                        foreach (var unit in Unit.All) {
                            if (unit.GetCurrentMapTile() == mapTile) {
                                unit.PopUpText("15");
                                unit.TakeDamage(15);
                            }
                        }
                    }
                }
            }

            Unit.ActiveUnit.watchingTiles.Clear();
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        public override void EndAbility() {
            Unit.ActiveUnit.EndTurn();
        }
    }
}