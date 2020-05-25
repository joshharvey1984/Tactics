using System;
using System.Collections.Generic;
using TacticsGame.Battle.Map;
using UnityEngine;

namespace TacticsGame.Battle {
    public class Unit : MonoBehaviour
    {
        public static List<Unit> All = new List<Unit>();

        public MapTile currentTile;

        public int movePoints = 8;

        private void Awake() {
            All.Add(this);
        }
    }
}
