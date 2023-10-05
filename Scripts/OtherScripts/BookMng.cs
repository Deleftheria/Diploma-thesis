using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookMng : MonoBehaviour
{
    [SerializeField] private GameObject BG_elements;
    [SerializeField] private GameObject textElement;

    // Start is called before the first frame update
    void Start()
    {
        BG_elements.SetActive(false);
        textElement.SetActive(false);
    }

    //activates the canvas to display the information contained in the books near each cube
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textElement.SetActive(true);
            BG_elements.SetActive(true);
        }
    }

    public void CloseWindow()
    {
        BG_elements.SetActive(false);
        textElement.SetActive(false);
    }
}
