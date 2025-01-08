using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * --------------------WARNING--------------------
 * このコードを使用するにはヒエラルキー上に「CoroutineHandler」という名前のGameObjectを作成し
 * 本スクリプトをアタッチしておく必要があります
 */

namespace HolmonUtility
{
    public class CoroutineHandler : MonoBehaviour
    {
        static protected CoroutineHandler m_Instance;
        static public CoroutineHandler instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject o = new GameObject("CoroutineHandler");

                    if(o == null)
                    {
                        Debug.LogError("CoroutineHandlerオブジェクトがヒエラルキー上に存在しません");
                    }

                    DontDestroyOnLoad(o);
                    m_Instance = o.AddComponent<CoroutineHandler>();
                }

                return m_Instance;
            }
        }

        public void OnDisable()
        {
            if (m_Instance)
                Destroy(m_Instance.gameObject);
        }

        //コルーチンの再生開始
        static public Coroutine StartStaticCoroutine(IEnumerator coroutine)
        {
            return instance.StartCoroutine(coroutine);
        }

        //コルーチンの再生終了
        static public void StopStaticCoroutine(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }
    }
}