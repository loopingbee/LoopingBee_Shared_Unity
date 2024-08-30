using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LoopingBee.TikGames
{
    public class TikGamesInput : MonoBehaviour
    {
        static TikGamesInput _instance;
        public static TikGamesInput Instance => _instance;

        public event Action<string> OnDataReceived;

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
    }
}