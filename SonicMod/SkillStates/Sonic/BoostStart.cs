using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using SonicMod.SkillStates.BaseStates;

namespace SonicMod.SkillStates
{
    
    public class BoostStart : BaseSkillState
    {
        public static float armorDuration = 0.5f;
        public static float invincibleDuration = 0.25f;
        public static float invincibleTimer = 0.25f;
        public const float energyCost = 25f; //use this later for Sonic's Ring Energy gauge
        public static float chargeDamageCoefficient;
        public static float speedMultiplier = 3.5f;
        private int charge = 0;
        private Vector3 boostDirection;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterMotor.disableAirControlUntilCollision = false;
            base.characterBody.isSprinting = true;
            base.StartAimMode(0.5f, false); //Face character forward while using the skill, might put it in fixedupdate
            /*Transform modelTransform = base.GetModelTransform();
            bool flag = modelTransform;
            if (flag)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = this.chargeDuration;
                temporaryOverlay.animateShaderAlpha = true;
                //temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                //temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/boostVFX");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }*/
            Ray aimRay = base.GetAimRay();
            bool isAuthority = base.isAuthority;
            bool active = NetworkServer.active;
            if (isAuthority && active)
            {
                base.characterBody.AddTimedBuff(Modules.Buffs.boostArmorBuff, 0.2f);
                //base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = true;
            base.characterMotor.disableAirControlUntilCollision = false;
            if (base.characterDirection && base.characterMotor)
            {
                boostDirection = base.characterDirection.forward * base.characterBody.moveSpeed * BoostStart.speedMultiplier * Time.fixedDeltaTime;
                Vector3 direction = boostDirection;
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.rootMotion += direction;
                if (base.isAuthority)
                {
                    if (!this.boostIsHeld() &&  isAuthority)
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    else
                    {
                        this.outer.SetNextState(new BoostCharge());
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.characterMotor.disableAirControlUntilCollision = false;
            if (base.characterMotor && !base.characterMotor.disableAirControlUntilCollision)
            {
                base.characterMotor.velocity += GetIdealVelocity();
            }
            bool flag = this.boostInstance;
            if (flag)
            {
                EntityState.Destroy(this.boostInstance);
            }
            //base.PlayAnimation("Gesture, Override", "BufferEmpty");
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        protected virtual bool boostIsHeld()
        {
            return base.inputBank.skill3.down;
        }
        private Vector3 GetIdealVelocity()
        {
            return base.characterDirection.forward * base.characterBody.moveSpeed * BoostStart.speedMultiplier;
        }

        public bool invincibilityTimerIsRunning = false;

        private float chargeDuration = 0.5f;

        private Vector3 left = new Vector3(0f, 0f, 1f);

        private GameObject boostInstance;
    }
}

/*{ Unaltered copy of original skilldata
    public class Boost : BaseSkillState
{
    public static float damageCoefficient = Modules.StaticValues.boostDamageCoefficient;
    public static float procCoefficient = 1f;
    public static float baseDuration = 0.6f;
    public static float force = 800f;
    public static float recoil = 3f;
    public static float range = 256f;
    public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

    private float duration;
    private float fireTime;
    private bool hasFired;
    private string muzzleString;

    public override void OnEnter()
    {
        base.OnEnter();
        this.duration = Boost.baseDuration / this.attackSpeedStat;
        this.fireTime = 0.2f * this.duration;
        base.characterBody.SetAimTimer(2f);
        this.muzzleString = "Muzzle";

        base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void Fire()
    {
        if (!this.hasFired)
        {
            this.hasFired = true;

            base.characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
            Util.PlaySound("HenryShootPistol", base.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil(-1f * Boost.recoil, -2f * Boost.recoil, -0.5f * Boost.recoil, 0.5f * Boost.recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = Boost.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = Boost.range,
                    force = Boost.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = base.RollCrit(),
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = false,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = Boost.tracerEffectPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (base.fixedAge >= this.fireTime)
        {
            this.Fire();
        }

        if (base.fixedAge >= this.duration && base.isAuthority)
        {
            this.outer.SetNextStateToMain();
            return;
        }
    }

    public override InterruptPriority GetMinimumInterruptPriority()
    {
        return InterruptPriority.PrioritySkill;
    }
}
} */