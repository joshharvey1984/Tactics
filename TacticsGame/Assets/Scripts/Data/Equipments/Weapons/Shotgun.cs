using System.Collections.Generic;
using TacticsGame.Data.Abilities;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Equipments.Weapons {
    public sealed class Shotgun : Weapon {
        public override string Name { get; set; }
        public override int Value { get; set; }
        public override List<Ability> AddedAbilities { get; set; }
        public override bool Melee { get; set; }
        public override bool Ranged { get; set; }
        public override int Damage { get; set; }
        public override GameObject WeaponModel { get; set; }
        public override Quaternion SlotRotation { get; set; }

        public Shotgun() {
            Name = "Shotgun";
            Value = 120;
            Melee = false;
            Ranged = true;
            Damage = 35;
            RangeDropOff = 7;
            AddedAbilities = new List<Ability> { new Scattershot() };
            WeaponModel = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Battle/Weapons/Shotgun.prefab", typeof(GameObject)) as GameObject;
            SlotRotation = Quaternion.Euler(-55, 116, 54);
        }
    }
}