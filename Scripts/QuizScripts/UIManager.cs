using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable()]
public struct UIMngParameters
{
    [Header("Answers Options")]
    public float margins;

    [Header("Resolution Screen Options")]
    public Color correctBGColor;
    public Color incorrectBGColor;
    public Color finalBGColor;
}

[Serializable()]
public struct UIElements
{
    public RectTransform answersContentArea;
    public TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI scoreText;

    [Space]
    public Animator resolutionScreenAnimator;
    public Image resolutionBG;
    public TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI resolutionScoreText;

    [Space]
    public TextMeshProUGUI messageText;
    public CanvasGroup mainCanvasGroup;
    public RectTransform finishUIElementsWin;
    public RectTransform finishUIElementsLose;

    [Space]
    public GameObject happyFace;
    public GameObject sadFace;
}

public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { correct, incorrect, finish}

    [Header("References")]
    [SerializeField] GameEvents events;

    [Header("UI Elements (Prefabs)")]
    [SerializeField] AnswerData answerPrefab;

    [SerializeField] UIElements uiElements;

    [Space]
    [SerializeField] UIMngParameters parameters;

    List<AnswerData> currAnswers = new List<AnswerData>();

    private int resStateParaHash = 0;

    private IEnumerator IE_DisplayTimeResolution;

    void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;
    }

    void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
    }

    void UpdateQuestionUI(Question question)
    {
        uiElements.questionInfoTextObject.text = question.Info;
        CreateAnswers(question);
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);
        uiElements.resolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uiElements.mainCanvasGroup.blocksRaycasts = false;

        if (type != ResolutionScreenType.finish)
        {
            if (IE_DisplayTimeResolution != null)
            {
                StopCoroutine(IE_DisplayTimeResolution);
            }
            IE_DisplayTimeResolution = DisplayTimeResolution();
            StartCoroutine(IE_DisplayTimeResolution);
        }
    }

    IEnumerator DisplayTimeResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uiElements.resolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uiElements.mainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResUI(ResolutionScreenType type, int score)
    {
        switch (type)
        {
            case ResolutionScreenType.correct:
                uiElements.happyFace.SetActive(true);
                uiElements.sadFace.SetActive(false);

                uiElements.resolutionBG.color = parameters.correctBGColor;
                uiElements.resolutionStateInfoText.text = "ΣΩΣΤΟ!";
                uiElements.resolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.incorrect:
                uiElements.sadFace.SetActive(true);
                uiElements.happyFace.SetActive(false);

                uiElements.resolutionBG.color = parameters.incorrectBGColor;
                uiElements.resolutionStateInfoText.text = "ΛΑΘΟΣ!";
                uiElements.resolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.finish:
                uiElements.happyFace.SetActive(false);
                uiElements.sadFace.SetActive(false);

                uiElements.resolutionBG.color = parameters.finalBGColor;
                uiElements.resolutionStateInfoText.text = "ΤΕΛΙΚΗ ΒΑΘΜΟΛΟΓΙΑ";

                StartCoroutine(CalculateScore());
               
                if (events.currFinalScore > 200)
                {
                    uiElements.finishUIElementsWin.gameObject.SetActive(true);
                }
                else
                {
                    uiElements.finishUIElementsLose.gameObject.SetActive(true);

                    if (events.currFinalScore < -200)
                    {
                        uiElements.messageText.text = "Δυστυχώς το σκορ που κατάφερες να συγκεντρώσεις δεν είναι αρκετό για να μπορέσεις να πάρεις πίσω την χαμένη σχολική ενότητα. Πρέπει να μελετήσεις καλύτερα την ενότητα για να μπορέσεις να απαντήσεις σωστά σε περισσότερες ερωτήσεις και να σώσεις την ενότητα απο τον Άρη!";
                    }
                    else
                    {
                        uiElements.messageText.text = "Δυστυχώς το σκορ που κατάφερες να συγκεντρώσεις δεν είναι αρκετό για να μπορέσεις να πάρεις πίσω την χαμένη σχολική ενότητα. Πρόσεχε τις λεπτομέριες της γραμματικής και του συνταντικού, ώστε να βοηθήσεις τον Νικόλα να πάρει πίσω την ενότητα!";
                    }
                }

                break;
        }
    }

    IEnumerator CalculateScore()
    {
        if (events.currFinalScore == 0)
        {
            uiElements.resolutionScoreText.text = 0.ToString();
            yield break;
        }

        var scoreValue = 0;
        var scoreMoreThanZero = events.currFinalScore > 0; 
        while ((scoreMoreThanZero) ? scoreValue < events.currFinalScore : scoreValue > events.currFinalScore)
        {
            scoreValue += scoreMoreThanZero ? 1 : -1;
            uiElements.resolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    void CreateAnswers(Question question)
    {
        //erase current answers
        EraseAnswers();

        //create new answers
        float offset = 0 - parameters.margins;
        for (int i = 0; i < question.answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uiElements.answersContentArea);
            newAnswer.UpdateData(question.answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.margins);
            uiElements.answersContentArea.sizeDelta = new Vector2(uiElements.answersContentArea.sizeDelta.x, offset * -1);

            currAnswers.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currAnswers)
        {
            Destroy(answer.gameObject);
        }

        currAnswers.Clear();
    }

    void UpdateScoreUI()
    {
        uiElements.scoreText.text = "ΣΚΟΡ: " + events.currFinalScore;
    }
}
