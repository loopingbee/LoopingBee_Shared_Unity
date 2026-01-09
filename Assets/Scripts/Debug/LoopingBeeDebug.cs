using System.Collections.Generic;
using UnityEngine;

namespace LoopingBee.Shared.LBDebug
{
    public class LoopingBeeDebug : MonoBehaviour
    {
        [SerializeField] Transform debugContainer;
        [SerializeField] Transform levelChoiceContainer;
        [SerializeField] LevelOption levelOptionPrefab;

        readonly List<LevelOption> levelOptions = new();

        void Awake()
        {
            debugContainer.gameObject.SetActive(false);

#if !UNITY_EDITOR
            Destroy(this);
            return;
#endif
        }

        void Start()
        {
            var levels = new Dictionary<string, string>();
            var levelFiles = System.IO.Directory.GetFiles(Application.dataPath + "/../snp_levels", "*.json");
            foreach (var file in levelFiles)
            {
                var levelName = System.IO.Path.GetFileNameWithoutExtension(file);
                levels[levelName] = file;

                var levelOption = Instantiate(levelOptionPrefab, levelChoiceContainer);
                levelOption.Setup(levelName);
                levelOptions.Add(levelOption);
            }
        }

        public void ToggleDebug()
        {
            debugContainer.gameObject.SetActive(!debugContainer.gameObject.activeSelf);
        }
    }
}