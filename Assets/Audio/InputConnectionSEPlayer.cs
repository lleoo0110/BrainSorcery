using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolmonUtility;
using System;

public class InputConnectionSEPlayer : SEPlayer
{
    [Serializable]
    public class InputBaseConnectableSE
    {
        public List<InputBase> input;
        public string seKey;
    }

    [SerializeField] private List<InputBaseConnectableSE> inputBaseConnectableSEs;

    protected override void Start()
    {
        base.Start();

        foreach (var inputBaseConnectableSE in inputBaseConnectableSEs)
        {
            foreach (var input in inputBaseConnectableSE.input)
            {
                input.AssignInputOnCallback(() =>
                {
                    PlaySE(inputBaseConnectableSE.seKey);
                });
            }
        }
    }
}
