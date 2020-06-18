using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public class FireAbility : Ability {
        public sealed override string Name { get; set; }
        public sealed override string Description { get; set; }
        public sealed override AbilityTypes AbilityType { get; set; }
        public sealed override Sprite Icon { get; set; }

        public FireAbility() {
            Name = "Fire";
            AbilityType = AbilityTypes.Active;
            Description = "Fire weapon at selected Enemy";
            Icon = AssetDatabase.LoadAssetAtPath("Assets/Textures/Abilities/Icon_Target.png", typeof(Sprite)) as Sprite;
        }
        
        public override void Execute() {
            Unit.SelectedUnit.FireBullets(5);
        }

    }
}
