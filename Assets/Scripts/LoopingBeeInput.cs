using System;
using System.Runtime.InteropServices;
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

        public event Action<string> OnDataReceived;

        [DllImport("__Internal")]
        private static extern void gameOver(bool won);

        string data;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
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

        public void ReceiveInput(string input)
        {
            data = input;

            Debug.Log("Received data: " + data);

            OnDataReceived?.Invoke(data);
            SceneManager.LoadScene(1);
        }

        public bool HasGameData() => !string.IsNullOrEmpty(data);

        public T GetGameData<T>() => JsonUtility.FromJson<T>(data);

        public void GameOver(bool won)
        {
#if !UNITY_EDITOR
            gameOver(won);
#endif
        }
    }
}