using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Complete
{
    [System.Serializable]
    public class PerfectOverride
    {
        //the orthographic size value represents the amount of in-game units (meters) half of your vertical screen size takes.         
        public int referenceOrthographicSize;
        public float referencePixelsPerUnit;

        
    }

    [ExecuteInEditMode]
    public class PerfectPixel : MonoBehaviour    //main goal: calculates the orthographic size for the camera based on the screen height.
    {
        [Tooltip("the reference resolution to which your game is made for (e.g. 768px)")]
        public int referenceResolution; //vertical resolution
        [Tooltip("Reference main PPU (e.g. 32, 64 etc...")]
        public float referencePixelsPerUnit;

        public List<PerfectOverride> overrides;

        private int lastSize = 0;

        public float scale = 2;

        // Use this for initialization
        void Start()
        {
            UpdateOrthoSize();
        }

        PerfectOverride FindOverride(int size)
        {
            //param: PerfectOverride x
            //returns the first element of the sequence that satisfies a condition or a default value if no such element is foundF
            return overrides.FirstOrDefault(x => x.referenceOrthographicSize == size);
        }

        void UpdateOrthoSize()
        {
            lastSize = Screen.height;

            // first find the reference orthoSize
            // It’s 0.5f because orthographic size specifies the size going from the center of the screen to the top.
            float refOrthoSize = (referenceResolution / referencePixelsPerUnit) * 0.5f;

            // then find the current orthoSize
            var overRide = FindOverride(lastSize);

            float ppu = overRide != null ? overRide.referencePixelsPerUnit : referencePixelsPerUnit;

            float orthoSize = (lastSize / ppu) * 0.5f;//calc~actual~ortho~size(curr~one)

             // the multiplier is to make sure the orthoSize is as close to the reference as possible
            float multiplier = Mathf.Max(1, Mathf.Round(orthoSize / refOrthoSize));

            //If your screen grows big enough, we could display your sprites at 2x by applying a scaling factor to the calculation
            //    of the orthographic size.For the case of 1440, we could keep using the PPU 32 sprites but calculate the orthographic
            //    size as such (1440 / (2 * 32)) * 0.5 which gives you 11.25.

            // This means, each world space units will contain 64 screen pixels. This effectively tells the engine to render
            //    32 pixels from the sprite onto 64 pixels on screen.This gives a nice whole number factor of 2 which will 
             //   look good at the same time giving you a world space that’s just 6.25 % smaller than your reference setup.

            // then we rescale the orthoSize by the multipler
            orthoSize /= multiplier;

            // set it
            this.GetComponent<Camera>().orthographicSize = orthoSize/scale;
        }

        // Update is called once per frame
        void Update()
        {
            //if in Unity editor: update orhoSize on everystep if different :  Otherwise this is done only on start          
            #if UNITY_EDITOR
            if (lastSize != Screen.height)
                UpdateOrthoSize();
            #endif
        }
    }
}
