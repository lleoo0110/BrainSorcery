
namespace HolmonUtility
{
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices.ComTypes;
    using System.Threading.Tasks;
    using UnityEngine;

    public interface IFileLoadLogic<T>
    {
        Task<T> LoadFile(string path);
    }

    public class JsonLoader<T>
    {
        private readonly IFileLoadLogic<string> _loadLogic = null;

        public JsonLoader(IFileLoadLogic<string> logic)
        {
            _loadLogic = logic;
        }

        public async Task<T> JsonLoad(string path)
        {
            if(_loadLogic == null)
            {
                Debug.LogError("テキストアセットの読み込みロジックが設定されていません");
                return default;
            }

            string jsonString = await _loadLogic.LoadFile(path);

            T data = JsonUtility.FromJson<T>(jsonString);

            return data;
        }

        public T JsonLoad(TextAsset textAsset)
        {
            T data = JsonUtility.FromJson<T>(textAsset.ToString());

            return data;
        }
    }

    public class JsonLoadByFileReadAllText : IFileLoadLogic<string>
    {
        public async Task<string> LoadFile(string path)
        {
            return await File.ReadAllTextAsync(path);
        }
    }
}
