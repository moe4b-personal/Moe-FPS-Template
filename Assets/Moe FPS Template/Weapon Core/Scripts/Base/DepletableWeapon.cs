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
	public abstract partial class BaseDepletableWeapon : RangedSharpShooterWeapon
	{
        private DepletableWeapon This { get { return this as DepletableWeapon; } }

        public override WeaponType Type { get { return base.Type | WeaponType.Depletable; } }

        [SerializeField]
        protected DepletableWeapon.AmmoModule ammo;
        public DepletableWeapon.AmmoModule Ammo { get { return ammo; } }
        [Serializable]
        public class BaseAmmoModule : Module<DepletableWeapon>
        {
            [SerializeField]
            protected MaxIntValue clip;
            public MaxIntValue Clip { get { return clip; } }

            [SerializeField]
            protected MaxIntValue reserve;
            public MaxIntValue Reserve { get { return reserve; } }

            public virtual bool CanReload
            {
                get
                {
                    return clip.Full == false && reserve.Value != 0;
                }
            }
            public virtual bool ClipEmpty
            {
                get
                {
                    return clip.Value == 0;
                }
            }

            public bool ProgressClip()
            {
                if (clip.Value == 0)
                    return false;

                clip.Value--;
                return true;
            }
            public bool ReserveToClip()
            {
                if (reserve.Value == 0)
                    return false;

                int requiredAmmo = clip.Max - clip.Value;

                if (reserve.Value >= requiredAmmo)
                {
                    reserve.Value -= requiredAmmo;
                    clip.Value += requiredAmmo;
                }
                else
                {
                    clip.Value += reserve.Value;
                    reserve.Value = 0;
                }

                return true;
            }

            public virtual void Fill()
            {
                reserve.Fill();
            }

            public BaseAmmoModule() : this(30, 120)
            {

            }
            public BaseAmmoModule(int clip, int reserve)
            {
                this.clip = new MaxIntValue(clip, clip);
                this.reserve = new MaxIntValue(reserve, reserve);
            }
        }
        protected virtual void UpdateAmmoUI()
        {

        }

        public override bool CanUse
        {
            get
            {
                return base.CanUse && !shoot.ModeLock && !ammo.ClipEmpty;
            }
        }

        [SerializeField]
        protected DepletableWeapon.ShootModule shoot;
        public DepletableWeapon.ShootModule Shoot { get { return shoot; } }
        [Serializable]
        public abstract class BaseShootModule : Module<DepletableWeapon>
        {
            [SerializeField]
            protected FiringMode mode = FiringMode.Automatic;
            public FiringMode Mode { get { return mode; } }
            public bool ModeLock { get; protected set; }

            [SerializeField]
            protected SoundSet sounds;
            public SoundSet Sounds { get { return sounds; } }

            public override void Init(DepletableWeapon weapon)
            {
                base.Init(weapon);

                weapon.OnUse += OnUse;
                weapon.Use.OnStart += Process;
            }

            protected virtual void OnUse(bool used)
            {
                if (!used && ModeLock)
                    ModeLock = false;
            }
            protected virtual void Process()
            {
                Weapon.PlayOneShot(sounds);
                Weapon.ammo.ProgressClip();
                Weapon.UpdateAmmoUI();

                if (mode == FiringMode.Semi)
                    ModeLock = true;

                Weapon.InvokeOnShoot();

            }
        }
        public event Action OnShoot;
        protected virtual void InvokeOnShoot()
        {
            if (OnShoot != null)
                OnShoot();
        }

        [SerializeField]
        protected DepletableWeapon.ReloadModule reload;
        public DepletableWeapon.ReloadModule Reload { get { return reload; } }
        [Serializable]
        public abstract class BaseReloadModule : Module<DepletableWeapon>
        {
            protected StateMachineCallbackRewind callback;
            public StateMachineCallbackRewind ReloadCallback { get { return callback; } }

            [SerializeField]
            protected DepletableWeapon.AutoReloadMode autoReload = DepletableWeapon.AutoReloadMode.OnEmptyMag;
            public DepletableWeapon.AutoReloadMode AutoReload { get { return autoReload; } }

            protected bool processing = false;
            public bool Processing
            {
                get
                {
                    return processing;
                }
                protected set
                {
                    processing = value;

                    Weapon.CanProcess = !value;
                }
            }

            public override void Init(DepletableWeapon weapon)
            {
                base.Init(weapon);

                weapon.OnUse += ProcessAutoReload;
            }

            public override void Activate()
            {
                base.Activate();

                callback = Weapon.anim.GetBehaviour<StateMachineCallbackRewind>("Reload");
                callback.StateExit.Simple += EndAction;
            }

            public override void Disable()
            {
                base.Disable();

                Processing = false;
            }

            public virtual void Process(bool reload)
            {
                if (Weapon.CanReload && reload)
                    Do();
            }
            protected virtual void ProcessAutoReload(bool used)
            {
                if (Weapon.CanReload && Weapon.ammo.Clip.Empty)
                {
                    if (autoReload == DepletableWeapon.AutoReloadMode.OnEmptyMag || (autoReload == DepletableWeapon.AutoReloadMode.OnEmptyFire && used))
                        Do();
                }
            }

            protected virtual void Do()
            {
                Processing = true;

                Weapon.UpdateAmmoUI();

                if (Weapon.aim.IsOn)
                    Weapon.aim.Toggle();

                Weapon.anim.SetTrigger("Reload");
            }

            protected virtual void EndAction()
            {
                Processing = false;

                Weapon.ammo.ReserveToClip();

                Weapon.UpdateAmmoUI();
            }
        }
        public virtual bool CanReload
        {
            get
            {
                return CanProcess && ammo.CanReload && !reload.Processing;
            }
        }

        protected override void AddModules()
        {
            base.AddModules();

            Modules.Add(ammo);
            Modules.Add(shoot);
            Modules.Add(reload);
        }
    }

    public abstract partial class DepletableWeapon : BaseDepletableWeapon
    {
        [Serializable]
        public partial class AmmoModule : BaseAmmoModule
        {
            public AmmoModule() : base()
            {

            }
            public AmmoModule(int clip, int reserve) : base(clip, reserve)
            {

            }
        }

        [Serializable]
        public partial class ShootModule : BaseShootModule
        {

        }

        [Serializable]
        public partial class ReloadModule : BaseReloadModule
        {

        }

        public enum AutoReloadMode
        {
            None, OnEmptyMag, OnEmptyFire
        }
    }

    public enum FiringMode
    {
        Semi, Automatic
    }
}