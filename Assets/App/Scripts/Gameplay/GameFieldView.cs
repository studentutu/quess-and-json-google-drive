using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay
{
    public class GameFieldView : MonoBehaviour
    {
        [Serializable]
        public class ImageAndMore
        {
            public RawImage image = null;
            public Button button = null;
            [HideInInspector] public int id = 0;
            [HideInInspector] public int paired = -1;
        }

        private enum GameState
        {
            Prepare,
            Play,
            End
        }

        [SerializeField] private List<ImageAndMore> allImages = new List<ImageAndMore>();
        [SerializeField] private float prepareTime = 7f;
        [SerializeField] private float gameTime = 60f;
        [SerializeField] private TMPro.TMP_Text text = null;
        private ImageAndMore first = null;
        private GameState gameState = GameState.Prepare;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2f);
            Initialize();
        }

        private void Initialize()
        {
            Generate();
            var time = GameTimers.GetNowTimestampSeconds();
            GameTimers.AddNewTimer(gameState.ToString(), time, time + (long)prepareTime,
            (currentTimer) =>
            {
                gameState = GameState.Play;
                var time2 = GameTimers.GetNowTimestampSeconds();
                GameTimers.AddNewTimer(gameState.ToString(), time, time + (long)gameTime,
                            (currentTimer2) =>
                            {
                                gameState = GameState.End;
                                Clear();
                            },
                            (timeLeft2) =>
                            {
                                text.text = timeLeft2.ToString();
                            }
                );
            },
            (timeLeft) =>
            {
                text.text = timeLeft.ToString();
            }
            );
        }

        private void Generate()
        {
            int length = allImages.Count;
            var shuffled = RandomExtension.ShuffledArray(length);
            float color = 0;
            for (int i = 0; i < length; i += 2)
            {
                color = i / length;
                allImages[shuffled[i]].id = shuffled[i];
                allImages[shuffled[i + 1]].id = shuffled[i + 1];

                allImages[shuffled[i]].image.color = new Color(color, color, color, 1);
                allImages[shuffled[i + 1]].image.color = new Color(color, color, color, 1);

                allImages[shuffled[i]].paired = allImages[shuffled[i + 1]].id;
                allImages[shuffled[i + 1]].paired = allImages[shuffled[i]].id;
            }
        }
        public void Play()
        {

        }
        private void Clear()
        {

        }
    }
}
