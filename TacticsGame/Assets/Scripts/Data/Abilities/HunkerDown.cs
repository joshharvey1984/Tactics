using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class HunkerDown : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }

        public HunkerDown() {
            Name = "Hunker Down";
            AbilityType = AbilityTypes.Active;
            Description = "Hunker down and increase defensive bonus.";
            Icon = AssetDatabase.LoadAssetAtPath("Assets/Textures/Abilities/Icon_Shield.png", typeof(Sprite)) as Sprite;
            TargetingType = TargetingTypes.Self;
        }
        public override void Execute() {
            Unit.SelectedUnit.PopUpText("HUNKER DOWN");
            Unit.SelectedUnit.PlayAnimation("Crouch");
            AddStatusEffect("Hunker Down");
            AbilityPause.StartPause(1.5F, this);
        }
    }
}