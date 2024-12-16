## How to Use This Library

This library serves as a bridge between the JavaScript engine of the host app and the game itself. The JavaScript engine will call `ReceiveInput` with the JSON representation of the level data.

1. **Import the Library**: Add the library to your Unity project using the Unity Package Manager.
    - Open the Unity Editor.
    - Go to `Window` > `Package Manager`.
    - Click on the `+` button and select `Add package from git URL...`.
    - Enter the URL of the library's repository and click `Add`.

2. **Include the Namespace**: In your C# scripts, include the namespace of the library.
    ```csharp
    using LoopingBee.Shared;
    ```
3. **Receive Input Data**: The JavaScript engine will call the `ReceiveInput` method with the JSON representation of the level data.
    ```csharp
    LoopingBeeInput.Instance.ReceiveInput("your input data");
    ```
4. **Subscribe to Events**: Subscribe to the `OnDataReceived` event to handle received data.
    ```csharp
    LoopingBeeInput.Instance.OnDataReceived += (data) => {
        Debug.Log("Data received: " + data);
    };
    ```
5. **Check for Game Data**: Use the `HasGameData` method to check if there is any game data.
    ```csharp
    bool hasData = LoopingBeeInput.Instance.HasGameData();
    ```
6. **Retrieve Game Data**: Use the `GetGameData` method to retrieve the game data.
    ```csharp
    var gameData = LoopingBeeInput.Instance.GetGameData<YourDataType>();
    ```
7. **Handle Game Over**: Use the `GameOver` method to signal the end of the game.
    ```csharp
    LoopingBeeInput.Instance.GameOver(true);
    ```

## Example Game Controller

Here is an example game controller using this library:

```csharp
using UnityEngine;
using LoopingBee.Shared;
using System.Collections;
using LoopingBee.Example.Data;

namespace LoopingBee.Example
{
    public class GameController : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] string testData;
#endif
        GameData data;

        void Start()
        {
            if (LoopingBeeInput.Instance.HasGameData())
            {
                var data = LoopingBeeInput.Instance.GetGameData<GameData>();
                ReadData(data);
            }

            LoopingBeeInput.Instance.OnDataReceived += input =>
            {
                data = LoopingBeeInput.Instance.GetGameData<GameData>();
                ReadData(data);
            };

#if UNITY_EDITOR
            if (string.IsNullOrEmpty(testData))
                ReadData(GameData.TestData());
            else
                ReadData(JsonUtility.FromJson<GameData>(testData));
#endif

            if (data != null)
                LoadData();

            StartGame();
        }

        public void StartGame()
        {
            StartCoroutine(GameRoutine());
        }

        void ReadData(GameData data)
        {
            this.data = data;
#if UNITY_EDITOR
            Debug.Log(JsonUtility.ToJson(data, false));
#endif
        }

        IEnumerator GameRoutine()
        {
            yield return new WaitUntil(() => data != null);
            LoadData();
        }

        void LoadData()
        {
            // Load your game data
        }

        public void GameOver(bool won)
        {
            LoopingBeeInput.Instance.GameOver(won);
        }
    }
}
```
