using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HolmonUtility
{
    public static class GlobalSEPlayer
    {
        public static void PlaySE(string key, int channnel = 0)
        {
            //ヒエラルキーからSEPlayerを探す
            SEPlayer sePlayer = GameObject.FindObjectOfType<SEPlayer>();

            if (sePlayer == null)
            {
                Debug.LogError("SEPlayerが見つかりませんでした");
                return;
            }

            sePlayer.PlaySE(key, channnel);
        }
    }

    public class SEPlayer : MonoBehaviour
    {
        [Serializable]
        public class SEs
        {
            public string key;
            public AudioClip clip;
        }

        [SerializeField] private List<SEs> _serializedSeList;
        [SerializeField] private int _channelLimit = 3;
        [SerializeField] private GameObject _channelPrefab;

        private List<AudioSource> _channnels = new List<AudioSource>();
        Dictionary<string, AudioClip> _seList { get
            {
                //SEsクラスのリストをDictionaryに変換する
                Dictionary<string, AudioClip> seList = new Dictionary<string, AudioClip>();
                foreach (var se in _serializedSeList)
                {
                    seList.Add(se.key, se.clip);
                }
                return seList;
            } }

        protected virtual void Start()
        {
            CheckList();
            GenerrateChannels();
        }

        //同名キーのSEがある場合はエラーを出す
        private void CheckList()
        {
            HashSet<string> hashSet = new HashSet<string>();
            foreach (var se in _seList)
            {
                if (hashSet.Contains(se.Key))
                {
                    Debug.LogError("同名のSEが存在します : " + se.Key);
                }
                else
                {
                    hashSet.Add(se.Key);
                }
            }
        }
        //チャンネルの生成を行う
        private void GenerrateChannels()
        {
            //_channelLimit分の_channelPrefabを子オブジェクトとして生成する
            for (int i = 0; i < _channelLimit; i++)
            {
                GameObject channel = Instantiate(_channelPrefab, this.transform);
                _channnels.Add(channel.GetComponent<AudioSource>());
            }
        }

        //SEを再生する
        public void PlaySE(string key, int channnel = 0)
        {
            //channnelが0未満の場合はエラーを出す
            if (channnel < 0)
            {
                Debug.LogError("チャンネル番号が不正です : " + channnel);
                return;
            }
            //channnelが_channnelsの要素数以上の場合はエラーを出す
            if (channnel >= _channnels.Count)
            {
                Debug.LogError("チャンネル番号が不正です : " + channnel);
                return;
            }

            //keyが_seListに存在しない場合はエラーを出す
            if (!_seList.ContainsKey(key))
            {
                Debug.LogError("指定されたキーがSEリストに存在しません : " + key);
                return;
            }

            //指定のチャンネルでSEを再生する
            _channnels[channnel].PlayOneShot(_seList[key]);
        }
    }
}



