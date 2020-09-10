using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Effects;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.Network;
using TacticsGame.Battle.UI;
using TacticsGame.Data;
using TacticsGame.Data.Abilities;
using TacticsGame.Data.Equipments;
using TacticsGame.Data.Equipments.Armours;
using TacticsGame.Data.Equipments.Utilities;
using TacticsGame.Data.Equipments.Weapons;
using UnityEngine;
using UnityEngine.UI;
using FragGrenade = TacticsGame.Data.Equipments.Utilities.Grenades.FragGrenade;
using SmokeGrenade = TacticsGame.Data.Equipments.Utilities.Grenades.SmokeGrenade;

namespace TacticsGame.Battle.Units {
    public class Unit : MonoBehaviour {
        public static readonly List<Unit> All = new List<Unit>();
        public static Unit ActiveUnit;
        public int unitId;

        private Transform _transform;
        private List<SkinnedMeshRenderer> _meshRenderers;
        [SerializeField] public GameObject weaponGameObject;
        [SerializeField] private GameObject throwSpawn;
        
        private GameObject _grenadeObject;

        public Weapon weapon;
        public Armour armour;
        public List<Equipment> equipment;
        public List<Ability> abilities = new List<Ability>();

        private UnitAudio _unitAudio;
        private UnitVoice _unitVoice;
        public string VoicePath { get; } = "Audio/SFX/Voice/Female/";

        private UnitMovement _unitMovement;
        private List<MapTile> _moveTiles;
        private int _moveNum;

        public AbilityUse AbilityUse;
        public Unit targetUnit;
        public Ability selectedAbility;
        public MapTile selectedMapTile;
        public Equipment selectedEquipment;
        public int damageCalculation;

        private static AbilityPanel _abilityPanel;
        private static TargetPanel _targetPanel;
        public GameObject popupTextPrefab;
        private GameObject _canvas;
        private GameManager _gameManager;
        public PlayerGang playerGang;
        public FogOfWar fogOfWar;

        public GameObject weaponSlot;
        public GameObject muzzle;
        private MuzzleFlashGenerator _muzzleFlashGenerator;
        private BulletTracerGenerator _bulletTracerGenerator;

        public int gang;
        public string unitName;

        public bool moveTaken;
        public bool turnTaken;
        public Dictionary<Ability, List<MapTile>> aoeTargetTiles = new Dictionary<Ability, List<MapTile>>();
        public Unit watchingUnit;

        public int movePoints = 8;
        public int hitPoints = 100;
        public int aim = 10;
        public int defence = 10;

        public int sightRange = 20;
        public int fireRange = 20;

        public int currentHitPoints = 100;
        public List<StatusEffect> currentStatusEffects = new List<StatusEffect>();

        [SerializeField] private GameObject statusBarPrefab;
        private GameObject _statusBar;
        private UnitStatusBar _unitStatusBar;

        private void Awake() {
            All.Add(this);

            _transform = gameObject.transform;
            _unitMovement = GetComponent<UnitMovement>();
            _unitMovement.unit = this;

            _unitAudio = GetComponent<UnitAudio>();
            _unitVoice = GetComponent<UnitVoice>();

            var objectRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _meshRenderers = objectRenderers.ToList();

            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _canvas = GameObject.Find("Canvas");
            _abilityPanel = GameObject.Find("AbilityPanel").GetComponent<AbilityPanel>();
            _targetPanel = GameObject.Find("TargetPanel").GetComponent<TargetPanel>();
            playerGang = GameObject.Find("GameManager").GetComponent<PlayerGang>();
            fogOfWar = GameObject.Find("GameManager").GetComponent<FogOfWar>();
            
            abilities.Add(new Shoot());
            abilities.Add(new HunkerDown());
            abilities.Add(new Overwatch());
            
            if (unitId == 0 || unitId == 2)
                weapon = new Shotgun();
            else {
                weapon = new SubMachineGun();
            }
            armour = new LightFlak();
            equipment = new List<Equipment> { new FragGrenade(), new SmokeGrenade() };
            
            abilities.AddRange(weapon.AddedAbilities);
            abilities.AddRange(armour.AddedAbilities);
            foreach (var equip in equipment) abilities.AddRange(equip.AddedAbilities);
            
            SpawnWeaponModel();
            PassiveCheck();
            CreateStatusBar();
            visEffects.staticInit();
        }

