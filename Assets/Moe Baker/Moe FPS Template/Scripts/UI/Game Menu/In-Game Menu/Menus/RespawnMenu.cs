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

using Moe.GameFramework;
using Moe.GameFramework.UI;

namespace Moe.FPSTemplate
{
    public class RespawnMenu : BaseMenuUI
    {
        public KeyCode Key { get { return Game.Control.ActionKeys.Respawn; } }

        [SerializeField]
        protected Text label;
        public Text Label { get { return label; } }

        public bool Interactable
        {
            get
            {
                return Game.Pause.State == GamePauseState.None;
            }
        }

        [SerializeField]
        float autoRespawnTime = 5f;
        public float AutoRespawnTime { get { return autoRespawnTime; } }

        public override void Display()
        {
            base.Display();

            timer = autoRespawnTime;
        }

        float timer;
        protected virtual void Update()
        {
            UpdateLabel(timer);

            timer -= Time.unscaledDeltaTime;

            if ((Interactable && Input.GetKeyDown(Key)) || timer <= 0f)
                Respawn();
        }

        protected virtual void UpdateLabel(float timer)
        {
            label.text = "Automatically Respawning In " + Math.Ceiling(timer) + 's' + '\n' + "Press " + ControlManager.Tools.GetKeyCodeText(Key) + " To Respawn Now";
        }

        protected virtual void Respawn()
        {
            MPLevel.Players.Respawn();

            Hide();
        }
    }
}