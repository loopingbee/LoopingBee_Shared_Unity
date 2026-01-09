using UnityEngine;
using UnityEngine.UI;

namespace LoopingBee.Shared.LBDebug
{
    public class LevelOption : MonoBehaviour
    {
        [SerializeField] Text levelNameText;

        string levelName;

        public void Setup(string levelName)
        {
            this.levelName = levelName;

            levelNameText.text = levelName;
        }

        public void PlayLevel()
        {
            var levelContents = System.IO.File.ReadAllText(System.IO.Path.Combine(Application.dataPath + "/../snp_levels", levelName + ".json"));
            LoopingBeeInput.Instance.ReceiveInput(levelContents);
        }
    }
}