using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    sealed class SmokeGrenade : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }

        public SmokeGrenade() {
            Name = "Smoke Grenade";
            AbilityType = AbilityTypes.Active;
            Description = "Throw a smoke grenade.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Cloud");
            TargetingType = TargetingTypes.Throw;
            SpecialTarget = SpecialTargeting.Throw;
        }
        
        public override void Targeting() {
            Unit.ActiveUnit.selectedEquipment = new Equipments.Utilities.Grenades.SmokeGrenade();
            var throwTarget = new ThrowTarget(10, 1, 0);
            throwTarget.SubscribeToHoverTile();
        }
        
        public override void Execute() {
            Unit.ActiveUnit.ThrowAnimation(new Equipments.Utilities.Grenades.SmokeGrenade());
        }

        public override void ExplodeEffect(MapTile landingTile) {
            var grenade = new Equipments.Utilities.Grenades.SmokeGrenade();
            var aoeTile = ThrowTarget.FindAOETiles(landingTile.UiTile, grenade.BlastAreaOfEffect, grenade.ShrapnelAreaOfEffect);
            foreach (var mapTile in aoeTile.Item1) {
                mapTile.CreateProp(Resources.Load("Prefabs/Battle/Map/Props/SmokeEffect", typeof(GameObject)) as GameObject);
            }
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        public override void EndAbility() {
            Unit.ActiveUnit.EndTurn();
        }
    }
}