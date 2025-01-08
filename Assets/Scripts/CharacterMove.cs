using HolmonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMove : MonoBehaviour
{
    [Header("入力")]
    [SerializeField] private InputBase _moveInput;
    [SerializeField] private InputBase _initInput;

    [Header("移動パラメータ")]
    [SerializeField] private int _movementLimitTimes;
    [SerializeField] private int _movementValue;
    [SerializeField] private float _movementTime;
    [SerializeField] private Vector3 _initPosition;

    [Header("演出")]
    [SerializeField] private string _walkSE;

    [Header("参照")]
    [SerializeField] private Transform _characterTransform;

    /// <summary>
    /// 移動した回数
    /// </summary>
    public int MovementTimes => _movementTimes;

    private int _movementTimes = 0;
    private bool _moving = false;

    private void Start()
    {
        _initInput.AssignInputOnCallback(() =>
        {
            InitPosition();
        });

        _moveInput.AssignInputOnCallback(() =>
        {
            MovePosition();
        });
    }

    /// <summary>
    /// 位置の初期化
    /// </summary>
    public void InitPosition()
    {
        _movementTimes = 0;

        _characterTransform.position = _initPosition;
    }

    /// <summary>
    /// 位置の移動
    /// </summary>
    public void MovePosition()
    {
        //もし移動回数が制限値を超えていたらreturnする
        if (_movementTimes >= _movementLimitTimes) return;

        //もし移動中ならreturnする
        if (_moving) return;


        _moving = true;
        _characterTransform.DOMove(_characterTransform.position + new Vector3(0, 0, _movementValue), _movementTime)
            .OnComplete(() =>
            {
                _moving = false;
                _movementTimes++;
            });

        GlobalSEPlayer.PlaySE(_walkSE);
    }
}