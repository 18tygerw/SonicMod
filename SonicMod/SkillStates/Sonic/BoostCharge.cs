using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using SonicMod.SkillStates.BaseStates;

namespace SonicMod.SkillStates
{

    public class BoostCharge : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //Timer();
            this.duration = this.baseDuration;
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
                base.characterBody.AddBuff(Modules.Buffs.boostArmorBuff);
            }
            //Util.PlaySound();
            if (isAuthority)
            {
                this.blastAttack = new BlastAttack
                {
                    radius = 20f,
                    procCoefficient = 0.7f,
                    position = aimRay.origin,
                    attacker = base.gameObject,
                    crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master),
                    baseDamage = base.characterBody.damage * (BoostCharge.damageCoefficient * this.MovespeedToDamageBoost()),
                    falloffModel = BlastAttack.FalloffModel.None,
                    baseForce = 5f,
                    damageType = DamageType.Stun1s,
                    attackerFiltering = AttackerFiltering.NeverHitSelf
                };
                this.blastAttack.teamIndex = TeamComponent.GetObjectTeam(this.blastAttack.attacker);
                //this.effectData = new EffectData();
                //this.effectData.scale = 2.5f;
                //this.effectData.color
                base.characterMotor.velocity += 75f * aimRay.direction;
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
            bool flag = !base.characterMotor.disableAirControlUntilCollision;
            if (flag)
            {
                base.characterMotor.velocity *= 0.4f;
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            

            bool isAuthority = base.isAuthority;
            if (isAuthority)
            {
                boostTimer += Time.fixedDeltaTime;
                while (boostTimer >= this.duration)
                {
                    boostTimer -= this.duration;
                    this.blastAttack.Fire();
                }
                if (!this.boostIsHeld())
                {
                    this.outer.SetNextStateToMain();
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
        protected virtual bool boostIsHeld()
        {
            return base.inputBank.skill3.down;
        }
        private float MovespeedToDamageBoost()
        {
            return Mathf.Max(1f, base.characterBody.moveSpeed / base.characterBody.baseMoveSpeed);
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

    }
}