using System;
using System.Collections.Generic;
using Codice.Client.Common.GameUI.Checkin;
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

    [Serializable]
    public class LBLevelData<T> where T : LBGameData
    {
        [Serializable]
        public class SnPMetadata
        {
            public int difficulty;
        }

        public SnPMetadata _snp_metadata;
        public T content;
    }
}