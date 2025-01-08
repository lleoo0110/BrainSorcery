using HolmonUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Windows;
using Cysharp.Threading.Tasks;

public class MagicController : MonoBehaviour
{
    [Serializable]
    public class MagicInputs
    {
        public MagicType type;
        public InputBase generateInput;
    }

    [Header("必要コンポーネント")]
    [SerializeField] private MagicGenerator _magicGenerator;
    [SerializeField] private MagicInputs[] _generateInput;
    [SerializeField] protected InputBase _releaseInput;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private CharacterMove _characterMove;
    [SerializeField] private LogLecorder _logLecorder;

    [Header("パラメータ")]
    [SerializeField] private float _magicShrinkSpeed = 0.02f;
    [SerializeField] private int _intervalS = 3;
    [SerializeField] private float _initialDelay = 2f; // 敵生成後の入力待機時間（秒）

    [Header("投擲アニメーション関連")]
    [SerializeField] private Transform _throwedPos;
    [SerializeField] private float _throwAnimationTime = 1f;
    [SerializeField] private ParticlesPlayer _hitParticle;

    [Header("演出")]
    [SerializeField] private string _fireExplodeSE;
    [SerializeField] private string _thunderExplodeSE;
    [SerializeField] private string _shootSE;

    private bool _magicGeneratable = false;
    private float _delayTimer = 0f;
    private bool _isDelaying = true;

    void Start()
    {
        foreach(var input in _generateInput)
        {
            input.generateInput.AssignInputOnCallback(() =>
            {
                InputOn(input.type);
            });
        }

        _releaseInput.AssignInputOnCallback(() =>
        {
            if(_magicGenerator.generatingType != MagicType.None)
            {
                float rat = _magicGenerator.effectScaleRat;
                var type = _magicGenerator.generatingType;
                var mag = _magicGenerator.Release();
                ThrowMagic(mag, rat, type);

                _magicGeneratable = false;

                UniTask.Void(async () =>
                {
                    await UniTask.Delay(_intervalS * 1000);
                    _magicGeneratable = true;
                });
            }
        });

        _magicGeneratable = true;
    }

    private void Update()
    {
        if (!_enemyGenerator.isExistEnemy) return;
        if (_characterMove.MovementTimes != 2) return;

        // 遅延タイマーの処理
        if (_isDelaying)
        {
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= _initialDelay)
            {
                _isDelaying = false;
                _magicGeneratable = true;
                Debug.Log("Magic input enabled!");
            }
        }
    }

    public void InputOn(MagicType type)
    {
        if (!_magicGeneratable || _isDelaying) return;  // 遅延中は入力を受け付けない

        int logType = type switch
        {
            MagicType.Fire => 2,
            MagicType.Electric => 3,
            _ => -1
        };

        if (_magicGenerator.generatingType == type)
        {
            _magicGenerator.SetScate(_magicGenerator.effectScaleRat + (_magicShrinkSpeed), null);
            if (logType != -1) _logLecorder.AddInput(logType);
        }
        else if (_magicGenerator.generatingType != MagicType.None)
        {
            _magicGenerator.SetScate(_magicGenerator.effectScaleRat - (_magicShrinkSpeed), () => 
            {
                if (_magicGenerator.effectScaleRat == 0)
                {
                    var gam = _magicGenerator.Release();
                    GameObject.Destroy(gam);
                }
            }); 
            if (logType != -1) _logLecorder.AddInput(logType);
        }
        else if (_magicGenerator.generatingType == MagicType.None)
        {
            if (!_enemyGenerator.isExistEnemy) return;
            if (!_magicGeneratable) return;

            _magicGenerator.Generate(type, _magicShrinkSpeed);
            if(logType != -1) _logLecorder.AddInput(logType);
        }
    }

    private void ThrowMagic(GameObject gam, float rat, MagicType type)
    {
        GlobalSEPlayer.PlaySE(_shootSE);

        gam.transform.DOMove(_throwedPos.position, _throwAnimationTime).OnComplete(() =>
        {
            _enemyGenerator.DamageEnemy(rat, type);

            if(type == MagicType.Fire) GlobalSEPlayer.PlaySE(_fireExplodeSE);
            else if (type == MagicType.Electric) GlobalSEPlayer.PlaySE(_thunderExplodeSE);

            _hitParticle.Play();
            GameObject.Destroy(gam);
        });

        _logLecorder.AddInput(4);
    }

    // 敵生成時にこのメソッドを呼び出して入力遅延を開始
    public void StartInputDelay()
    {
        _magicGeneratable = false;
        _isDelaying = true;
        _delayTimer = 0f;
        Debug.Log("Starting magic input delay...");
    }
}