using UnityEngine;

namespace LoopingBee.Shared
{
    public class SpritePlayerAvatar : BasePlayerAvatar
    {
        [SerializeField] SpriteRenderer avatarIcon;
        [SerializeField] SpriteRenderer avatarBackground;

        public override void SetAvatarBackground(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                avatarBackground.color = color;
        }

        public override void SetAvatarIcon(Sprite sprite) => avatarIcon.sprite = sprite;
    }
}