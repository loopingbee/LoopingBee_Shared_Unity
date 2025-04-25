using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoopingBee.Shared.Data
{
    [Serializable]
    public class LBSafeAreaData
    {
        public float top;
        public float bottom;
        public float left;
        public float right;
    }

    [RequireComponent(typeof(RectTransform))]
    public class LBSafeArea : MonoBehaviour
    {
        [SerializeField] bool ignoreTop;
        [SerializeField] bool ignoreBottom;
        [SerializeField] bool ignoreLeft;
        [SerializeField] bool ignoreRight;

#if UNITY_EDITOR
        [SerializeField] bool fullscreen;

        bool isFullscreen;
#endif

        void Awake()
        {
            LoopingBeeInput.Instance.OnSafeAreaChanged += OnSafeAreaChanged;
#if UNITY_EDITOR
            OnSafeAreaChanged(new()
            {
                bottom = 0.077767f,
                left = 0f,
                right = 0.127226f,
                top = 0.081107f,
            });
#endif
        }

        void OnDestroy()
        {
            LoopingBeeInput.Instance.OnSafeAreaChanged -= OnSafeAreaChanged;
        }

        void OnSafeAreaChanged(LBSafeAreaData data)
        {
            var rectTransform = GetComponent<RectTransform>();

            var anchorMin = new Vector2(ignoreLeft ? 0 : data.left, ignoreBottom ? 0 : data.bottom);
            var anchorMax = new Vector2(ignoreRight ? 1 : (1 - data.right), ignoreTop ? 1 : (1 - data.top));

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }

#if UNITY_EDITOR
        void Update()
        {
            if (fullscreen != isFullscreen)
            {
                TestRefresh();
                isFullscreen = fullscreen;
            }
        }

        void OnValidate()
        {
            TestRefresh();
        }

        void TestRefresh()
        {
            if (fullscreen)
                OnSafeAreaChanged(new()
                {
                    bottom = 0.077767f,
                    left = 0f,
                    right = 0f,
                    top = 0f,
                });
            else
                OnSafeAreaChanged(new()
                {
                    bottom = 0.077767f,
                    left = 0f,
                    right = 0.127226f,
                    top = 0.081107f,
                });
        }
#endif
    }
}