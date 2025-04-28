using System.Collections;

using LoopingBee.Shared.Data;
using LoopingBee.Shared.LitJson;

using UnityEngine;

namespace LoopingBee.Shared
{
    public abstract class GameController<T> : MonoBehaviour where T : LBGameData
    {
        public static GameController<T> Instance { get; private set; }

#if UNITY_EDITOR
        [SerializeField] string testData;
#endif

        public T Data { get; private set; }

        public bool IsGameOver { get; protected set; }
        public bool IsGameStarted { get; private set; }

        public bool IsPlaying => IsGameStarted && !IsGameOver;

        protected virtual void Awake()
        {
            Application.targetFrameRate = 60;

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        protected virtual void Start()
        {
            if (LoopingBeeInput.Instance.HasGameData())
            {
                var data = LoopingBeeInput.Instance.GetGameData<T>();
                ReadData(data);
            }

            LoopingBeeInput.Instance.OnDataReceived += OnDataReceived;
            LoopingBeeInput.Instance.OnContinue += OnContinue;

#if UNITY_EDITOR
            if (!LoopingBeeInput.Instance.HasGameData())
            {
                if (string.IsNullOrEmpty(testData))
                    LoopingBeeInput.Instance.ReceiveInput(GetTestData(), false);
                else
                    LoopingBeeInput.Instance.ReceiveInput(testData, false);
            }
#endif

            StartCoroutine(GameRoutine());
        }

        protected virtual void OnDestroy()
        {
            LoopingBeeInput.Instance.OnDataReceived -= OnDataReceived;
        }

        void OnDataReceived(string input)
        {
            Data = LoopingBeeInput.Instance.GetGameData<T>();
            ReadData(Data);
        }

        protected virtual void OnContinue()
        {
            IsGameOver = false;
        }

        protected virtual void ReadData(T data)
        {
            Data = data;
#if UNITY_EDITOR
            Debug.Log(JsonMapper.ToJson(data));
#endif
        }

        protected virtual IEnumerator GameRoutine()
        {
            yield return new WaitUntil(() => Data != null);
            LoadData();
            IsGameStarted = true;
        }

        protected abstract void LoadData();

        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.J))
                if (string.IsNullOrEmpty(testData))
                    LoopingBeeInput.Instance.ReceiveInput(GetTestData());
                else
                    LoopingBeeInput.Instance.ReceiveInput(testData);

            if (Input.GetKeyDown(KeyCode.C))
                OnContinue();
#endif
        }

        protected abstract string GetTestData();
    }
}
