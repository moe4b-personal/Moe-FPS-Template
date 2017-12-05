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
    public abstract partial class BaseWeapon : TypesData.TWeapon
    {
        public const string MenuPath = Constants.CreateAssetMenuPath + "Weapons/";

        private Weapon This { get { return this as Weapon; } }

        public WeaponController Controller { get; protected set; }
        public IWeaponCorePlayer Player { get { return Controller.Player; } }

        public Weapon.ModulesManager Modules { get; protected set; }
        public abstract class BaseModulesManager : MoeModuleManager<Weapon.Module>
        {
            public Weapon Weapon { get; protected set; }

            public virtual void Init()
            {
                ForAll(InitModule);
            }
            protected virtual void InitModule(Weapon.Module module)
            {
                module.Init(Weapon);
            }

            public virtual void Activate()
            {
                ForAll(ActivateModule);
            }
            protected virtual void ActivateModule(Weapon.Module module)
            {
                module.Activate();
            }

            public virtual void Disable()
            {
                ForAll(DisableModule);
            }
            protected virtual void DisableModule(Weapon.Module module)
            {
                module.Disable();
            }

            public BaseModulesManager(Weapon weapon) : base()
            {
                this.Weapon = weapon;
            }
        }

        public abstract WeaponType Type { get; }
        [SerializeField]
        protected Weapon.TypeCastModule typeCast;
        public Weapon.TypeCastModule TypeCast { get { return typeCast; } }
        [Serializable]
        public abstract class BaseTypeCastModule : Weapon.Module<Weapon>
        {
            public Weapon.TypeCastModule This { get { return this as Weapon.TypeCastModule; } }

            public TypeController<RangedWeapon> Ranged { get; protected set; }
            public TypeController<RangedSharpShooterWeapon> RangedSharpShooter { get; protected set; }
            public TypeController<DepletableWeapon> Depletable { get; protected set; }
            public TypeController<CasterWeapon> Caster { get; protected set; }
            public TypeController<ProjectileWeapon> Projectile { get; protected set; }
            public TypeController<Gun> Gun { get; protected set; }

            public override void Init(Weapon weapon)
            {
                base.Init(weapon);

                InitTypes();
            }

            protected virtual void InitTypes()
            {
                Ranged = new TypeController<RangedWeapon>(This);
                RangedSharpShooter = new TypeController<RangedSharpShooterWeapon>(This);
                Depletable = new TypeController<DepletableWeapon>(This);
                Caster = new TypeController<CasterWeapon>(This);
                Projectile = new TypeController<ProjectileWeapon>(This);
                Gun = new TypeController<Gun>(This);
            }

            [Serializable]
            public class TypeController<TValue>
                where TValue : Weapon
            {
                public Weapon.TypeCastModule TypeCast { get; protected set; }
                public Weapon Weapon { get { return TypeCast.Weapon; } }

                [SerializeField]
                protected TValue value;
                public TValue Value
                {
                    get
                    {
                        if (Weapon is TValue)
                        {
                            if (value == null)
                                value = Weapon as TValue;

                            return value;
                        }

                        return null;
                    }
                }

                public TypeController(Weapon.TypeCastModule typeCast)
                {
                    this.TypeCast = typeCast;
                }
            }
        }

        [SerializeField]
        protected WeaponData data;
        public WeaponData Data { get { return data; } }

        [SerializeField]
        protected DualInt damage = new DualInt(30, 40);
        public DualInt Damage { get { return damage; } }
        public int RandomDamage { get { return damage.GetRandom(); } }

        protected bool _canProcess = true;
        public virtual bool CanProcess { get { return _canProcess; } set { _canProcess = value; } }

        [SerializeField]
        protected Weapon.UseModule use;
        public Weapon.UseModule Use { get { return use; } }
        [Serializable]
        public abstract class BaseUseModule : Weapon.TimeDelayModule<Weapon>
        {
            public virtual void Process(bool used)
            {
                Weapon.InvokeOnUse(used);

                if (Weapon.CanUse && used)
                    Coroutine.Start();
            }

            protected override void StartAction()
            {
                base.StartAction();

                Weapon.CanProcess = false;

                Weapon.anim.SetTrigger("Use");
            }

            protected override void EndAction()
            {
                base.EndAction();

                Weapon.CanProcess = true;
            }
        }
        public virtual bool CanUse { get { return CanProcess; } }
        public event Action<bool> OnUse;
        protected virtual void InvokeOnUse(bool used)
        {
            if (OnUse != null)
                OnUse(used);
        }

        [SerializeField]
        protected Weapon.SprintModule sprint;
        public Weapon.SprintModule Sprint { get { return sprint; } }
        [Serializable]
        public abstract class BaseSprintModule : Weapon.Module<Weapon>
        {
            protected bool isOn = false;
            public bool IsOn { get { return isOn; } }

            public override void Disable()
            {
                base.Disable();

                isOn = false;
            }

            public virtual void Process(bool input)
            {
                if (input && !Weapon.CanProcess)
                    return;

                if (!isOn && input)
                    StartAction();
                if (isOn && !input)
                    EndAction();

                isOn = input;

                Weapon.anim.SetBool("Sprinting", IsOn);
            }

            public event Action OnStart;
            protected virtual void StartAction()
            {
                Weapon.CanProcess = false;

                if (OnStart != null)
                    OnStart();
            }

            public event Action OnEnd;
            protected virtual void EndAction()
            {
                Weapon.CanProcess = true;

                if (OnEnd != null)
                    OnEnd();
            }
        }

        public AudioSource Aud { get { return Controller.Aud; } }
        public void PlayOneShot(AudioClip clip)
        {
            Aud.PlayOneShot(clip);
        }
        public virtual AudioClip PlayOneShot(SoundSet set)
        {
            AudioClip clip = set.RandomClip;

            PlayOneShot(clip);

            return clip;
        }

        [SerializeField]
        protected Animator anim;
        public Animator Anim { get { return anim; } }

        protected virtual void Reset()
        {

        }

        public virtual void Init(WeaponController controller)
        {
            this.Controller = controller;

            InitModules();
        }
        protected virtual void InitModules()
        {
            Modules = new Weapon.ModulesManager(This);

            AddModules();

            Modules.Init();
        }
        protected virtual void AddModules()
        {
            Modules.Add(typeCast);
            Modules.Add(use);
            Modules.Add(sprint);
        }

        public virtual void Equip()
        {
            Activate();
        }
        public virtual void Activate()
        {
            gameObject.SetActive(true);

            Modules.Activate();
        }

        public virtual bool CanUnEquip
        {
            get
            {
                return true;
            }
        }
        public virtual void UnEquip()
        {
            Disable();
        }
        public virtual void Disable()
        {
            gameObject.SetActive(false);

            Modules.Disable();

            CanProcess = true;
        }

        public virtual void Drop()
        {

        }

        public delegate void DoDamageDelegate(GameObject damaged, int damage);
        public event DoDamageDelegate OnDoDamage;
        public virtual void InvokeOnDoDamage(GameObject damaged, int damage)
        {
            if (OnDoDamage != null)
                OnDoDamage(damaged, damage);
        }
        public virtual void DoDamage(GameObject damaged)
        {
            DoDamage(damaged, RandomDamage);
        }
        public virtual void DoDamage(GameObject damaged, int damage)
        {
            DoDamage(damaged, damage, This);
        }

        public static void DoDamage(GameObject damaged, Weapon weapon)
        {  
            DoDamage(damaged, weapon.RandomDamage, weapon);
        }
        public static void DoDamage(GameObject damaged, int damage, Weapon weapon)
        {
            DoDamageInternal(damaged, damage);

            if (weapon)
                weapon.InvokeOnDoDamage(damaged, damage);
        }

        static partial void DoDamageInternal(GameObject damaged, int damage);

        [Serializable]
        public abstract class BaseModule : MoeModule
        {
            public abstract Weapon BaseWeapon { get; }

            public abstract void Init(Weapon weapon);

            public virtual void Activate()
            {

            }

            public virtual void Disable()
            {

            }
        }
        [Serializable]
        public abstract class BaseModule<TWeapon> : Weapon.Module
            where TWeapon : Weapon
        {
            public TWeapon Weapon { get; protected set; }
            public override Weapon BaseWeapon { get { return Weapon; } }

            public virtual void Init(TWeapon weapon)
            {
                this.Weapon = weapon;
            }
        }

        [Serializable]
        public abstract class BaseTimeModule<TWeapon> : Weapon.Module<TWeapon>
            where TWeapon : Weapon
        {
            public AutoCoroutine Coroutine { get; protected set; }
            public bool Running { get { return Coroutine.Running; } }

            public override void Init(TWeapon weapon)
            {
                base.Init(weapon);

                Coroutine = new AutoCoroutine(weapon, Procedure);
            }

            public override void Disable()
            {
                base.Disable();

                Coroutine.End();
            }

            protected abstract IEnumerator Procedure();
        }
        
        [Serializable]
        public abstract class BaseTimeDelayModule<TWeapon> : Weapon.TimeModule<TWeapon>
            where TWeapon : Weapon
        {
            [SerializeField]
            protected float delay;
            public float Delay { get { return delay; } }

            public event Action OnStart;
            protected virtual void StartAction()
            {
                if (OnStart != null)
                    OnStart();
            }

            protected override IEnumerator Procedure()
            {
                StartAction();

                yield return new WaitForSeconds(delay);

                Coroutine.End();
                EndAction();
            }

            public event Action OnEnd;
            protected virtual void EndAction()
            {
                if (OnEnd != null)
                    OnEnd();
            }
        }
    }

    public abstract partial class Weapon : BaseWeapon
    {
        public partial class ModulesManager : BaseModulesManager
        {
            public ModulesManager(Weapon weapon) : base(weapon)
            {

            }
        }

        [Serializable]
        public abstract partial class Module : BaseModule
        {

        }

        [Serializable]
        public abstract partial class Module<TWeapon> : BaseModule<TWeapon>
            where TWeapon : Weapon
        {
            public override void Init(Weapon weapon)
            {
                if (weapon is TWeapon)
                    Init(weapon as TWeapon);
                else
                    throw new InvalidCastException("Cannot Cast " + weapon.GetType().Name + " To a " + typeof(TWeapon).Name);
            }
        }

        [Serializable]
        public abstract partial class TimeModule<TWeapon> : BaseTimeModule<TWeapon>
            where TWeapon : Weapon
        {

        }

        [Serializable]
        public abstract partial class TimeDelayModule<TWeapon> : BaseTimeDelayModule<TWeapon>
            where TWeapon : Weapon
        {

        }


        [Serializable]
        public partial class UseModule : BaseUseModule
        {

        }

        [Serializable]
        public partial class TypeCastModule : BaseTypeCastModule
        {
            
        }

        [Serializable]
        public partial class SprintModule : BaseSprintModule
        {

        }
    }

    [Flags]
    public enum WeaponType
    {
        Ranged = 1 << 1,
        RangedSharpShooter = 1 << 1,
        Depletable = 1 << 2,
        Caster = 1 << 3,
        Projectile = 1 << 4,
        Gun = 1 << 5,
        Melee = 1 << 6
    }
}