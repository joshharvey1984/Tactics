﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Equipments {
    sealed class SniperRifle : Weapon {
        public override string Name { get; set; }
        public override int Value { get; set; }
        public override List<Ability> AddedAbilities { get; set; }
        public override bool Melee { get; set; }
        public override bool Ranged { get; set; }
        public override int Damage { get; set; }
        public override GameObject WeaponModel { get; set; }
        public override Quaternion SlotRotation { get; set; }

        public SniperRifle() {
            Name = "Sniper Rifle";
            Value = 200;
            Melee = false;
            Ranged = true;
            Damage = 55;
            RangeDropOff = 2;
            AddedAbilities = new List<Ability> { new Scopeshot() };
            WeaponModel = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Battle/Weapons/SniperRifle.prefab", typeof(GameObject)) as GameObject;
            SlotRotation = Quaternion.Euler(-115, -75, 252);
        }
    }
}