using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Core
{
    public class СameraFacing : MonoBehaviour
    {
        private Camera _camera;
        private void Awake()
        {
            if(Camera.main != null) _camera = Camera.main;
        }

        void LateUpdate()
        {
            if(_camera!= null && this!= null)
                this.transform.forward = _camera.transform.forward;
        }
    }
}
