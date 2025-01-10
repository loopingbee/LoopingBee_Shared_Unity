using System;

namespace LoopingBee.Shared.Data
{
#if !UNITY_EDITOR
    [Serializable]
#endif
    public class UserData
    {
        public string name;

        public string avatar_icon;
        public string avatar_background_hex;

        public int highScore;
    }

    [Serializable]
    public class GameData
    {
        public UserData user;
    }
}