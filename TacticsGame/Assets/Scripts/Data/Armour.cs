using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Armour")]
public class Armour : ScriptableObject
{
    public new string name;
    public int value;
    public int damageReduction;
}
