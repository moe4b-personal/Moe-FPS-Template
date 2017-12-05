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
    public abstract class BaseWeaponData : ScriptableObject
    {
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        protected Weapon _weapon;
        public Weapon Weapon
        {
            get
            {
                if (_weapon == null || _weapon.gameObject != prefab)
                    _weapon = prefab.GetComponent<Weapon>();

                return _weapon;
            }
        }

        [SerializeField]
        [TextArea]
        protected string description;
        public string Description { get { return description; } }

        [SerializeField]
        protected Sprite sprite;
        public Sprite Sprite { get { return sprite; } }
    }

    [CreateAssetMenu(menuName = Weapon.MenuPath + "Data")]
	public partial class WeaponData : BaseWeaponData
    {
        
	}
}