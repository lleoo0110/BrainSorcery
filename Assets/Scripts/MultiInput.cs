using HolmonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInput : InputBase
{
    [SerializeField] private InputBase[] inputs;

    private void Start()
    {
        foreach(var input in inputs)
        {
            input.AssignInputOnCallback(() =>
            {
                this.ExecuteInputOnCallback();
            });
            input.AssignInputOffCallback(() =>
            {
                this.ExecuteInputOffCallback();
            });
        }
    }
}
