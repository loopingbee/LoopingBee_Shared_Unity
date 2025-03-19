using System;
using System.Collections.Generic;
using LoopingBee.Shared.LitJson;

namespace LoopingBee.Shared.Data
{
    [Serializable]
    public class UserData
    {
        public string name;

        public int highScore;
    }

    [Serializable]
    public class LevelData
    {
        public int id;
        public int difficulty;
    }

    [Serializable]
    public class ProductData
    {
        public string product_id;
        public int coins;
    }

    [Serializable]
    public class LBGameData
    {
#if UNITY_EDITOR
        [JsonSkipAttribute]
#endif
        public UserData user;
        public LevelData level;
        public Dictionary<string, ProductData> products;
    }
}