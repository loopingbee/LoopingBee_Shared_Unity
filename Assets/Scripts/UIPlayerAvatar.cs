using UnityEngine;
using UnityEngine.UI;

namespace LoopingBee.Shared
{
    public class UIPlayerAvatar : BasePlayerAvatar
    {
        [SerializeField] Image avatarIcon;
        [SerializeField] Image avatarBackground;

        public override void SetAvatarBackground(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                avatarBackground.color = color;
        }

        public override void SetAvatarIcon(Sprite sprite) => avatarIcon.sprite = sprite;
    }
}