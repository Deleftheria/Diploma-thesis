using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class QuizManager : MonoBehaviour
{
    public TextAsset Q1;
    public TextAsset Q2;

    private Data data = new Data();

    [SerializeField] GameEvents events = null;

    private List<AnswerData> pickedAnswers = new List<AnswerData>();
    private List<int> finishedQuestions = new List<int>();
    private int currQuestion = 0;

    private IEnumerator IE_WaitTillNextRound = null;

    private bool isFinished
    {
        get
        {
            return (finishedQuestions.Count < data.questions.Length) ? false : true;
        }
    }

    void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    void Awake()
    {
        events.currFinalScore = 0;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(GameUtility.SavePrefValue))
        {
            print(PlayerPrefs.GetString(GameUtility.SavePrefValue));
        }
        else
        {
            PlayerPrefs.SetString(GameUtility.SavePrefValue, "FFFFFFFFFFFFFFFF");
            Debug.Log(PlayerPrefs.GetString(GameUtility.SavePrefValue));
        }

        LoadData();

        var seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (data.questions[currQuestion].ansType == AnswerType.Single)
        {
            foreach (var answer in pickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }

            pickedAnswers.Clear();
            pickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = pickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                pickedAnswers.Remove(newAnswer);
            }
            else
            {
                pickedAnswers.Add(newAnswer);
            }
        }
    }

    //is EraseAnswers method
    public void PickedAnswers()
    {
        pickedAnswers = new List<AnswerData>();
    }

    void Display()
    {
        PickedAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Something went wrong while trying to display new Question UI Data");
        }
    }

    public void Accept()
    {
        bool isCorrect = CheckAnswers();
        finishedQuestions.Add(currQuestion);

        int addNum = (isCorrect) ? data.questions[currQuestion].score : -data.questions[currQuestion].score;
        UpdateScore(addNum);

        if (isFinished)
        {
            if (events.currFinalScore > 200)
            {
                SetSectionNum();
            }

            Debug.Log("saveprefvalue is " + PlayerPrefs.GetString(GameUtility.SavePrefValue));
        }

        var type = (isFinished) ? UIManager.ResolutionScreenType.finish : (isCorrect) ? UIManager.ResolutionScreenType.correct : UIManager.ResolutionScreenType.incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, data.questions[currQuestion].score);
        }

        QuizAudioManager.instance.PlaySound((isCorrect) ? "CorrectSFX" : "IncorrectSFX");

        if (type != UIManager.ResolutionScreenType.finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }
    }

    IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currQuestion = randomIndex;

        return data.questions[currQuestion];
    }

    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (finishedQuestions.Count < data.questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, data.questions.Length);
            } while (finishedQuestions.Contains(random) || random == currQuestion);
        }

        return random;
    }

    bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    bool CompareAnswers()
    {
        if (pickedAnswers.Count > 0)
        {
            List<int> correctAnsList = data.questions[currQuestion].GetCorrectAnswers();
            List<int> pickedAnsList = pickedAnswers.Select(x => x.AnswerIndex).ToList();

            var first = correctAnsList.Except(pickedAnsList).ToList();
            var second = pickedAnsList.Except(correctAnsList).ToList();

            return !first.Any() && !second.Any();
        }

        return false;
    }

    //function load all data from the xml file
    void LoadData()
    {
        var name = SceneManager.GetActiveScene().name;
        switch (name)
        {
            case "Level1":
                name = "Questions_Data";
                break;
            case "Level2":
                name = "Q2";
                break;
            case "Level4":
                name = "Q4";
                break;
            case "Level5":
                name = "Q5";
                break;
            case "Level6":
                name = "Q6";
                break;
            case "Level7":
                name = "Q7";
                break;
            case "Level9":
                name = "Q9";
                break;
            case "Level10":
                name = "Q10";
                break;
            case "Level11":
                name = "Q11";
                break;
            case "Level13":
                name = "Q13";
                break;
            case "Level14":
                name = "Q14";
                break;
            case "Level16":
                name = "Q16";
                break;
            default:
                Debug.Log("There is problem in LoadData function.");
                break;
        }

        Debug.Log("name == "+name);
        var path = Path.Combine(GameUtility.FileDir, name + ".xml");
        data = Data.Fetch(path);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void SetSectionNum(){

        var name = SceneManager.GetActiveScene().name;
        string levels;
        char[] isFinished = new char[16];

        for (int i = 0; i < 16; i++)
        {
            if (name == "Level"+(i+1))
            {
                levels = PlayerPrefs.GetString(GameUtility.SavePrefValue);

                for (int j = 0; j < 16; j++)
                {
                    isFinished[j] = levels[j];
                }
                isFinished[i] = 'T';

                levels = new string(isFinished);
                PlayerPrefs.SetString(GameUtility.SavePrefValue, levels);
            }
        }
    }

    private void UpdateScore(int add)
    {
        events.currFinalScore += add;
        events.ScoreUpdated?.Invoke();
    }
}
