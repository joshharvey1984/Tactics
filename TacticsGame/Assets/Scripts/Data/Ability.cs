using System.Collections.Generic;
using UnityEngine;

namespace TacticsGame.Data {
    public abstract class Ability {
        public abstract string Name { get; set;}
        public abstract string Description { get; set;}
        public enum AbilityTypes {
            Active,
            Passive
        }
        public abstract AbilityTypes AbilityType { get; set; }
        public abstract Sprite Icon { get; set; }
        public abstract List<AbilityBehaviour> AbilityBehaviours { get; set; }
    }
}
