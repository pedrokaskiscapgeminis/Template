using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Player
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector2 movementInput;
        public Vector3 aimForwardVector;
        public Vector3 aimRightVector;
        public NetworkBool isJumpPressed;
        public NetworkBool isRunning;
        public float cursorSpeedX;
        public float cursorSpeedY;
    }
}
