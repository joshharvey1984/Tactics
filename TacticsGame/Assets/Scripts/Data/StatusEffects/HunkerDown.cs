using System.Collections.Generic;
using TacticsGame.Battle.Core;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.StatusEffects {
    public sealed class HunkerDown : StatusEffect {
        public override string Name { get; set; }
        public override Sprite Icon { get; set; }
        public override List<EndAction> EndActions { get; set; }
        public override List<CombatHitModifier> CombatHitModifiers { get; set; }

        public HunkerDown() {
            Name = "Hunker Down";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Shield");
            EndActions = new List<EndAction> {EndAction.TurnStart};
            CombatHitModifiers = new List<CombatHitModifier> {
                new CombatHitModifier {
                    Name = "Hunker Down", 
                    Description = "Hunkered Down Target", 
                    ModifierValue = -0.2F
                }
            };
        }
    }
}