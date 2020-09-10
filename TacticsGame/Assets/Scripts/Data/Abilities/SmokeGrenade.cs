using System.Collections.Generic;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    sealed class SmokeGrenade : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public SmokeGrenade(Equipment smokeGrenade) {
            Name = "Smoke Grenade";
            AbilityType = AbilityTypes.Active;
            Description = "Throw a smoke grenade.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Cloud");
            TargetingType = TargetingTypes.Point;
            EquipmentObject = smokeGrenade;
            AbilityBehaviours = new List<AbilityBehaviour> {
                new UnitVoiceClip(1, "Grenade")
            };
        }

        public override void ExplodeEffect(MapTile landingTile) {
            var grenade = new Equipments.Utilities.Grenades.SmokeGrenade();
            var aoeTile = ThrowTarget.FindAOETiles(landingTile.UiTile, grenade.BlastAreaOfEffect, grenade.ShrapnelAreaOfEffect);
            foreach (var mapTile in aoeTile.Item1) {
                mapTile.CreateProp(Resources.Load("Prefabs/Battle/Map/Props/SmokeEffect", typeof(GameObject)) as GameObject);
            }
            //AbilityPause.StartPause(1.5F, this, "EndAbility");
        }
    }
}