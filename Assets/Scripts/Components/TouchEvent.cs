using UnityEngine;

namespace Client 
{
    public enum TouchPhase
    {
        Began, Moved, Ended, Stationary
    }
    public struct TouchEvent 
    {
        public TouchPhase Phase;
        public Vector3 Direction;   //�����������
        public Vector3 Velocity;    //��������
    }
}