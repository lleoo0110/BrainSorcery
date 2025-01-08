using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolmonUtility;

public class ButtonInput : InputBase
{
    [SerializeField] private KeyCode useKey;

    void Update()
    {
        if (Input.GetKeyDown(useKey)) ExecuteInputOnCallback();
        if(Input.GetKeyUp(useKey)) ExecuteInputOffCallback();
    }
}
