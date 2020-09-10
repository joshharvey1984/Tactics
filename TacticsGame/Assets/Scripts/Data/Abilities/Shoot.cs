using System.Collections.Generic;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class Shoot : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public Shoot() {
            Name = "Shoot";
            AbilityType = AbilityTypes.Active;
            Description = "Shoot weapon at selected Enemy";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Target");
            TargetingType = TargetingTypes.Single;
            AbilityCalculations = new List<AbilityCalculation> {
                new CalculationDamage()
            };
            AbilityBehaviours = new List<AbilityBehaviour> {
                new UnitVoiceClip(1, "Firing", true),
                new UnitAnimation(1, "AimWeapon"),
                new FireWeapon(2),
                new UnitTakeDamage(2)
            };
        }
    }
}