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
    [Serializable]
    public abstract class BaseWeaponController : MonoBehaviour
    {
        public WeaponController This { get { return this as WeaponController; } }

        public IWeaponCorePlayer Player { get; protected set; }

        [SerializeField]
        protected WeaponKitData startKit;
        public WeaponKitData StartKit { get { return startKit; } set { startKit = value; } }

        [SerializeField]
        protected WeaponController.KitData kit;
        public WeaponController.KitData Kit { get { return kit; } }
        [Serializable]
        public abstract class BaseKitData : BaseWeaponKitTemplate<Weapon>
        {

        }

        [SerializeField]
        protected KitSlot slot = KitSlot.Primary;
        public KitSlot Slot
        {
            get
            {
                return slot;
            }
            set
            {
                Equip(value);
            }
        }

        [SerializeField]
        protected WeaponController.SwayModule sway;
        public WeaponController.SwayModule Sway { get { return sway; } }

        [SerializeField]
        protected AudioSource aud;
        public AudioSource Aud { get { return aud; } }

        public Weapon CurrentWeapon { get { return kit[slot]; } }
        public DepletableWeapon CurrentDepletable
        {
            get
            {
                if (CurrentWeapon)
                    return CurrentWeapon.TypeCast.Depletable.Value;

                return null;
            }
        }
        public RangedSharpShooterWeapon CurrentRangedSharpShooter
        {
            get
            {
                if (CurrentWeapon)
                    return CurrentWeapon.TypeCast.RangedSharpShooter.Value;

                return null;
            }
        }

        public virtual void Init(IWeaponCorePlayer player)
        {
            this.Player = player;

            sway.SetLink(This);
            sway.Init();

            if (startKit != null)
                SetKit(startKit, slot);
        }

        #region Update Input
        public virtual void UpdateCurrent(WeaponUpdateData data)
        {
            UpdateSprint(data.Sprint);

            UpdateUse(data.Use);
            UpdateAim(data.Aim);

            UpdateSwitch(data.Switch);

            UpdateReload(data.Reload);

            sway.Process(data.Sway);
        }

        protected virtual void UpdateUse(bool input)
        {
            if (CurrentWeapon)
                UpdateUseInternal(input);
        }
        protected virtual void UpdateUseInternal(bool input)
        {
            CurrentWeapon.Use.Process(input);
        }

        protected virtual void UpdateReload(bool input)
        {
            if (CurrentDepletable)
                UpdateReloadInternal(input);
        }
        protected virtual void UpdateReloadInternal(bool input)
        {
            CurrentDepletable.Reload.Process(input);
        }

        protected virtual void UpdateAim(bool input)
        {
            if (CurrentRangedSharpShooter)
                UpdateAimInternal(input);
        }
        protected virtual void UpdateAimInternal(bool input)
        {
            CurrentRangedSharpShooter.Aim.Process(input);
        }

        protected virtual void UpdateSwitch(int input)
        {
            if(input == 0)
            {

            }
            else
            {
                KitSlot newSlot = slot;

                if (input > 0)
                    slot = GetNextValidSlot();
                else
                    slot = GetPreviousValidSlot();

                if(newSlot != slot)
                {
                    Equip(newSlot);
                }
            }
        }
        public virtual KitSlot GetNextValidSlot()
        {
            KitSlot newSlot = slot;

            do
            {
                newSlot = slot.GetNextValue();
            }
            while (kit[newSlot] == null);

            return newSlot;
        }
        public virtual KitSlot GetPreviousValidSlot()
        {
            KitSlot newSlot = slot;

            do
            {
                newSlot = slot.GetNextValue();
            }
            while (kit[newSlot] == null);

            return newSlot;
        }

        protected virtual void UpdateSprint(bool input)
        {
            if (CurrentWeapon)
                UpdateSprintInternal(input);
        }
        protected virtual void UpdateSprintInternal(bool input)
        {
            CurrentWeapon.Sprint.Process(input);
        }
        #endregion

        public virtual void SetKit(WeaponKitData data)
        {
            SetKit(data, KitSlot.Primary);
        }
        public virtual void SetKit(WeaponKitData data, KitSlot slot)
        {
            for (int i = 0; i < WeaponKit.Count; i++)
            {
                if (kit[i] != null)
                    Drop(WeaponKit.IndexToSlot(i));
            }

            for (int i = 0; i < WeaponKit.Count; i++)
            {
                kit[i] = SpawnWeapon(data.Kit[i]);

                kit[i].Init(This);

                if (i == WeaponKit.SlotToIndex(slot))
                    EquipInternal(slot);
                else
                    kit[i].Disable();
            }
        }

        public virtual void Swap(Weapon weapon)
        {
            Swap(slot, weapon);
        }
        public virtual void Swap(KitSlot newSlot, Weapon weapon)
        {
            Drop(newSlot);

            kit[newSlot] = weapon;
        }

        public virtual void Swap(WeaponData weapon)
        {
            Swap(slot, weapon);
        }
        public virtual void Swap(KitSlot newSlot, WeaponData weapon)
        {
            Swap(newSlot, SpawnWeapon(weapon));
        }

        protected virtual Weapon SpawnWeapon(WeaponData data)
        {
            if(data.Prefab == null)
                throw new ArgumentException("Cannot Spawn Weapon From " + data.name + " Data, Because The Prefab Has Not Been Set");
            if (data.Prefab.GetComponent<Weapon>() == null)
                throw new ArgumentException("Cannot Spawn Weapon From " + data.name + " Data, Because The Prefab Has No Type of Weapon Component");

            var instance = Instantiate(data.Prefab, transform, false);

            instance.gameObject.name = data.name;

            return instance.GetComponent<Weapon>();
        }

        public virtual void Equip(KitSlot newSlot)
        {
            if (newSlot == slot)
            {

            }
            else
            {
                if (kit[newSlot] == null)
                {
                    Debug.LogWarning("Trying To Equip " + newSlot + " But No Weapon Is Located In That Slot");
                }
                else
                {
                    if(CurrentWeapon == null || CurrentWeapon.CanUnEquip)
                    {
                        UnEquip();

                        EquipInternal(newSlot);
                    }
                    else
                    {

                    }
                }
            }
        }
        protected virtual void EquipInternal(KitSlot newSlot)
        {
            kit[newSlot].Equip();

            slot = newSlot;
        }
        protected virtual void UnEquip()
        {
            if (CurrentWeapon)
                CurrentWeapon.UnEquip();
        }

        protected virtual void Drop(KitSlot slot)
        {
            kit[slot].Drop();
        }

        [Serializable]
        public abstract class BaseModule : MoeLinkedModule<WeaponController>
        {
            public virtual void Init()
            {

            }
        }

        [Serializable]
        public abstract class BaseSwayModule : WeaponController.Module
        {
            [SerializeField]
            protected Vector2 input;
            public Vector2 Input { get { return input; } }

            [SerializeField]
            protected Vector2 range = new Vector2(0.2f, 0.3f);
            public Vector2 Range { get { return range; } }
            protected float rangeScale = 1;
            public float RangeScale
            {
                get
                {
                    return rangeScale;
                }
                set
                {
                    value = Mathf.Clamp01(value);

                    rangeScale = value;
                }
            }

            [SerializeField]
            protected float delta = 0.3f;
            public float Delta { get { return delta; } }

            [SerializeField]
            protected float gravity = 0.12f;
            public float Gravity { get { return gravity; } }

            protected Vector3 position = Vector3.zero;
            public Vector3 Position { get { return position; } }

            public virtual void Process(Vector2 value)
            {
                input = value;

                position.x = ProcessAxis(-input.x, position.x, range.x);
                position.y = ProcessAxis(-input.y, position.y, range.y);

                Link.transform.localPosition = position;
            }

            public virtual float ProcessAxis(float axisValue, float value, float range)
            {
                value = Mathf.Lerp(value, range * rangeScale * Mathf.Sign(axisValue), delta * Time.deltaTime * Math.Abs(axisValue));
                value = Mathf.Lerp(value, 0, gravity * Time.deltaTime);

                return value;
            }
        }
    }

    [Serializable]
    public partial class WeaponController : BaseWeaponController
    {
        [Serializable]
        public partial class KitData : BaseKitData
        {

        }

        [Serializable]
        public abstract partial class Module : BaseModule
        {

        }

        [Serializable]
        public partial class SwayModule : BaseSwayModule
        {

        }
    }


    public interface IBaseWeaponCorePlayer
    {
        GameObject gameObject { get; }
    }

    public partial interface IWeaponCorePlayer : IBaseWeaponCorePlayer
    {

    }

    
    [Serializable]
    public partial struct WeaponUpdateData
    {
        public bool Use;
        public bool Aim;
        public bool Reload;
        public int Switch;
        public bool Sprint;

        public Vector2 Sway;
    }


    [Serializable]
    public abstract class BaseWeaponNumberSwitcher
    {
        public WeaponController Controller { get; protected set; }

        public readonly KeyCode[] Keys = new KeyCode[]
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
        };
        public int MaxCount { get { return Keys.Length; } }

        public BaseWeaponNumberSwitcher(WeaponController controller)
        {
            this.Controller = controller;
        }

        public virtual void Update()
        {
            int count = WeaponKit.Count;

            if (count > MaxCount)
                count = MaxCount;

            KitSlot slot;
            for (int i = 0; i < count; i++)
            {
                if (Input.GetKeyDown(Keys[i]))
                {
                    slot = WeaponKit.IndexToSlot(i);

                    Controller.Equip(slot);
                }
            }
        }
    }

    [Serializable]
    public partial class WeaponNumberSwitcher : BaseWeaponNumberSwitcher
    {
        public WeaponNumberSwitcher(WeaponController controller) : base(controller)
        {

        }
    }
}