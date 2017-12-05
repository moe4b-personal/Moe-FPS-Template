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
	public class Rocket : ExplosiveProjectile
    {
        [SerializeField]
        protected float minTravel = 4;
        public float MinTravel { get { return minTravel; } }
        public bool PastMinTravel { get { return TraveledDistance >= minTravel; } }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            Explode();
        }
    }
}