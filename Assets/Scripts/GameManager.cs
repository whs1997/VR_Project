using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Text playerScoreText;
    [SerializeField] Text enemyScoreText;
    [SerializeField] Text youWin;
    [SerializeField] Text youLose;

    [SerializeField] GameObject racket;
    [SerializeField] GameObject ball;

    private int playerScore = 0;
    private int enemyScore = 0;

    private int winScore = 11;

    private void Start()
    {
        startButton.gameObject.SetActive(true);
        UpdateScoreUI();
    }

    public void AddPlayerScore() // 내 점수 추가
    {
        playerScore++;
        UpdateScoreUI();
    }

    public void AddEnemyScore() // 상대 점수 추가
    {
        enemyScore++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() // 점수판에 스코어 갱신
    {
        playerScoreText.text = $"{playerScore}";
        enemyScoreText.text = $"{enemyScore}";

        CheckScore(); // 11점이 넘으면 승리
    }

    private void CheckScore()
    {
        if (playerScore >= winScore - 1 && enemyScore >= winScore - 1) // 둘다 10점이면 듀스
        {
            if (playerScore >= enemyScore + 2) // 2점을 먼저 이긴쪽이 승리
            {
                YouWin();
            }
            else if(enemyScore >= playerScore + 2)
            {
                YouLose();
            }
        }
        else
        {
            if(playerScore >= winScore) // 11점을 먼저 넘으면 승리
            {
                YouWin();
            }
            else if(enemyScore >= winScore)
            {
                YouLose();
            }
        }        
    }

    private void YouWin()
    {
        youWin.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        racket.gameObject.SetActive(false);
        ball.gameObject.SetActive(false);
    }

    private void YouLose()
    {
        youLose.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        racket.gameObject.SetActive(false);
        ball.gameObject.SetActive(false);
    }

    public void StartGame() // 버튼을 XR Interactor로 활성화하면 게임 시작
    {
        playerScore = 0; 
        enemyScore = 0; // 점수 0으로 초기화
        UpdateScoreUI();

        youWin.gameObject.SetActive(false);
        youLose.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        racket.gameObject.SetActive(true);
        ball.gameObject.SetActive(true);
        racket.GetComponent<Racket>().ResetRacket(); // 라켓 원위치
    }
}
