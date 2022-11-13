using SonicMod.SkillStates.BaseStates;
using RoR2;
using UnityEngine;

namespace SonicMod.SkillStates
{
    public class SonicCombo : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            this.hitboxName = "Sword";
            //Chat.AddMessage(swingIndex.ToString()); //log swingIndex
            this.damageType = DamageType.Generic;
            if (swingIndex != 3) this.damageCoefficient = Modules.StaticValues.comboDamageCoefficient;
            else this.damageCoefficient = Modules.StaticValues.comboFinisherCoefficient;
            //this.damageCoefficient = Modules.StaticValues.comboDamageCoefficient;
            this.procCoefficient = 0.75f; //1f default
            this.pushForce = 300f;
            this.bonusForce = Vector3.zero;
            this.baseDuration = 0.75f; //1f default
            this.attackStartTime = 0.2f; //0.2f default
            this.attackEndTime = 0.8f; //0.4f default
            this.baseEarlyExitTime = 0.3f; //0.4f default
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 4f;

            this.swingSoundString = "HenrySwordSwing";
            this.hitSoundString = "";
            //this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight"; uncomment this when i want to reset it
            this.muzzleString = swingIndex != 3 ? "SwingLeft" : "SwingRight"; //Swing left 3 times, 4th swings right
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            switch (index) // Changes swingIndex depending on which swing we're on. Starts from 0, goes up to 3, totaling 4 hits.
            {
                case 0:
                    index = 1;
                    break;
                case 1:
                    index = 2;
                    break;
                case 2:
                    index = 3;
                    break;
                case 3:
                    index = 0;
                    break;
            }
            //if (index == 0) index = 1; original function
            //else index = 0;

            this.outer.SetNextState(new SonicCombo
            {
                swingIndex = index

            });
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}