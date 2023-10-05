using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionsInfo : MonoBehaviour
{
    [SerializeField] GameObject BG_elements;
    [SerializeField] Button[] buttons;

    void Start()
    {
        BG_elements.SetActive(false);

        for (int i = 0; i < 16; i++)
        {
            buttons[i].interactable = false;
        }
    }

    public void OpenBook()
    {
        BG_elements.SetActive(true);
        Time.timeScale = 0f;

        string levels = PlayerPrefs.GetString(GameUtility.SavePrefValue);
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == 'T')
            {
                buttons[i].interactable = true;
            }
        }
    }

    public void CloseBook()
    {
        BG_elements.SetActive(false);

        Time.timeScale = 1f;
    }
}
