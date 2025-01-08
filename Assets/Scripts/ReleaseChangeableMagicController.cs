using HolmonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseChangeableMagicController : MagicController
{
    [Header("Ž©“®”­ŽË")]
    [SerializeField] private bool _automaticReleasable;
    [SerializeField] private InputBase _normalRelase;
    [SerializeField] private InputBase _automaticRelease;

    private void Awake()
    {
        if(_automaticReleasable)
        {
            _releaseInput = _automaticRelease;
        }
        else
        {
            _releaseInput = _normalRelase;
        }
    }
}