using System.Collections.Generic;

namespace TacticsGame.Data {
    public abstract class Equipment {
        public abstract string Name { get; set; }
        public abstract int Value { get; set; }
        public abstract List<Ability> AddedAbilities { get; set; }
    }
}