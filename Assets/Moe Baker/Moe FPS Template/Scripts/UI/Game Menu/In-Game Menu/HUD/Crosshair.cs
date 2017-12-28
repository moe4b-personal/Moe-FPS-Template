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

namespace Moe.FPSTemplate
{
	public class Crosshair : MonoBehaviour
	{
		public bool Visibility { set { gameObject.SetActive(value); } }

        [SerializeField]
        protected PartsData parts;
        public PartsData Parts { get { return parts; } }
        [Serializable]
        public struct PartsData
        {
            [SerializeField]
            Image middle;
            public Image Middle { get { return middle; } }

            [SerializeField]
            RectTransform[] feathers;
            public RectTransform[] Feathers { get { return feathers; } }
        }

        [SerializeField]
        protected RangedSmoothValue distance = new RangedSmoothValue(0f, 40f, 10f);
        public RangedSmoothValue Distance { get { return distance; } }
        public virtual float DistanceValue
        {
            get
            {
                return distance.Value;
            }
            set
            {
                distance.Value = value;
            }
        }

        protected virtual void Update()
        {
            distance.MoveTowardsMin();

            SetDistance(DistanceValue);
        }

        protected virtual void SetDistance(float distance)
        {
            for (int i = 0; i < parts.Feathers.Length; i++)
                SetFeatherDistance(parts.Feathers[i], distance);
        }

        protected virtual void SetFeatherDistance(RectTransform feather, float distance)
        {
            feather.anchoredPosition = (feather.rotation * Vector3.up) * distance;
        }
	}
}