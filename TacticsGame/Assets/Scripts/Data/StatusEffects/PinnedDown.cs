using System.Collections.Generic;
using TacticsGame.Battle.Core;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.StatusEffects {
    public sealed class PinnedDown : StatusEffect {
        public override string Name { get; set; }
        public override Sprite Icon { get; set; }
        public override List<EndAction> EndActions { get; set; }
        public override List<CombatHitModifier> CombatHitModifiers { get; set; }

        public PinnedDown() {
            Name = "Pinned Down";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_PinDown");
            EndActions = new List<EndAction> { EndAction.OnMove };
            CombatHitModifiers = new List<CombatHitModifier> {
                new CombatHitModifier {
                    Name = "Pinned Down",
                    Description = "Pinned Down",
                    ModifierValue = -0.2F
                }
            };
        }
    }
}