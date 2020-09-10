using System.Collections.Generic;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class Scattershot : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public Scattershot() {
            Name = "Scattershot";
            AbilityType = AbilityTypes.Active;
            Description = "Graze all units in cone target for half damage.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Tex_skill_44");
            TargetingType = TargetingTypes.Cone;
            AbilityBehaviours = new List<AbilityBehaviour> {
                new UnitVoiceClip(1, "Firing"),
                new UnitAnimation(1, "AimWeapon"),
                new FireWeapon(2),
                new DamageUnitsInAOE(2)
            };
        }
    }
}