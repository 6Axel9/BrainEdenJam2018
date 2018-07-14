using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeInput : Inputs
{
    public override Vector3 Steering
    {
        get
        {

            return Steerings.Evade(_origin, _target.position);
        }
    }
}
