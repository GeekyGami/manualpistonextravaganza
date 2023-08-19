using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using UnityEngine;
using UnityEngine.Video;
using BepInEx.Configuration;


[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
public class ManualPistonExtravaganza : BaseUnityPlugin
{
        public const string pluginGuid = "ManualPistonExtravaganza";
        public const string pluginName = "Manual Piston Extravaganza";
        public const string pluginVersion = "1.1.0.0";
        //public ConfigEntry<KeyboardShortcut> Hotkey_Dump { get; private set; }
        private float speed;
        private Vector3 startPosition;
        public bool isOn = false;

        private List<Animator> animators = new List<Animator>();
        private List<VideoPlayer> videoPlayers = new List<VideoPlayer>();

        private void SetSpeeds(float theSpeed)
        {
            var hasAnyNulls = false;
            for (var i = 0; i < animators.Count; i++)
            {
                var animator = animators[i];
                if (animator == null)
                {
                    hasAnyNulls = true;
                    continue;
                }
                animator.speed = theSpeed;
            }
            if (hasAnyNulls)
            {
                animators.RemoveAll(animators => animators == null);
                hasAnyNulls = false;
            }

            for (var i = 0; i < videoPlayers.Count; i++)
            {
                var videoPlayer = videoPlayers[i];
                if (videoPlayer == null)
                {
                    hasAnyNulls = true;
                    continue;
                }
                videoPlayer.playbackSpeed = theSpeed;
            }
            if (hasAnyNulls)
            {
                videoPlayers.RemoveAll(videoPlayers => videoPlayers == null);
            }
        }

    private void AnimVidAdder()
    {
        if (isOn)
        {
            animators.AddRange(FindObjectsOfType<Animator>());
            videoPlayers.AddRange(FindObjectsOfType<VideoPlayer>());
        }
    }
    private void Start()
    { InvokeRepeating("AnimVidAdder", 1f, 0.1f); }

        private void Update()
        {
            var delta = Input.mousePosition - startPosition;
            var mag = delta.sqrMagnitude;
            if (mag > 0.01f)
            {
            speed = Mathf.Clamp((float)Math.Sqrt(mag) * 0.1f, 0, 11f);
            }
            else
            {
                speed = 0f;
            }
            startPosition = Input.mousePosition;
            if (Input.GetKeyDown(KeyCode.KeypadDivide))
            {
                isOn = !isOn;
            }
            if (isOn)
            {
                SetSpeeds(speed);
            }
            else
            {       
            SetSpeeds(1f);
            videoPlayers.Clear();
            animators.Clear();
        }
        }
    }




