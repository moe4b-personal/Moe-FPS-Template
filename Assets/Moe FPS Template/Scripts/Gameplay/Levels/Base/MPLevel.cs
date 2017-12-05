using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using Moe.Tools;

namespace Moe.GameFramework
{
	public partial class MPLevel
	{
        [SerializeField]
        Camera startCamera;
        public Camera StartCamera { get { return startCamera; } }
        public void SetStartCameraActive(bool value)
        {
            startCamera.gameObject.SetActive(value);
        }

        public virtual void Update()
        {
            ProcessQuickSpawn();
        }
        protected virtual void ProcessQuickSpawn()
        {
            if(Debug.isDebugBuild)
            {
                if (PlayerInstance)
                    return;

                KeyCode key;
                for (int i = 0; i < 12; i++)
                {
                    key = GameTools.Enum.Parse<KeyCode>("F" + (i + 1));

                    if (players.Characters.IsInRange(i) && Input.GetKeyDown(key))
                            players.SpawnCharacter(i);
                }
            }
        }

        public partial class PlayersModule
        {
            public MPLevel Level { get { return Link as MPLevel; } }

            protected override void SetLocal(MPPlayer newLocal)
            {
                base.SetLocal(newLocal);

                newLocal.Weapons.SetKit(characters[IndexValue].Kit);

                if (MenuInstance.Respawn.Visibile)
                    MenuInstance.Respawn.Hide();

                Level.SetStartCameraActive(false);
            }

            public partial class CharacterData
            {
                [SerializeField]
                protected WeaponCore.WeaponKitData kit;
                public WeaponCore.WeaponKitData Kit { get { return kit; } }
            }
        }
    }
}