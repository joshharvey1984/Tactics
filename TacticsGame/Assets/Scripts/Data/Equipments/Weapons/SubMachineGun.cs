using System.Collections.Generic;
using TacticsGame.Data.Abilities;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Equipments.Weapons {
    public class SubMachineGun : Weapon {
        public sealed override string Name { get; set; }
        public sealed override int Value { get; set; }
        public sealed override List<Ability> AddedAbilities { get; set; }
        public sealed override bool Melee { get; set; }
        public sealed override bool Ranged { get; set; }
        public sealed override int Damage { get; set; }
        public sealed override int RangeDropOff { get; set; }
        public sealed override GameObject WeaponModel { get; set; }
        public sealed override Quaternion SlotRotation { get; set; }
        public override AudioClip FireSound { get; set; }

        public SubMachineGun() {
            Name = "Submachine Gun";
            Value = 100;
            Melee = false;
            Ranged = true;
            Damage = 25;
            RangeDropOff = 5;
            AddedAbilities = new List<Ability> { new PinDown() };
            WeaponModel = Resources.Load("Assets/Prefabs/Battle/Weapons/SMG", typeof(GameObject)) as GameObject;
            SlotRotation = Quaternion.Euler(-12, -96, 260);
        }
    }
}