using System;
using LoopingBee.Shared.LitJson;

namespace LoopingBee.Shared.Data
{
    [Serializable]
    public class UserData
    {
        public string name;

        public string avatar_icon;
        public string avatar_background_hex;

        public int highScore;
    }

    [Serializable]
    public class LevelData
    {
        public int id;
        public int difficulty;
    }

    [Serializable]
    public class LBGameData
    {
#if UNITY_EDITOR
        [JsonSkipAttribute]
#endif
        public UserData user;
        public LevelData level;
    }
}