using System;
using System.Runtime.InteropServices;
using LoopingBee.Shared.LitJson;
using LoopingBee.Shared.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace LoopingBee.Shared
{
    public class LoopingBeePurchasing : MonoBehaviour
    {
        static LoopingBeePurchasing _instance;
        public static LoopingBeePurchasing Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<LoopingBeePurchasing>("LoopingBeePurchasing");
                    _instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                }

                return _instance;
            }
        }

        public delegate void OnPurchaseResultDelegate(string product_id, string uuid, LBPurchaseResult result);


        [DllImport("__Internal")]
        private static extern void purchaseProduct(string product_id, string uuid);

        readonly Dictionary<string, OnPurchaseResultDelegate> purchaseCallbacks = new();

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        public void PurchaseProduct(string product_id, OnPurchaseResultDelegate onPurchaseResult)
        {
#if !UNITY_EDITOR
            var gameData = GetGameData<LBGameData>();

            if (!gameData.products.ContainsKey(product_id))
            {
                onPurchaseResult?.Invoke(product_id, null, LBPurchaseResult.Failure);
                return;
            }
#endif

            var uuid = Guid.NewGuid().ToString();
            purchaseCallbacks.Add(uuid, onPurchaseResult);

#if !UNITY_EDITOR
            purchaseProduct(product_id, uuid);
#else
            ResolvePurchaseInternal(product_id, uuid, (int)LBPurchaseResult.Success);
#endif
        }

        internal void ResolvePurchase(string json)
        {
            var data = JsonMapper.ToObject(json);
            ResolvePurchaseInternal(data["product_id"].ToString(), data["uuid"].ToString(), (int)data["result"]);
        }

        void ResolvePurchaseInternal(string product_id, string uuid, int result)
        {
            purchaseCallbacks[uuid]?.Invoke(product_id, uuid, (LBPurchaseResult)result);
            purchaseCallbacks.Remove(uuid);
        }
    }
}