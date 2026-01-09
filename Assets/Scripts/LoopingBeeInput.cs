using System.Runtime.InteropServices;
using LoopingBee.Shared.LitJson;
using LoopingBee.Shared.Data;
using LoopingBee.Shared.Data.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoopingBee.Shared
{
    public class LoopingBeeInput : MonoBehaviour
    {
        public static bool IsDestroyed { get; set; }
        static LoopingBeeInput _instance;
        public static LoopingBeeInput Instance
        {
            get
            {
                if (IsDestroyed)
                    return null;

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

        public delegate void OnContinueDelegate();
        public event OnContinueDelegate OnContinue;

        public delegate void OnSafeAreaChangedDelegate(LBSafeAreaData data);
        public event OnSafeAreaChangedDelegate OnSafeAreaChanged;

        public LBSafeAreaData SafeAreaData { get; private set; }

        [DllImport("__Internal")]
        private static extern void gameOver(bool won, int score, bool allowContinue, float showcaseDelay);

        [DllImport("__Internal")]
        private static extern void gameStarted();

        string data;

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

        void OnDestroy()
        {
            if (_instance == this)
            {
                IsDestroyed = true;
                _instance = null;
            }
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

        public void ReceiveInput(string input) => ReceiveInput(input, true);
        public void ReceiveInput(string input, bool reloadScene = true)
        {
            data = input;

            Debug.Log("Received data: " + data);

            OnDataReceived?.Invoke(data);

#if !UNITY_EDITOR
            gameStarted();
#endif

            if (reloadScene)
                SceneManager.LoadScene(1);
        }

        public bool HasGameData() => !string.IsNullOrEmpty(data);

        public T GetGameData<T>() where T : LBGameData
        {
            var parsedData = JsonMapper.ToObject<T>(data);

#if UNITY_EDITOR
            var metadataPath = Application.dataPath + "/../metadata.json";
            if (System.IO.File.Exists(metadataPath))
            {
                var metadata = System.IO.File.ReadAllText(metadataPath);
                var metadataParsed = JsonMapper.ToObject<GameMetadata>(metadata);

                if (parsedData.products == null)
                {
                    parsedData.products = metadataParsed.products;
                    foreach (var kvp in parsedData.products)
                        kvp.Value.product_id = kvp.Key;
                }
            }
#endif

            return parsedData;
        }

        public void GameOver(bool won, int score, bool allowContinue, float showcaseDelay = 0)
        {
#if !UNITY_EDITOR
            gameOver(won, score, allowContinue, showcaseDelay);
#endif
        }

        internal void UseContinue()
        {
            OnContinue?.Invoke();
        }

        public void UpdateSafeArea(string json)
        {
            var safeArea = JsonMapper.ToObject<LBSafeAreaData>(json);
            if (SafeAreaData == null || SafeAreaData.top != safeArea.top || SafeAreaData.bottom != safeArea.bottom ||
                SafeAreaData.left != safeArea.left || SafeAreaData.right != safeArea.right)
            {
                SafeAreaData = safeArea;
                OnSafeAreaChanged?.Invoke(safeArea);
            }
        }
    }
}