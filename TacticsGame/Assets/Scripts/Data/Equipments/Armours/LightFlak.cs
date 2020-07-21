using System.Collections.Generic;

namespace TacticsGame.Data.Equipments.Armours {
    public class LightFlak : Armour {
        public sealed override string Name { get; set; }
        public sealed override int Value { get; set; }
        public sealed override List<Ability> AddedAbilities { get; set; }
        public sealed override int DamageReduction { get; set; }
        public sealed override int MoveReduction { get; set; }

        public LightFlak() {
            Name = "Light Flak";
            Value = 50;
            AddedAbilities = new List<Ability>();
            DamageReduction = 15;
            MoveReduction = 0;
        }
    }
}