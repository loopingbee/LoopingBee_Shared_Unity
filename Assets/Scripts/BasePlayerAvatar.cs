using UnityEngine;
using LoopingBee.Shared.Data;

namespace LoopingBee.Shared
{
    public abstract class BasePlayerAvatar : MonoBehaviour
    {
        public abstract void SetAvatarBackground(string hex);
        public abstract void SetAvatarIcon(Sprite sprite);
        public void SetAvatarIconAndBackground(Sprite sprite, string hex)
        {
            SetAvatarIcon(sprite);
            SetAvatarBackground(hex);
        }

        protected virtual void Start()
        {
            SetAvatarIconAndBackground(LoopingBeeInput.Instance.DefaultAvatar, LoopingBeeInput.Instance.DefaultAvatarBackground);

            if (LoopingBeeInput.Instance.HasGameData())
            {
                var data = LoopingBeeInput.Instance.GetGameData<GameData>();
                SetAvatarIconAndBackground(LoopingBeeInput.Instance.GetAvatarSprite(data.user.avatar_icon), data.user.avatar_background_hex);
            }

            LoopingBeeInput.Instance.OnDataReceived += input =>
            {
                var data = LoopingBeeInput.Instance.GetGameData<GameData>();
                SetAvatarIconAndBackground(LoopingBeeInput.Instance.GetAvatarSprite(data.user.avatar_icon), data.user.avatar_background_hex);
            };
        }
    }
}