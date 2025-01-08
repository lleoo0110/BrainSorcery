using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolmonUtility;

public class AutomaticRelease : InputBase
{
    [SerializeField] private MagicGenerator _magicGenerator;

    private void Update()
    {
        if(_magicGenerator.effectScaleRat >= 1)
        {
            ExecuteInputOnCallback();
        }
    }
}