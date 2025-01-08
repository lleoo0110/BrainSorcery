namespace HolmonUtility
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEditor;

    public class TaskTweener
    {
        private readonly float TWEEN_TIME;

        private float _progressedTime = 0;
        private bool _canceled = false;

        public TaskTweener(float tweenTime)
        {
            TWEEN_TIME = tweenTime;
            _progressedTime = 0;
            _canceled = false;
        }

        public async Task Play(float startValue, float endValue, Action<float> receiveValue)
        {
            _progressedTime = 0;
            _canceled = false;

            while (true)
            {
                float r = _progressedTime / TWEEN_TIME;
                if (r > 1) r = 1;

                float sendV = (endValue - startValue) * r + startValue;
                receiveValue(sendV);

                if (_canceled || r >= 1) break;
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) break;
#endif

                double nowT = AudioSettings.dspTime;

                await Task.Delay(10);

                double diff = AudioSettings.dspTime - nowT;

                _progressedTime += (float)diff;
            }
        }

        public void Stop()
        {
            _canceled = true;
        }
    }
}