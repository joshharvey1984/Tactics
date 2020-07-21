using System.Collections.Generic;
using TacticsGame.Battle.Core;
using UnityEngine;

namespace TacticsGame.Data {
    public abstract class StatusEffect {
        public abstract string Name { get; set; }
        public abstract Sprite Icon { get; set; }
        public enum EndAction {
            TurnStart,
            OnMove
        }
        public abstract List<EndAction> EndActions { get; set; }
        public virtual List<CombatHitModifier> CombatHitModifiers { get; set; }
    }
}