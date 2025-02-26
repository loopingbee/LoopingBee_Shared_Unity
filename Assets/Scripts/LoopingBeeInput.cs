using System.Runtime.InteropServices;
using LoopingBee.Shared.LitJson;
using LoopingBee.Shared.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public delegate void OnContinueDelegate();
        public event OnContinueDelegate OnContinue;

        [DllImport("__Internal")]
        private static extern void gameOver(bool won, int score, bool allowContinue, float showcaseDelay);

        [SerializeField] Sprite[] avatarIcons;
        [SerializeField] Sprite defaultAvatar;

        string data;

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

        public void ReceiveInput(string input) => ReceiveInput(input, true);
        public void ReceiveInput(string input, bool reloadScene = true)
        {
            data = input;

            Debug.Log("Received data: " + data);

            OnDataReceived?.Invoke(data);

            if (reloadScene)
                SceneManager.LoadScene(1);
        }

        public bool HasGameData() => !string.IsNullOrEmpty(data);

        public T GetGameData<T>() where T : LBGameData => JsonMapper.ToObject<T>(data);

        public void GameOver(bool won, int score, bool allowContinue, float showcaseDelay = 0)
        {
#if !UNITY_EDITOR
            gameOver(won, score, allowContinue, showcaseDelay);
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

        internal void UseContinue()
        {
            OnContinue?.Invoke();
        }
    }
}