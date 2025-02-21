using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    [SerializeField]
    private UIPageManager pageManager;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void ShowInfos()
    {
        pageManager.GoToPageByName("Infos");
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