        public static Unit GetUnitById(int id) {
            return All.First(unit => unit.unitId == id);
        }

        public void Speak(string folder) {
            if (gang != _gameManager.gangNumber) {
                _unitVoice.PlayVoiceClip(folder, 0.0F);
            }
            else {
                _unitVoice.PlayVoiceClip(folder, 0.7F);
                _unitStatusBar.SetVoiceIcon(true);
            }
        }
        
        public void FireWeapon() {
            GetComponent<UnitAudio>().Play(weapon.FireSound);
            FireBullets(5);
        }

        private void PassiveCheck() {
            foreach (var ability in abilities.Where(ability => ability.AbilityType == Ability.AbilityTypes.Passive)) {
                //ability.PassiveEffect(this);
            }
        }

        private void SpawnWeaponModel() {
            weaponGameObject = Instantiate(weapon.WeaponModel, weaponSlot.transform);
            weaponSlot.transform.rotation = weapon.SlotRotation;
            muzzle = weaponGameObject.transform.Find("MuzzleFlashGenerator").gameObject;
        }
        
        private void CreateStatusBar() {
            _statusBar = Instantiate(statusBarPrefab, GameObject.Find("Canvas").transform);
            _statusBar.GetComponent<Slider>().maxValue = hitPoints;
            _statusBar.GetComponent<Slider>().value = hitPoints;
            _unitStatusBar = _statusBar.GetComponent<UnitStatusBar>();
            _unitStatusBar.unit = this;
        }

        public void MoveUnit(List<MapTile> tiles) {
            _gameManager.StopTimer();
            _moveTiles = tiles;
            _moveNum = 0;
            _unitMovement.SetAnimationBool("Crouch", false);
            _unitAudio.SprintSound();
            Speak("Moving");
            MoveNextTile();
        }

        public void MoveNextTile() {
            _moveNum++;
            if (_moveNum == _moveTiles.Count - 1) EndMoveTurn();
            else _unitMovement.SetNextTile(_moveTiles[_moveNum]);
        }

        private void EndMoveTurn() {
            _unitAudio.StopSprintSound();
            _unitMovement.EndMove();
            if (_gameManager.gangNumber == _gameManager.currentGangTurn) {
                _abilityPanel.CreateAbilityButtons();
                _targetPanel.UpdateTargetPanel();
                _gameManager.StartTimer();
            }
        }

        public void ExecuteAbility() {
            _gameManager.StopTimer();
            if (selectedAbility.TargetingType == Ability.TargetingTypes.Cone || selectedAbility.TargetingType == Ability.TargetingTypes.Point) 
                selectedAbility.Targeting();
            else {
                if (selectedAbility.AbilityCalculations != null)
                    foreach (var calculation in selectedAbility.AbilityCalculations) calculation.Calculate();
                _gameManager.SendAbility(new AbilityAction());
            }
        }

        public MapTile GetCurrentMapTile() => 
            MapTile.GetMapTileFromPos(Convert.ToInt32(_transform.position.x), 
                Convert.ToInt32(_transform.position.z));

        public void StartTurn() {
            UpdateVisibility();
            StatusEffectUpdate();
            _unitStatusBar.UpdateStatusIcons();
            MovementUI.DrawMovementUI(this);
            _abilityPanel.CreateAbilityButtons();
            _targetPanel.UpdateTargetPanel();
            Speak("Selected");
        }

        private void StatusEffectUpdate() {
            
        }

        public void EndTurn() {
            MovementUI.DestroyMovementUI();
            selectedAbility = null;
            targetUnit = null;
            turnTaken = true;
            _gameManager.EndUnitTurn();
        }

