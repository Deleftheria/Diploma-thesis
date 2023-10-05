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
                    finishMessage.text = "Μην αφήσεις τον μάγο Άρη να καταστρέψει το σχολείο του Νικόλα";
                    finalWords.text = "Βοήθησε τον Νικόλα να βρει όλες τις κρυμμένες ενότητες του σχολικού βιβλίου της γλώσσας και να απαντήσει σωστά στα κουίζ. Μόνο με αυτό τον τρόπο θα μπορέσει ο Νικόλας να σπάσει το ξόρκι του κακού μάγου Άρη και να σώσει το βιβλίο και το σχολείο του. Ξεκίνα το παιχνίδι τώρα! ";
                }
                else
                {
                    int i;
                    for (i = 0; i < 16; i++)
                    {
                        if (levels[i] != 'T')
                        {
                            finishMessage.fontSize = 85;
                            finishMessage.text = "Η προσπάθεια του Νικολά για να σώσει το βιβλίο της γλώσσας συνεχίζεται…";
                            finalWords.text = "Μέχρι στιγμής ο Νικόλα έχει καταφέρει να πάρει πίσω κάποιες από τις ενότητες, ωστόσο πρέπει να συνεχίσει την προσπάθεια για να κατορθώσει να βρει όλες τις ενότητες του σχολικού βιβλίου και να σπάσει το ξόρκι του κακού μάγου Άρη, προκειμένου να σώσει το σχολείο του. Γύρνα στο παιχνίδι για να βοηθήσεις τον Νικόλα να πάρει πίσω και την ενότητα " + (i + 1);
                            break;
                        }
                    }

                    if (i == 16)
                    {
                        continueButton.SetActive(false);
                        finishMessage.fontSize = 150;
                        finishMessage.text = "Συγχαρητήρια!!!";
                        finalWords.text = "Χάρη στην βοήθεια σου ο Νικόλας κατάφερε να σπάσει το ξόρκι του κακού μάγου Άρη και να πάρει πίσω όλες τις χαμένες ενότητες του σχολικού βιβλίου. Από σήμερα, ο Νικόλας και οι συμμαθητές του θα μπορούν και πάλι να πηγαίνουν στο σχολείο τους και να μαθαίνουν νέα πράγματα.";
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
