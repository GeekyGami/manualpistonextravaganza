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
    public const string pluginVersion = "1.0.0.0";
    //public ConfigEntry<KeyboardShortcut> Hotkey_Dump { get; private set; }
    private float speed;
    private Vector3 startPosition;
    public bool isOn = false;

    private List<Animator> animators = new List<Animator>();
    private List<VideoPlayer> videoPlayers = new List<VideoPlayer>();
    private bool donewithspeed = false;

   private void SetSpeeds(float theSpeed)
    {
        for (var i = 0; i < animators.Count; i++)
            animators[i].speed = theSpeed;
        for (var i = 0; i < videoPlayers.Count; i++)
            videoPlayers[i].playbackSpeed = theSpeed;
    }


    private void GetAnimVids()
    { if (this.isOn)
        {
            animators.RemoveAll(animators => animators == null);
            videoPlayers.RemoveAll(videoPlayers => videoPlayers == null);
            if (animators != null) animators.AddRange(FindObjectsOfType<Animator>());
            if (videoPlayers != null) videoPlayers.AddRange(FindObjectsOfType<VideoPlayer>());
        }
    }
        private void Start()
    {
        InvokeRepeating("GetAnimVids", 1f, 0.1f);
        this.Update(); 
    }
    private void Update()
    {
        var x = Input.mousePosition.x - startPosition.x;
        var y = Input.mousePosition.y - startPosition.y;
        if (Mathf.Abs(x) >= 0.5f || Mathf.Abs(y) >= 0.5f)
        {
            speed = (float)Math.Sqrt((double)(Math.Abs(y) * Math.Abs(y) + Math.Abs(x) * Math.Abs(x))) * 0.1f;
            if (speed > 11f)
            {
                speed = 11f;
            }
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
            this.SetSpeeds(this.speed);
        }
        else
        { this.SetSpeeds(1f);
               this.donewithspeed = true;
                  if (this.donewithspeed == true)
                {
                    this.donewithspeed = false;
                        videoPlayers.Clear();
                        animators.Clear();
                }
        }
    }
}




