using System.Collections.Generic;
using TacticsGame.Data.Abilities;

namespace TacticsGame.Data.Equipments.Weapons {
    public class SubMachineGun : Weapon {
        public sealed override string Name { get; set; }
        public sealed override int Value { get; set; }
        public sealed override List<Ability> AddedAbilities { get; set; }
        public sealed override bool Melee { get; set; }
        public sealed override bool Ranged { get; set; }
        public sealed override int Damage { get; set; }
        public sealed override int RangeDropOff { get; set; }

        public SubMachineGun() {
            Name = "Submachine Gun";
            Value = 100;
            Melee = false;
            Ranged = true;
            Damage = 25;
            RangeDropOff = 5;
            AddedAbilities = new List<Ability> { new PinDown() };
        }
    }
}