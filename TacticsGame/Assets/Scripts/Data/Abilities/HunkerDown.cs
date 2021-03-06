﻿using System.Collections.Generic;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class HunkerDown : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public HunkerDown() {
            Name = "Hunker Down";
            AbilityType = AbilityTypes.Active;
            Description = "Hunker down and increase defensive bonus.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Shield");
            TargetingType = TargetingTypes.None;
            AbilityBehaviours = new List<AbilityBehaviour> {
                new UnitVoiceClip(1, "Cover"),
                new UnitAnimation(1, "Crouch"),
                new AddStatusEffect(1, new StatusEffects.HunkerDown())
            };
        }
    }
}