namespace HolmonUtility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using UnityEngine;
    
    public enum eInputType
    {
        On,
        Off
    }

    //入力の基底クラス
    public abstract class InputBase : MonoBehaviour
    {
        protected Dictionary<string, Action> _inputOnCallBack = new Dictionary<string, Action>();
        protected Dictionary<string, Action> _inputOffCallBack = new Dictionary<string, Action>();

        public bool inputable { get; private set; } = true;

        /// <summary>
        /// 入力がONになった時に呼ばれるコールバックを設定する
        /// </summary>
        /// <param name="inputOnCallBack"></param>
        /// <returns></returns>
        public string AssignInputOnCallback(Action inputOnCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._inputOnCallBack.Add(key, inputOnCallBack);
            return key;
        }
        /// <summary>
        /// 入力がOFFになった時に呼ばれるコールバックを設定する
        /// </summary>
        /// <param name="inputOffCallBack"></param>
        /// <returns></returns>
        public string AssignInputOffCallback(Action inputOffCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._inputOffCallBack.Add(key, inputOffCallBack);
            return key;
        }

        /// <summary>
        /// 入力がONになった時に呼ばれるコールバックを解除する
        /// </summary>
        /// <param name="key"></param>
        public void RemoveInputOnCallback(string key)
        {
            bool res = this._inputOnCallBack.Remove(key);
            if (!res) Debug.LogWarning("コールバック一覧に登録されていないコールバックを解除しようとしました");
        }
        /// <summary>
        /// 入力がOFFになった時に呼ばれるコールバックを解除する
        /// </summary>
        /// <param name="key"></param>
        public void RemoveInputOffCallback(string key)
        {
            bool res = this._inputOffCallBack.Remove(key);
            if (!res) Debug.LogWarning("コールバック一覧に登録されていないコールバックを解除しようとしました");
        }

        /// <summary>
        /// このインプットが入力可能かどうかを切り替える
        /// </summary>
        /// <param name="inputable"></param>
        public void SetInputable(bool inputable)
        {
            this.inputable = inputable;
        }

        //コールバックを呼び出す
        protected void ExecuteInputOnCallback()
        {
            if (!inputable) return;

            foreach (var value in _inputOnCallBack.Values)
            {
                value();
            }
        }
        protected void ExecuteInputOffCallback()
        {
            if (!inputable) return;

            foreach (var value in _inputOffCallBack.Values)
            {
                value();
            }
        }

        private void OnDisable()
        {
            _inputOnCallBack.Clear();
            _inputOffCallBack.Clear();
            Debug.Log("InputBaseは初期化されました");
        }
    }
}