using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region VARIABLES
    public static GameController instance;

    private Vector3 centerPosition;
    [SerializeField]
    private Vector2 tileSize;
    [SerializeField]
    private GameObject projectorPrefab;
    [SerializeField]
    private Transform environment;

    [SerializeField]
    private GameObject laserSourcePrefab;
    [SerializeField]
    private GameObject goalPrefab;
    [SerializeField]
    private GameObject decalPrefab;
    [SerializeField]
    private GameObject mirror;
    [SerializeField]
    private DescriptionPanel description;

    private Transform level;
    private int targetGoals;
    public string fileName;

    public GameObject endPan;

    private LevelConfig config;
    private bool endGameCheckPassed;
    int currentLevel;
    #endregion

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        currentLevel = SaveManager.LoadLevel() != 0 ? SaveManager.LoadLevel() : 1;
        Debug.Log(currentLevel);
        SetupAsync();
    }

    private async void SetupAsync()
    {
        endGameCheckPassed = false;
        if (currentLevel > 5)
        {
            endPan.SetActive(true);
            return;
        }
        fileName = $"Levels/Level{currentLevel}.json";
        description.text = "hey";
        config = await ConfigReader.ReadConfigAsync(fileName);
        description.text = "hay";
        description.transform.parent.gameObject.SetActive(true);
        if (config != null)
        {
            description.text = config.levelDescription;
        }
        else
        {
            description.text = "Oh Oh, quelque chose s'est mal passé.";
        }
    }

    public void BuildLevel()
    {
        if (config != null)
        {
            targetGoals = config.targetPositions.Count;
            level = new GameObject("Level").transform;
            level.parent = environment;

            // Création des sources
            foreach (var (position, rotation) in config.sourcePositions.Zip(config.sourceRotations, (p, r) => (p, r)))
            {
                Instantiate(laserSourcePrefab, position, Quaternion.Euler(rotation), level);
            }

            // Création des cibles
            foreach (var targetPosition in config.targetPositions)
            {
                Instantiate(goalPrefab, targetPosition, Quaternion.identity, level);
            }

            // Création des conteneurs pour la carte et les objets
            Transform map = new GameObject("Map").transform;
            map.parent = level;

            Transform boxes = new GameObject("Boxes").transform;
            boxes.parent = level;

            int sizeX = config.grid.GetLength(0);
            int sizeY = config.grid.GetLength(1);

            float offsetX = (sizeX % 2 == 0) ? tileSize.x / 2 : 0;
            float offsetY = (sizeY % 2 == 0) ? tileSize.y / 2 : 0;

            float gridWidth = sizeY * tileSize.y;
            float gridHeight = sizeX * tileSize.x;
            centerPosition = new Vector3(gridWidth / 2 - tileSize.y / 2 - offsetY, 0, gridHeight / 2 - tileSize.x / 2 - offsetX);

            Debug.Log(config.grid.GetLength(1));



            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    string letter = config.grid[x, y];
                    Vector3 position = new Vector3(y * tileSize.y, 0, sizeX - x * tileSize.x) - centerPosition;

                    if (letter != "0")
                    {
                        GameObject decal = Instantiate(decalPrefab, position, Quaternion.identity, map);
                        decal.name = x.ToString() + y.ToString();
                    }

                    switch (letter)
                    {
                        case "C":
                            break;
                        case "M":
                            Instantiate(mirror, position + new Vector3(0, 0.5f, 0), Quaternion.identity, boxes);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        else
        {
            GoToScene("MainMenu");
        }
    }


    public void CheckSuccess(int touchCoount)
    {
        if (touchCoount == targetGoals && !endGameCheckPassed)
        {
            StartCoroutine(EndGameCoroutine());
        }
    }

    private IEnumerator EndGameCoroutine()
    {
        endGameCheckPassed = true;
        currentLevel += 1;
        SaveManager.SaveLevel(currentLevel);
        yield return new WaitForSeconds(1f);
        Destroy(level.gameObject);
        SetupAsync();
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
