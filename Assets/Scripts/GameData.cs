using System;

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
    public class GameData
    {
        public UserData user;
    }
}