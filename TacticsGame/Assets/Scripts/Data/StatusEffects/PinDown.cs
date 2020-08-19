using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.StatusEffects {
    public sealed class PinDown : StatusEffect {
        public override string Name { get; set; }
        public override Sprite Icon { get; set; }
        public override List<EndAction> EndActions { get; set; }

        public PinDown() {
            Name = "Pin Down";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_PinDown");
            EndActions = new List<EndAction> { EndAction.TurnStart };
        }
    }
}