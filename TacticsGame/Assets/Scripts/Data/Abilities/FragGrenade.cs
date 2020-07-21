using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public class FragGrenade : Ability {
        public sealed override string Name { get; set; }
        public sealed override string Description { get; set; }
        public sealed override AbilityTypes AbilityType { get; set; }
        public sealed override Sprite Icon { get; set; }
        public sealed override TargetingTypes TargetingType { get; set; }
        public sealed override SpecialTargeting SpecialTarget { get; set; }

        public FragGrenade() {
            Name = "Frag Grenade";
            AbilityType = AbilityTypes.Active;
            Description = "Throw a frag grenade.";
            Icon = AssetDatabase.LoadAssetAtPath("Assets/Textures/Abilities/Icon_Fire.png", typeof(Sprite)) as Sprite;
            TargetingType = TargetingTypes.Throw;
            SpecialTarget = SpecialTargeting.Throw;
        }

        public override void Targeting() {
            Unit.ActiveUnit.selectedEquipment = new Equipments.Utilities.Grenades.FragGrenade();
            var throwTarget = new ThrowTarget(10, 1, 3);
            throwTarget.SubscribeToHoverTile();
        }

        public override void Execute() {
            Unit.ActiveUnit.ThrowAnimation(new Equipments.Utilities.Grenades.FragGrenade());
        }

        public override void ExplodeEffect(MapTile landingTile) {
            var grenade = new Equipments.Utilities.Grenades.FragGrenade();
            var aoeTile = ThrowTarget.FindAOETiles(landingTile.UiTile, grenade.BlastAreaOfEffect, grenade.ShrapnelAreaOfEffect);
            foreach (var mapTile in aoeTile.Item1) {
                foreach (var unit in Unit.All) {
                    if (unit.GetCurrentMapTile() == mapTile) {
                        unit.PopUpText(grenade.BlastDamage.ToString());
                        unit.TakeDamage(grenade.BlastDamage);
                    }
                }
            }
            foreach (var mapTile in aoeTile.Item2) {
                foreach (var unit in Unit.All) {
                    if (unit.GetCurrentMapTile() == mapTile) {
                        unit.PopUpText(grenade.ShrapnelDamage.ToString());
                        unit.TakeDamage(grenade.ShrapnelDamage);
                    }
                }
            }
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        public override void EndAbility() {
            Unit.ActiveUnit.EndTurn();
        }
    }
}