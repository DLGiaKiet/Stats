using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public Text questionText;

    public Text scoreText;

    public Text FinalScore;

    public Button[] replyButtons;

    public QtsData qtsData;

    public GameObject Right;

    public GameObject Wrong;

    public GameObject GameFinished;

    private int currentQuestion = 0;

    private static int score = 0;

    private PlayerStats playerStats;

    private LevelUpUI levelUpUI;

    void Start()
    {
        SetQuestion(currentQuestion);
        Right.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        GameFinished.gameObject.SetActive(false);

        GameObject tempChar = GameObject.FindGameObjectWithTag("Player");
        playerStats = tempChar.GetComponent<PlayerStats>();

        levelUpUI = FindObjectOfType<LevelUpUI>();
    }

    void SetQuestion(int questionIndex)
    {
        questionText.text = qtsData.questions[questionIndex].questionText;

        //Remove pre listeners before adding a new ones
        foreach (Button r in replyButtons)
        {
            r.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < replyButtons.Length; i++)
        {
            replyButtons[i].GetComponentInChildren<Text>().text = qtsData.questions[questionIndex].replies[i];
            int replyIndex = i;
            replyButtons[i].onClick.AddListener(() =>
            {
                CheckReply(replyIndex);
            });
        }
    }

    void CheckReply(int replyIndex)
    {
        if(replyIndex == qtsData.questions[currentQuestion].correctReplyIndex)
        {
            score++;
            scoreText.text = "" + score;

            Right.gameObject.SetActive(true);

            foreach (Button r in replyButtons)
            {
                r.interactable = false;
            }

            //Next question
            StartCoroutine(Next());
        }
        else
        {
            Wrong.gameObject.SetActive(true);

            foreach (Button r in replyButtons)
            {
                r.interactable = false;
            }

            //Next question
            StartCoroutine(Next());
        }
    }

    IEnumerator Next()
    {
        yield return new WaitForSeconds(2);

        currentQuestion++;

        if(currentQuestion < qtsData.questions.Length)
        {
            //Reset the UI and enable all reply buttons
            Reset();
        }
        else
        {
            GameFinished.SetActive(true);

            float scorePercentage = (float)score / qtsData.questions.Length * 100;

            FinalScore.text = "You scored " + scorePercentage.ToString("F0") + "%";

            if(scorePercentage < 50)
            {
                FinalScore.text += "\nNo gift for you";
            }
            else
            {
                FinalScore.text += "\nYou're a genius!\nYou'll get bonus souls";
                playerStats.currentSouls = playerStats.currentSouls + 500;
                levelUpUI.UpdateUI();
            }
        }
    }

    public void Reset()
    {
        Right.SetActive(false);
        Wrong.SetActive(false);

        foreach(Button r in replyButtons)
        {
            r.interactable = true;
        }

        SetQuestion(currentQuestion);
    }
}
