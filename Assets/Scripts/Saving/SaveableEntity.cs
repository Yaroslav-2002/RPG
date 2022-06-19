using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RPG.Saving
{
    public class SaveableEntity
    {
        public string GetUniqueIdentifier()
        {
            return String.Empty;
        }

        public object CaptureState()
        {
            // print("Capturing state for :" + GetUniqueIdentifier());
            return null;
        }
    }
}