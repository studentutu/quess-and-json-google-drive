using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public class Timer : MonoBehaviour
    {
        private long _preTime;

        public string id;
        public long startTime;
        public long endTime;
        public long timeLeft;
        public bool isComplete = false;

        public Action<long> OnUpdate;
        public Action<Timer> OnComplete;

        public long GetTimeLeft()
        {
            var time = endTime - GameTimers.GetNowTimestampSeconds();
            return time < 0 ? 0 : time;
        }

        private void Update()
        {
            _preTime = timeLeft;
            timeLeft = GetTimeLeft();

            if (_preTime != timeLeft) OnUpdate?.Invoke(timeLeft);

            if (timeLeft <= 0)
            {
                Debug.Log(GameTimers.TIMER + id + " Completed!");
                isComplete = true;
                GameTimers.RemoveTimer(id);

                OnComplete?.Invoke(this);
            }
        }

        public void Stop()
        {
            Debug.Log(GameTimers.TIMER + id + " Stoped!");
            OnUpdate?.Invoke(0);
            isComplete = true;
            GameTimers.RemoveTimer(id);
        }

        private void LateUpdate()
        {
            if (isComplete)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}
