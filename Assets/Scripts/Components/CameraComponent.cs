using UnityEngine;

namespace Client 
{
    struct CameraComponent 
    {
        public Transform cameraTransform;
        public Vector3 curVelocity;
        public Vector3 offset;
        public float cameraSmoothness;
    }
}