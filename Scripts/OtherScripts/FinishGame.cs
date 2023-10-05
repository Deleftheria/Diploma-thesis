using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishGame : MonoBehaviour
{
    public GameObject finishCanvas;
    public TextMeshProUGUI finishMessage;
    public TextMeshProUGUI finalWords;
    public GameObject infoText;
    public GameObject continueButton;

    void Start()
    {
        finishCanvas.SetActive(false);
        infoText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                string levels = PlayerPrefs.GetString(GameUtility.SavePrefValue);
                if (levels == "")
                {
                    finishMessage.fontSize = 85;
                    finishMessage.text = "��� ������� ��� ���� ��� �� ����������� �� ������� ��� ������";
                    finalWords.text = "������� ��� ������ �� ���� ���� ��� ��������� �������� ��� �������� ������� ��� ������� ��� �� ��������� ����� ��� �����. ���� �� ���� ��� ����� �� �������� � ������� �� ������ �� ����� ��� ����� ����� ��� ��� �� ����� �� ������ ��� �� ������� ���. ������ �� �������� ����! ";
                }
                else
                {
                    int i;
                    for (i = 0; i < 16; i++)
                    {
                        if (levels[i] != 'T')
                        {
                            finishMessage.fontSize = 85;
                            finishMessage.text = "� ���������� ��� ������ ��� �� ����� �� ������ ��� ������� �����������";
                            finalWords.text = "����� ������� � ������ ���� ��������� �� ����� ���� ������� ��� ��� ��������, ������ ������ �� ��������� ��� ���������� ��� �� ���������� �� ���� ���� ��� �������� ��� �������� ������� ��� �� ������ �� ����� ��� ����� ����� ���, ����������� �� ����� �� ������� ���. ����� ��� �������� ��� �� ��������� ��� ������ �� ����� ���� ��� ��� ������� " + (i + 1);
                            break;
                        }
                    }

                    if (i == 16)
                    {
                        continueButton.SetActive(false);
                        finishMessage.fontSize = 150;
                        finishMessage.text = "������������!!!";
                        finalWords.text = "���� ���� ������� ��� � ������� �������� �� ������ �� ����� ��� ����� ����� ��� ��� �� ����� ���� ���� ��� ������� �������� ��� �������� �������. ��� ������, � ������� ��� �� ���������� ��� �� ������� ��� ���� �� ��������� ��� ������� ���� ��� �� ��������� ��� ��������.";
                    }
                }

                finishCanvas.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            infoText.SetActive(false);
        }

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ContinueGame()
    {
        finishCanvas.SetActive(false);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quiting game");
        Application.Quit();
    }
}
