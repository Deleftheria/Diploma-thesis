using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSections : MonoBehaviour
{
    public GameObject sectionCanvas;

    void Start()
    {
        sectionCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sectionCanvas.SetActive(true);

            string levels = PlayerPrefs.GetString(GameUtility.SavePrefValue);
            char[] isFinished = new char[16];
            for (int j = 0; j < 16; j++)
            {
                isFinished[j] = levels[j];
            }

            Debug.Log("special section -> " + sectionCanvas.name); 
            var name = sectionCanvas.name;
            switch (name)
            {
                case "Section3":
                    isFinished[2] = 'T';
                    break;
                case "Section8":
                    isFinished[7] = 'T';
                    break;
                case "Section12":
                    isFinished[11] = 'T';
                    break;
                case "Section15":
                    isFinished[14] = 'T';
                    break;
            }

            levels = new string(isFinished);
            PlayerPrefs.SetString(GameUtility.SavePrefValue, levels);
        }
    }

    public void CloseWindow()
    {
        sectionCanvas.SetActive(false);
    }
}
