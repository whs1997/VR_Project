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

    public void AddPlayerScore() // �� ���� �߰�
    {
        playerScore++;
        UpdateScoreUI();
    }

    public void AddEnemyScore() // ��� ���� �߰�
    {
        enemyScore++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() // �����ǿ� ���ھ� ����
    {
        playerScoreText.text = $"{playerScore}";
        enemyScoreText.text = $"{enemyScore}";

        CheckScore(); // 11���� ������ �¸�
    }

    private void CheckScore()
    {
        if (playerScore >= winScore - 1 && enemyScore >= winScore - 1) // �Ѵ� 10���̸� �ེ
        {
            if (playerScore >= enemyScore + 2) // 2���� ���� �̱����� �¸�
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
            if(playerScore >= winScore) // 11���� ���� ������ �¸�
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

    public void StartGame() // ��ư�� XR Interactor�� Ȱ��ȭ�ϸ� ���� ����
    {
        playerScore = 0; 
        enemyScore = 0; // ���� 0���� �ʱ�ȭ
        UpdateScoreUI();

        youWin.gameObject.SetActive(false);
        youLose.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        racket.gameObject.SetActive(true);
        ball.gameObject.SetActive(true);
        racket.GetComponent<Racket>().ResetRacket(); // ���� ����ġ
    }
}
