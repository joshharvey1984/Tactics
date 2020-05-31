using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    public new string name;
    public enum AbilityTypes
    {
        standard,
        weapon
    }
    public AbilityTypes abilityType;
    public string description;
    public Sprite icon;
    public List<TargettingTypes> targetting;
    public enum TargettingTypes
    {
        self,
        enemy,
        teammate, 
        cone,
        fire
    }
    public List<StatusEffect> statusEffects;
    public List<StatusEffect> targetStatusEffect;
    public float aimModifier;
    public int damageModifer;
}
