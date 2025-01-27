using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // If you're using TextMeshPro for better text rendering (recommended)

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [SerializeField] private GameObject gameComplete;
    [SerializeField] private GameObject correctAns;
    [SerializeField] private GameObject levelU;
    [SerializeField] private QuizDataScriptable questionDataScriptable;
    [SerializeField] private WordData[] answerWordList;
    [SerializeField] private WordData[] optionsWordList;

    [SerializeField] private TextMeshProUGUI scoreText; // UI Text element for score (TextMeshPro)
    [SerializeField] private TextMeshProUGUI levelText; // UI Text element for level (TextMeshPro)

    private GameStatus gameStatus = GameStatus.Playing;
    private char[] wordsArray = new char[12];
    private List<int> selectedWordsIndex;
    private List<int> usedQuestions; // To track used questions
    private int currentAnswerIndex = 0;
    private string answerWord;

    private int score = 0; // Player's score
    private int level = 1; // Player's level

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        selectedWordsIndex = new List<int>();
        usedQuestions = new List<int>(); // Initialize the used questions list
        UpdateUI(); // Display initial score and level
        SetQuestion();
    }

    void SetQuestion()
    {
        if (usedQuestions.Count == questionDataScriptable.questions.Count)
        {
            Debug.Log("All questions have been used. Game complete!");
            gameComplete.SetActive(true); // Show the game complete UI
            return;
        }

        gameStatus = GameStatus.Playing;

        // Select a random word that hasn't been used yet
        int questionIndex;
        do
        {
            questionIndex = UnityEngine.Random.Range(0, questionDataScriptable.questions.Count);
        } while (usedQuestions.Contains(questionIndex));

        usedQuestions.Add(questionIndex); // Add to used questions
        answerWord = questionDataScriptable.questions[questionIndex].answer;

        ResetQuestion();

        selectedWordsIndex.Clear();
        System.Array.Clear(wordsArray, 0, wordsArray.Length);

        // Shuffle the letters of the answer
        char[] shuffledLetters = ShuffleList.ShuffleListItems(answerWord.ToCharArray().ToList()).ToArray();

        // Assign shuffled letters to options
        for (int i = 0; i < optionsWordList.Length; i++)
        {
            if (i < shuffledLetters.Length)
            {
                optionsWordList[i].SetWord(shuffledLetters[i]);
                optionsWordList[i].gameObject.SetActive(true);
            }
            else
            {
                optionsWordList[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetQuestion()
    {
        SoundEffectPlayer.instance.PlayButtonClickSound();
        for (int i = 0; i < answerWordList.Length; i++)
        {
            if (i < answerWord.Length)
            {
                answerWordList[i].gameObject.SetActive(true);
                answerWordList[i].SetWord(' ');
            }
            else
            {
                answerWordList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < optionsWordList.Length; i++)
        {
            optionsWordList[i].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    public void SelectedOption(WordData value)
    {
        SoundEffectPlayer.instance.PlayButtonClickSound();
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        selectedWordsIndex.Add(value.transform.GetSiblingIndex());
        value.gameObject.SetActive(false);
        answerWordList[currentAnswerIndex].SetWord(value.wordValue);

        currentAnswerIndex++;

        if (currentAnswerIndex == answerWord.Length)
        {
            bool correctAnswer = true;

            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordList[i].wordValue))
                {
                    correctAnswer = false;
                    break;
                }
            }

            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
                SoundEffectPlayer.instance.PlayCorrectSound();
                correctAns.SetActive(true);
                AddScore(3); // Add 3 points for a correct answer
                gameStatus = GameStatus.Next;
                Invoke("SetQuestion", 1f); // Set next question after 0.5 seconds
                Invoke("HideCorrectAnswer", 1f);
            }
            else
            {
                Debug.Log("Incorrect Answer");
                SoundEffectPlayer.instance.PlayWrongMatchSound();
                ResetQuestion();
            }
        }
    }
    private void HideCorrectAnswer()
{
    correctAns.SetActive(false); // Hide the correct answer feedback
    // Play correct answer sound
    //SoundEffectPlayer.instance.PlayCorrectSound();
}
private void HidelevelU()
{
    levelU.SetActive(false); // Hide the correct answer feedback
}


    public void ResetLastWord()
    {
        SoundEffectPlayer.instance.PlayButtonClickSound();
        if (selectedWordsIndex.Count > 0)
        {
            int index = selectedWordsIndex[selectedWordsIndex.Count - 1];
            optionsWordList[index].gameObject.SetActive(true);
            selectedWordsIndex.RemoveAt(selectedWordsIndex.Count - 1);

            currentAnswerIndex--;
            answerWordList[currentAnswerIndex].SetWord(' ');
        }
    }
   /* public void ResetClickedLetter(int answerIndex)
{   Debug.LogWarning("Invalid answerIndexxx: " );
    // Ensure the index is valid and within range
    if (answerIndex < 0 || answerIndex >= currentAnswerIndex)
    {
        Debug.LogWarning("Invalid answerIndex: " + answerIndex);
        return;
    }

    // Get the letter to reset from the answer list
    char letterToReset = answerWordList[answerIndex].wordValue;

    // If the position is already empty, do nothing
    if (letterToReset == '_')
    {
        Debug.Log("No letter to reset at index: " + answerIndex);
        return;
    }

    // Clear the current slot
    answerWordList[answerIndex].SetWord('_');

    // Re-enable the corresponding letter in the options list
    for (int i = 0; i < optionsWordList.Length; i++)
    {
        if (optionsWordList[i].wordValue == letterToReset && !optionsWordList[i].gameObject.activeSelf)
        {
            optionsWordList[i].gameObject.SetActive(true); // Make the option visible again
            break;
        }
    }

    // Shift all subsequent letters one step to the left
    for (int i = answerIndex; i < currentAnswerIndex - 1; i++)
    {
        answerWordList[i].SetWord(answerWordList[i + 1].wordValue);
        answerWordList[i + 1].SetWord('_');
    }

    // Update the tracking variables
    currentAnswerIndex--; // Decrease the answer index
    selectedWordsIndex.RemoveAt(answerIndex); // Remove the entry from the selected index list
}*/
public void ResetClickedLetter(int answerIndex)
{
    SoundEffectPlayer.instance.PlayButtonClickSound();
    // Check if the passed parameter is 3
    if (answerIndex == 3)
    {
        Debug.Log("Parameter is 3. Perform specific action.");
        // Add the logic you want to execute when answerIndex is 3
    }
    if (answerIndex == 2)
    {
        Debug.Log("Parameter is 2. Perform specific action.");
        // Add the logic you want to execute when answerIndex is 3
    }
    if (answerIndex == 1)
    {
        Debug.Log("Parameter is 1. Perform specific action.");
        // Add the logic you want to execute when answerIndex is 3
    }
    else
    {
        Debug.Log("Parameter is 0. Perform default action.");
        // Add the default logic here
    }
}


    private void AddScore(int points)
    {
        score += points;
        int requiredScore = CalculateRequiredScore(level);

        // Level up every 6 points
        if (score >= requiredScore)
        {
            level++;
            Debug.Log("Level Up!");
            levelU.SetActive(true);
            // Play level-up sound
            SoundEffectPlayer.instance.PlayLevelUpSound();
            requiredScore = CalculateRequiredScore(level);
        }

        UpdateUI();
    }
    private int CalculateRequiredScore(int currentLevel)
{
    // Calculate the total number of correct answers needed for the current level
    int baseScore = 0;

    if (currentLevel <= 1) baseScore = 3; // Level 1: 1 correct answer
    else if (currentLevel == 2) baseScore = 9; // Level 2: 3 total answers (1 + 2)
    else if (currentLevel == 3) baseScore = 18; // Level 3: 7 total answers (1 + 2 + 4)
    else if (currentLevel <= 5) baseScore = 36; // Level 4-5: 15 total answers (1 + 2 + 4 + 8)
    else if (currentLevel <= 10) baseScore = 48; // Levels 6-10: 31 total answers (1 + 2 + 4 + 8 + 16)
    else if (currentLevel <= 15) baseScore = 96; // Levels 11-15: 63 total answers (1 + 2 + 4 + ... + 32)
    else if (currentLevel <= 20) baseScore = 192; // Levels 16-20: 127 total answers
    else if (currentLevel <= 25) baseScore = 384; // Levels 21-25: 255 total answers
    else if (currentLevel <= 50) baseScore = 768; // Levels 26-30: 511 total answers
    else baseScore = int.MaxValue; // Beyond level 30, no further progression

    return baseScore;
}

    private void UpdateUI()
    {
        // Update the score and level UI
        if (scoreText != null)
            scoreText.text = " " + score;

        if (levelText != null)
            levelText.text = " " + level;
    }
}

public enum GameStatus
{
    Playing,
    Next
}