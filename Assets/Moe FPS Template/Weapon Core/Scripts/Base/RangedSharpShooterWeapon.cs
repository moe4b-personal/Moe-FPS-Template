using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using Moe.Tools;

namespace WeaponCore
{
    public abstract partial class BaseRangedSharpShooterWeapon : RangedWeapon
    {
        [SerializeField]
        protected RangedSharpShooterWeapon.AimModule aim;
        public RangedSharpShooterWeapon.AimModule Aim { get { return aim; } }
        [Serializable]
        public abstract class BaseAimModule : Module<RangedSharpShooterWeapon>
        {
            public RangedSharpShooterWeapon.AimModule This { get { return this as RangedSharpShooterWeapon.AimModule; } }

            [SerializeField]
            protected bool isOn = false;
            public bool IsOn { get { return isOn; } }

            [SerializeField]
            protected float swayScale = 0.2f;
            public float SwayScale { get { return swayScale; } }

            [SerializeField]
            protected RangedSharpShooterWeapon.AimModule.StateMachinesModule stateMachines;
            public RangedSharpShooterWeapon.AimModule.StateMachinesModule StateMachines { get { return stateMachines; } }
            [Serializable]
            public abstract class BaseStateMachinesModule : Module<RangedSharpShooterWeapon>
            {
                [SerializeField]
                protected StateMachineCallbackRewind start;
                public StateMachineCallbackRewind Start { get { return start; } }

                [SerializeField]
                protected StateMachineCallbackRewind idle;
                public StateMachineCallbackRewind Idle { get { return idle; } }

                [SerializeField]
                protected StateMachineCallbackRewind end;
                public StateMachineCallbackRewind End { get { return end; } }

                [SerializeField]
                protected StateMachineCallbackRewind shot;
                public StateMachineCallbackRewind Shot { get { return shot; } }

                public override void Activate()
                {
                    base.Activate();

                    GetStates();
                }
                protected virtual void GetStates()
                {
                    start = Weapon.anim.GetBehaviour<StateMachineCallbackRewind>("Aim Start");
                    idle = Weapon.anim.GetBehaviour<StateMachineCallbackRewind>("Aim Idle");
                    shot = Weapon.anim.GetBehaviour<StateMachineCallbackRewind>("Aim Shot");
                    end = Weapon.anim.GetBehaviour<StateMachineCallbackRewind>("Aim End");

                    start.StateEnter.Simple += OnStart;
                    shot.StateEnter.Simple += OnShot;
                    end.StateEnter.Simple += OnEnd;
                }

                protected virtual void InitIdle(bool enabled)
                {
                    if(enabled)
                        idle.StateUpdate.Complex += OnIdle;
                    else
                        idle.StateUpdate.Complex -= OnIdle;
                }

                protected virtual void OnStart()
                {
                    InitIdle(true);
                }

                protected virtual void OnIdle(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
                {

                }

                protected virtual void OnShot()
                {

                }

                protected virtual void OnEnd()
                {
                    InitIdle(false);
                }
            }

            public override void Init(RangedSharpShooterWeapon weapon)
            {
                base.Init(weapon);

                stateMachines.Init(weapon);
                weapon.Modules.Add(stateMachines);

                Weapon.sprint.OnStart += OnSprint;
            }
            public override void Disable()
            {
                base.Disable();

                isOn = false;
            }

            public virtual void Process(bool input)
            {
                if (Weapon.CanAim && input)
                    Toggle();
            }

            public event Action<bool> OnToggle;
            public virtual void Toggle()
            {
                isOn = !isOn;

                Weapon.anim.SetBool("Aim", isOn);

                Weapon.Controller.Sway.RangeScale = isOn ? swayScale : 1f;

                if (OnToggle != null)
                    OnToggle(isOn);
            }

            protected virtual void OnSprint()
            {
                if (isOn)
                    Toggle();
            }
        }
        public bool CanAim { get { return CanProcess; } }

        protected override void AddModules()
        {
            base.AddModules();

            Modules.Add(aim);
        }
    }

    public abstract partial class RangedSharpShooterWeapon : BaseRangedSharpShooterWeapon
    {
        [Serializable]
        public partial class AimModule : BaseAimModule
        {
            [Serializable]
            public partial class StateMachinesModule : BaseStateMachinesModule
            {

            }
        }
    }
}