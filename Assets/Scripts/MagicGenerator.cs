using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public enum MagicType
{
    None,
    Fire,
    Water,
    Electric,
    Leaf,
    Ice
}

public class MagicGenerator : MonoBehaviour
{
    [Serializable]
    public class MagicEffect
    {
        public GameObject obj;
        public float maxScale;
        public Vector3 rot;

        public MagicEffect(GameObject obj, float maxScale)
        {
            this.obj = obj;
            this.maxScale = maxScale;
        }
    }

    [SerializeField] private Transform _generateTransform;
    [SerializeField] private MagicEffect _fire;
    [SerializeField] private MagicEffect _electric;

    [SerializeField] private float _tweenTime = 0.5f;

    private MagicEffect _hasMasic;
    public MagicType generatingType { get; private set; } = MagicType.None;
    public float effectScaleRat { get; private set; } = 0;

    float _toRat = 0;
    float _fromRat = 0;
    float _t = 0;

    Action _callBack = null;

    private void Update()
    {
        if (_hasMasic != null)
        {
            if (_toRat != effectScaleRat)
            {
                var tweenRat = _t / _tweenTime;
                effectScaleRat = Mathf.Lerp(_fromRat, _toRat, tweenRat);

                var scale = _hasMasic.maxScale * effectScaleRat;
                _hasMasic.obj.transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                if(_callBack != null)
                {
                    _callBack();
                    _callBack = null;
                }
            }
        }

        _t += Time.deltaTime;
    }

    public void Generate(MagicType type, float rat)
    {
        if(_hasMasic != null)
        {
            Debug.LogError("魔法オブジェクトをまだ所有しています");
            return;
        }

        MagicEffect target = null;
        switch (type)
        {
            case MagicType.Fire:
                target = _fire;
                break;
            case MagicType.Electric:
                target = _electric;
                break;
        }

        var obj = Instantiate(target.obj, _generateTransform.position, Quaternion.Euler(target.rot));
        obj.transform.localScale = Vector3.zero;

        _hasMasic = new MagicEffect(obj, target.maxScale);
        effectScaleRat = 0;
        _fromRat = effectScaleRat;
        _toRat = effectScaleRat;
        generatingType = type;

        SetScate(rat, null);
    }

    public void SetScate(float rat, Action callback)
    {
        if (_hasMasic == null)
        {
            Debug.LogError("魔法オブジェクトをまだ所有していません");
            return;
        }

        rat = Mathf.Clamp01(rat);

        _t = 0;
        _fromRat = effectScaleRat;
        _toRat = rat;

        _callBack = callback;
    }

    public GameObject Release()
    {
        if (_hasMasic == null)
        {
            Debug.LogError("魔法オブジェクトをまだ所有していません");
            return null;
        }

        var ret = _hasMasic.obj;
        _hasMasic = null;
        generatingType = MagicType.None;

        return ret;
    }
}