using System.Collections.Generic;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    internal sealed class Scopeshot : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public Scopeshot() {
            Name = "Scopeshot";
            AbilityType = AbilityTypes.Passive;
            Description = "Allows unit to fire at any enemy in unobstructed line-of-sight ignoring range limit.";
            Icon = Resources.Load("Textures/Abilities/Icon_address", typeof(Sprite)) as Sprite;
            TargetingType = TargetingTypes.None;
        }

        public override void PassiveEffect(Unit effectedUnit) {
            effectedUnit.fireRange = 1000;
        }
    }
}