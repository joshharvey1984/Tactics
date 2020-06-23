using UnityEngine;

namespace TacticsGame.Data {
    [CreateAssetMenu(fileName = "New Status Effect", menuName = "StatusEffect")]
    public class StatusEffect : ScriptableObject
    {
        public new string name;
        public Sprite icon;
        public int turnLength;
    }
}