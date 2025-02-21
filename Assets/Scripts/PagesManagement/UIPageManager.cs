using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIPageManager : MonoBehaviour
{
    public static UIPageManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField]
    private List<UIPage> pages;
    private readonly List<int> pagesStack = new() { 0 };

    public void GoToPage(int pageIndex)
    {
        if(pageIndex > 0)
        {
            if(pageIndex > pages.Count)
            {
                Debug.LogWarning("Warning: Trying to access an undefined page index. Aborting operation.");
            }
            ShowPage(pageIndex);
            pagesStack.Add(pageIndex);
        }
        else
        {
            for(int i = 0; i < -pageIndex; i++)
            {
                if(pagesStack.Count > 1)
                {
                    pagesStack.RemoveAt(pagesStack.Count - 1);
                    int index = pagesStack.Last();
                    ShowPage(index);
                }
            }
        }

    }

    private void ShowPage(int pageIndex)
    {
        pages.ForEach((p) => { p.SetActive(pages.IndexOf(p) == pageIndex); });
    }

    public void Back()
    {
        GoToPage(-1);
    }

    public void GoToPageByName(string pageName)
    {
        UIPage page = pages.Find(p => p.name == pageName);
        GoToPage(pages.IndexOf(page));
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Back();
        }
    }
}