        public List<Unit> EnemiesInLineOfSight() =>
            All.Where(unit => unit.gang != gang)
                .Where(unit => GetCurrentMapTile().CanSeeOtherTile(unit.GetCurrentMapTile(), sightRange, true, true))
                .Distinct()
                .ToList();
        
        public List<MapTile> AllTilesInSight() => 
            MapTile.All
                .Where(mapTile => GetCurrentMapTile().CanSeeOtherTile(mapTile, 20.0F, true))
                .ToList();

        public void FireBullets(int numBullets) {
            muzzle.GetComponent<MuzzleFlashGenerator>().toFire = numBullets;
            muzzle.GetComponent<BulletTracerGenerator>().toFire = numBullets;
        }

        public void PopUpText(string text) {
            var popUpText = Instantiate(popupTextPrefab, _canvas.transform);
            popUpText.GetComponent<TextPopup>().TrueStart(this, text);
        }

        public void LookAtGameObject(GameObject gameObj) {
            gameObject.transform.LookAt(gameObj.transform);
            AbilityUse?.RemoveBehaviour(selectedAbility.AbilityBehaviours.Find(x => x is UnitAnimation));
        }
        
        public void EndSpeak() {
            _unitStatusBar.SetVoiceIcon(false);
            AbilityUse?.RemoveBehaviour(selectedAbility.AbilityBehaviours.Find(x => x is UnitVoiceClip));
        } 

        public void PlayAnimation(string anim) => _unitMovement.SetAnimationBool(anim, true);

        public void ThrowAnimation(Equipment item) {
            var grenade = item as Grenade;
            selectedEquipment = grenade;
            _unitMovement.SetAnimationTrigger("Throw");
            _grenadeObject = Instantiate(grenade?.GrenadePrefab, throwSpawn.transform);
        }

        public void ThrowEvent() {
            _grenadeObject.transform.parent = null;
            _grenadeObject.GetComponent<Projectile>().GetTrajectory();
        }

        public void ThrowExecute() {
            var grenade = selectedEquipment as Grenade;
            var explosion = Instantiate(grenade?.ExplodePrefab, selectedMapTile.GetWorldPosition(), Quaternion.identity);
            var parts = explosion.GetComponent<ParticleSystem>();
            Destroy(explosion, parts.main.duration);
            selectedAbility.ExplodeEffect(selectedMapTile);
        }

        public void TakeDamage(int damageToTake) {
            if (damageToTake <= 0) {
                PopUpText("MISS");
                return;
            }
            
            var bloodPos = _transform.position;
            bloodPos.y += 0.5F;
            visEffects.createEffect((int)visualEffectNames.bloodImpact, bloodPos );
            PopUpText(damageToTake.ToString());
            Speak("Damaged");
            currentHitPoints -= damageToTake;
            if (currentHitPoints <= 0) UnitDeath();
            else _unitMovement.SetAnimationTrigger("Damaged");
            _unitStatusBar.LoseHealth(damageToTake);
        }

        private void UnitDeath() {
            All.Remove(this);
            Speak("Killed");
            _unitMovement.SetAnimationTrigger("Die");
        }

        public void AddStatusEffect(StatusEffect statusEffect) {
            currentStatusEffects.Add(statusEffect);
            PopUpText(statusEffect.Name);
            _unitStatusBar.UpdateStatusIcons();
        }

        public void UpdateVisibility() {
            playerGang.UnitVisibilityUpdate();
            fogOfWar.UpdateFogOfWar();
        }
        

        public void HideUnit() {
            foreach (var skinnedMeshRenderer in _meshRenderers) skinnedMeshRenderer.enabled = false;

            foreach (var meshRenderer in weaponGameObject.GetComponentsInChildren<MeshRenderer>()) 
                meshRenderer.enabled = false;

            _statusBar.SetActive(false);
        }

        public void ShowUnit() {
            foreach (var skinnedMeshRenderer in _meshRenderers) skinnedMeshRenderer.enabled = true;
            
            
            foreach (var meshRenderer in weaponGameObject.GetComponentsInChildren<MeshRenderer>()) 
                meshRenderer.enabled = true;

            _statusBar.SetActive(true);
        }
    }
}