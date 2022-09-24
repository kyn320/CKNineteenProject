using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landmark
{
    public enum Landmark_State
    {
        NONE = 0,
        LANDMARK_WAIT,
        LANDMARK_READY,
        LANDMARK_WORK,
        LANDMARK_DESTROY
    }
}
