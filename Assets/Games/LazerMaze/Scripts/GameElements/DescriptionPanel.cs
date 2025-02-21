using System.Collections;
using TMPro;
using UnityEngine;

public class DescriptionPanel : MonoBehaviour
{
    private TMP_Text m_Text;

    public string text
    {
        get
        {
            return m_Text.text;
        }

        set
        {
            m_Text.text = value;
        }
    }

    private void Awake()
    {
        m_Text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
