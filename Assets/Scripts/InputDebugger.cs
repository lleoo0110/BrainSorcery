using HolmonUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDebugger : MonoBehaviour
{
    [Serializable]
    public class DebugableInputBases
    {
        public InputBase input;
        public int inputNum;
    }

    [SerializeField] private DebugableInputBases[] debugableInputBases;
    [SerializeField] private LogLecorder _logLecorder;


    private void Start()
    {
        foreach(var debugableInputBase in debugableInputBases)
        {
            debugableInputBase.input.AssignInputOnCallback(() =>
            {
                Debug.Log(debugableInputBase.inputNum.ToString() + " is on");

                _logLecorder.AddInput(debugableInputBase.inputNum);
            });
            debugableInputBase.input.AssignInputOffCallback(() =>
            {
                Debug.Log(debugableInputBase.inputNum.ToString() + " is off");
            });
        }
    }
}
