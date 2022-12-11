using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using SonicMod.SkillStates.BaseStates;

namespace SonicMod.SkillStates
{

    public class BoostWIP : BaseSkillState
    {
        public static float armorDuration = 0.5f;
        public static float invincibleDuration = 0.25f;
        public static float invincibleTimer = 0.25f;
        public const float energyCost = 25f; //use this later for Sonic's Ring Energy gauge
        public static float chargeDamageCoefficient;
        public static float speedMultiplier = 3.5f;
        private Vector3 boostDirection;
        public override void OnEnter()
        {
            base.OnEnter();
            boostIsHeld = true;
            Ray aimRay = base.GetAimRay();
            base.characterMotor.disableAirControlUntilCollision = false;
            //base.GetModelChildLocator().FindChild("BoostChargeEffect").GetComponent<ParticleSystem>().Play();
            /*Transform modelTransform = base.GetModelTransform();
            bool flag = modelTransform;
            if (flag)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = this.duration;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 15f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matOnFire");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());

            }*/
            bool active = NetworkServer.active;
            if (active)
            {
                
            }
            //Util.PlaySound();
            if (isAuthority)
            {
                
            }
        }

        public override void OnExit()
        {
            bool active = NetworkServer.active;
            if (active)
            {
                base.characterBody.RemoveBuff(Modules.Buffs.boostArmorBuff);
                base.characterBody.AddTimedBuff(Modules.Buffs.boostArmorBuff, 0.5f);
            }
            base.characterMotor.disableAirControlUntilCollision = false;
            if (!base.characterMotor.disableAirControlUntilCollision)
            {
                base.characterMotor.velocity += GetIdealVelocity();
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isBoosting())
            {
                boostIsHeld = true;
            }
            base.characterBody.isSprinting = true;
            base.characterMotor.disableAirControlUntilCollision = false;
            bool active = NetworkServer.active;
            if (base.characterDirection && base.characterMotor)
            {
                boostDirection = base.characterDirection.forward * base.characterBody.moveSpeed * BoostStart.speedMultiplier * Time.fixedDeltaTime;
                Vector3 direction = boostDirection;
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.rootMotion += direction;
                if (base.isAuthority)
                {
                    if (!boostIsHeld)
                    {
                        Chat.AddMessage("boost ENDED");
                        this.outer.SetNextStateToMain();
                        return;
                    }
                    if (boostIsHeld)
                    {
                        Chat.AddMessage("boost STARTED");
                        base.characterMotor.rootMotion += direction;
                        this.blastAttack.Fire();
                        if (active)
                        {
                            base.characterBody.AddBuff(Modules.Buffs.boostArmorBuff);
                        }
                    }
                }

                /*Ray aimRay = base.GetAimRay();
                this.blastAttack.position = aimRay.origin;
                this.effectData.origin = aimRay.origin;
                //bool flag = base.fixedAge >= this.duration && base.isAuthority && !this.boostIsHeld();
                if (isAuthority && !this.boostIsHeld());
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
                
                bool flag = base.fixedAge >= this.duration && base.isAuthority/* && !this.boostIsHeld()*/;
                /*if (flag)
                {
                    this.outer.SetNextStateToMain();
                }
                else
                {
                    float f = (float)this.charged * (0.8f + 0.2f * this.attackSpeedStat);
                    int num = Mathf.FloorToInt(f);
                    int num2 = Mathf.Max(2, 40 - num);
                    bool flag2 = this.frameCounter % num2 == 0;
                    if (flag2)
                    {
                        BlastAttack.Result result = this.blastAttack.Fire();
                        int hitCount = result.hitCount;
                        EffectManager.SpawnEffect(this.explodePrefab, this.effectData, false);
                    }
                    this.frameCounter++;
                }*/
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        protected virtual bool isBoosting()
        {
            return base.inputBank.skill3.down;
        }
        private float MovespeedToDamageBoost()
        {
            return Mathf.Max(1f, base.characterBody.moveSpeed / base.characterBody.baseMoveSpeed);
        }
        private Vector3 GetIdealVelocity()
        {
            return base.characterDirection.forward * base.characterBody.moveSpeed * BoostWIP.speedMultiplier;
        }
        /*protected void Timer()
        {
            if (timer >= 0f)
            {
                timer += Time.fixedDeltaTime; //Timer to count down for boost invincibility
            }
            else
            {
                timer = 0f;
                timerIsRunning = false;
            }
            if (!this.boostIsHeld())
            {
                timer = 0f;
            }
        }*/

        public bool timerIsRunning = false;
        public static float boostTimer;
        public static float timer = 0f;
        public GameObject explodePrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");
        public int charged;
        public float baseDuration = 0.1f + timer;
        private float duration;
        private BlastAttack blastAttack;
        private EffectData effectData;
        public static float damageCoefficient = Modules.StaticValues.boostDamageCoefficient;
        private int frameCounter = 0;
        private bool boostIsHeld = false;

    }
}