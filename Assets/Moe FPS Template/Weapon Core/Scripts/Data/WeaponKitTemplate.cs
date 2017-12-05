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

namespace WeaponCore
{
    [Serializable]
	public abstract class BaseWeaponKitTemplate<TWeapon>
	{
        public const int Count = 3;
        public const int Size = Count - 1;

		[SerializeField]
        protected TWeapon primary;
        public TWeapon Primary { get { return primary; } set { primary = value; } }

        [SerializeField]
        protected TWeapon secondary;
        public TWeapon Secondary { get { return secondary; } set { secondary = value; } }

        [SerializeField]
        protected TWeapon melee;
        public TWeapon Melee { get { return melee; } set { melee = value; } }

        public virtual TWeapon this[KitSlot slot]
        {
            get
            {
                switch (slot)
                {
                    case KitSlot.Primary:
                        return primary;

                    case KitSlot.Secondary:
                        return secondary;

                    case KitSlot.Melee:
                        return melee;
                }

                throw new NotImplementedException();
            }
            set
            {
                switch (slot)
                {
                    case KitSlot.Primary:
                        Primary = value;
                        return;

                    case KitSlot.Secondary:
                        Secondary = value;
                        return;

                    case KitSlot.Melee:
                        Melee = value;
                        return;
                }

                throw new NotImplementedException();
            }
        }
        public virtual TWeapon this[int index]
        {
            get
            {
                return this[IndexToSlot(index)];
            }
            set
            {
                this[IndexToSlot(index)] = value;
            }
        }

        public static int SlotToIndex(KitSlot slot)
        {
            return (int)slot;
        }
        public static KitSlot IndexToSlot(int index)
        {
            return (KitSlot)index;
        }
    }

    [Serializable]
    public abstract partial class WeaponKitTemplate<TWeapon> : BaseWeaponKitTemplate<TWeapon>
    {

    }

    public enum KitSlot
    {
        Primary, Secondary, Melee
    }
}