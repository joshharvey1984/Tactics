using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.StatusEffects {
    public class Overwatch : StatusEffect {
        public sealed override string Name { get; set; }
        public sealed override Sprite Icon { get; set; }
        public sealed override List<EndAction> EndActions { get; set; }

        public Overwatch() {
            Name = "Overwatch";
            Icon = AssetDatabase.LoadAssetAtPath("Assets/Textures/Abilities/Icon_Eye.png", typeof(Sprite)) as Sprite;
            EndActions = new List<EndAction> {EndAction.TurnStart};
        }
    }
}