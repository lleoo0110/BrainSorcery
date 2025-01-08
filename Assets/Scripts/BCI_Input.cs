using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolmonUtility;

public class BCI_Input : InputBase
{
    [SerializeField] private int _reactionValue = 0;

    public void ExecuteInput(int recValue)
    {
        if(_reactionValue == recValue)
        {
            ExecuteInputOnCallback();
        }
    }
}
