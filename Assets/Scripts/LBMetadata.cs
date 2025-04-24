using System;
using System.Collections.Generic;

namespace LoopingBee.Shared.Data.Internal
{
#if UNITY_EDITOR
    [Serializable]
    public class GameMetadata
    {
        public int developer_id;
        public bool hidden;
        public bool highscore_based;
        public string name;
        public Dictionary<string, ProductData> products;
    }
#endif
}