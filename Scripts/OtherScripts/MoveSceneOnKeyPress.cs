using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSceneOnKeyPress : MonoBehaviour
{
    [SerializeField] private string newLevel;
    [SerializeField] private GameObject uiElement;

    SaveNLoad playerPosData;

    void Start()
    {
        playerPosData = FindObjectOfType<SaveNLoad>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiElement.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                SoundManager.Instance.startingZoneBGMusic.Stop();

                Debug.Log("Save game");
                playerPosData.Save();
                SceneManager.LoadScene(newLevel);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiElement.SetActive(false);
        }
    }
}
