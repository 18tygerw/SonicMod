using EntityStates;
using System;
using UnityEngine;

namespace SonicMod.SkillStates.BaseStates
{
    //see example skills below
    public class BaseBoostSkill : BaseSkillState
    {
        //total duration of the move
        public static float TimedBaseDuration; //Total loop duration of boost

        //time relative to duration that the skill starts
        //for example, set 0.5 and the "cast" will happen halfway through the skill
        public static float TimedBaseCastStartTime;
        public static float TimedBaseCastEndTime;

        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected bool hasFired;
        protected bool isFiring;
        protected bool hasExited;
        

        //initialize your time values here
        protected virtual void InitDurationValues(float baseDuration, float baseCastStartTime, float baseCastEndTime = 1)
        {
            TimedBaseDuration = baseDuration;
            TimedBaseCastStartTime = baseCastStartTime;
            TimedBaseCastEndTime = baseCastEndTime;

            duration = TimedBaseDuration; //duration = TimedBaseDuration / base.attackSpeedStat;
            castStartTime = baseCastStartTime * duration;
            castEndTime = baseCastEndTime * duration;
        }

        protected virtual void OnCastEnter() { }
        protected virtual void OnCastFixedUpdate() { }
        protected virtual void OnCastUpdate() { }
        protected virtual void OnCastExit() { }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //wait start duration and fire
            if(!hasFired && fixedAge > castStartTime)
            {
                hasFired = true;
                OnCastEnter();
            }

            bool fireStarted = fixedAge >= castStartTime;
            bool fireEnded = fixedAge >= castEndTime;
            isFiring = false;

            //to guarantee attack comes out if at high attack speed the fixedage skips past the endtime
            if ((fireStarted && !fireEnded) || (fireStarted && fireEnded && !this.hasFired))
            {
                isFiring = true;
                OnCastFixedUpdate();
            }

            if(fireEnded && !hasExited)
            {
                hasExited = true;
                OnCastExit();
            }

            if(!base.IsKeyDownAuthority() && isAuthority) //if(fixedAge > duration) is the default. Exits the skill after releasing the key with IsKeyDownAuthority
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void Update()
        {
            base.Update();
            if (isFiring)
            {
                OnCastUpdate();
            }
        }

        

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }



    public class ExampleTimedSkillState : BaseBoostSkill
    {
        public static float SkillBaseDuration = 1.5f;
        public static float SkillStartTime = 0f; //0.2f;
        public static float SkillEndTime =  1f; //0.9f;

        public override void OnEnter()
        {
            base.OnEnter();

            InitDurationValues(SkillBaseDuration, SkillStartTime, SkillEndTime);
        }

        protected override void OnCastEnter()
        {
            //perform my skill after 0.3 seconds of windup
        }

        protected override void OnCastFixedUpdate()
        {
            //perform some continuous action after the windup, which will end .15 seconds before the full duration
        }

        protected override void OnCastExit()
        {
            //probably play an animation at the end of the action
        }
    }

    public class ExampleDelayedSkillState : BaseBoostSkill
    {
        public static float SkillBaseDuration = 1.5f;
        public static float SkillStartTime = 0.2f;

        public override void OnEnter()
        {
            base.OnEnter();

            InitDurationValues(SkillBaseDuration, SkillStartTime);
        }

        protected override void OnCastEnter()
        {
            //perform my skill after 0.3 seconds of windup
        }
    }
}