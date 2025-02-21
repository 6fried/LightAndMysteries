using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LaserGamePage : UIPage
{
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private Button levelButtonPrefab;

    private LaserGameConfig gameConfig;

    private void Start()
    {
        gameConfig = Resources.Load<GameConfig>("Game/Configs/GameConfig").laserGameConfig;
    }

    private void OnEnable()
    {
        ConfigLevels();
    }

    private void ConfigLevels()
    {
        string levelFolderPath = Path.Combine(Application.dataPath, "Games/LazerMaze/Resources/Levels");

        int fileCount = CountFilesInDirectory(levelFolderPath);

        for (int i = 1; i <= fileCount; i++)
        {
            Button b = Instantiate(levelButtonPrefab, content);
            b.GetComponentInChildren<TMP_Text>().text = i.ToString();
            b.onClick.AddListener(SelectLevel(i));
        }
    }

    UnityAction SelectLevel(int i)
    {
        return () =>
        {
            gameConfig.level = i;
            MainMenu.instance.GoToScene("LazerMaze");
        };
    }
    private int CountFilesInDirectory(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath, "*.lvl", SearchOption.TopDirectoryOnly);
            return files.Length;
        }
        else
        {
            Debug.LogError("The specified folder does not exist");
            return 0;
        }
    }
}
