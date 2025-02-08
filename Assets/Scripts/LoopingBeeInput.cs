using System;
using System.Runtime.InteropServices;
using LoopingBee.Shared.LitJson;
using LoopingBee.Shared.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace LoopingBee.Shared
{
    public class LoopingBeeInput : MonoBehaviour
    {
        static LoopingBeeInput _instance;
        public static LoopingBeeInput Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<LoopingBeeInput>("LoopingBeeInput");
                    _instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }

                return _instance;
            }
        }

        public delegate void OnDataReceivedDelegate(string data);
        public event OnDataReceivedDelegate OnDataReceived;

        public enum PurchaseResult
        {
            Success,
            Failure
        }

        public delegate void OnPurchaseResultDelegate(string product_id, string uuid, PurchaseResult result);

        [DllImport("__Internal")]
        private static extern void gameOver(bool won, int score, float showcaseDelay);

        [DllImport("__Internal")]
        private static extern void purchaseProduct(string product_id, string uuid);

        [SerializeField] Sprite[] avatarIcons;
        [SerializeField] Sprite defaultAvatar;

        string data;
        readonly Dictionary<string, OnPurchaseResultDelegate> purchaseCallbacks = new();

        internal Sprite DefaultAvatar => defaultAvatar;
        internal string DefaultAvatarBackground => "#95A5A6";

        void Awake()
        {
            Application.targetFrameRate = 60;

            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLInput.captureAllKeyboardInput = false;
            Cursor.lockState = CursorLockMode.None;
            WebGLInput.stickyCursorLock = false;
#endif
            SceneManager.LoadScene(1);
        }

        public void ReceiveInput(string input)
        {
            data = input;

            Debug.Log("Received data: " + data);

            OnDataReceived?.Invoke(data);
            SceneManager.LoadScene(1);
        }

        public bool HasGameData() => !string.IsNullOrEmpty(data);

        public T GetGameData<T>() where T : LBGameData => JsonMapper.ToObject<T>(data);

        public void GameOver(bool won, int score, float showcaseDelay = 0)
        {
#if !UNITY_EDITOR
            gameOver(won, score, showcaseDelay);
#endif
        }

        public Sprite GetAvatarSprite(string name)
        {
            if (string.IsNullOrEmpty(name))
                return defaultAvatar;

            foreach (var sprite in avatarIcons)
            {
                if (sprite.name == name)
                    return sprite;
            }

            return defaultAvatar;
        }

        public void PurchaseProduct(string product_id, OnPurchaseResultDelegate onPurchaseResult)
        {
            var gameData = GetGameData<LBGameData>();
            if (!gameData.products.ContainsKey(product_id))
            {
                onPurchaseResult?.Invoke(product_id, null, PurchaseResult.Failure);
                return;
            }

            var uuid = Guid.NewGuid().ToString();
            purchaseCallbacks.Add(uuid, onPurchaseResult);

#if !UNITY_EDITOR
            purchaseProduct(product_id, uuid);
#else
            ResolvePurchase(product_id, uuid, (int)PurchaseResult.Success);
#endif
        }

        internal void ResolvePurchase(string product_id, string uuid, int result)
        {
            purchaseCallbacks[uuid]?.Invoke(product_id, uuid, (PurchaseResult)result);
            purchaseCallbacks.Remove(uuid);
        }
    }
}