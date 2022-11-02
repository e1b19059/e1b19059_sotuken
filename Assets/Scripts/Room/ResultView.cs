using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
    [SerializeField] private PhotonLogin photonLogin;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreBoard scoreBoard;

    [SerializeField] private TextMeshProUGUI WinORLose;
    [SerializeField] private TextMeshProUGUI MyResult;
    [SerializeField] private TextMeshProUGUI RivalResult;

    private StringBuilder MyBuilder;
    private StringBuilder RivalBuilder;

    private void Start()
    {
        MyBuilder = new StringBuilder();
        RivalBuilder = new StringBuilder();
    }

    private void Update()
    {
        if (!gameManager.GetShowingResults())
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    // ゲーム終了時に表示される結果表示ボタンで呼び出される
    public void SetResult()
    {
        int MyScore = scoreBoard.GetMyScore();
        int RivalScore = scoreBoard.GetRivalScore();
        MyBuilder.Clear();
        MyBuilder.AppendLine($"Team {scoreBoard.GetMyTeam()}");
        MyBuilder.AppendLine($"{scoreBoard.GetMyTeamMember()}\n");
        MyBuilder.AppendLine($"Total Score: {MyScore}");
        MyResult.text = MyBuilder.ToString();
        RivalBuilder.Clear();
        RivalBuilder.AppendLine($"Team {scoreBoard.GetRivalTeam()}");
        RivalBuilder.AppendLine($"{scoreBoard.GetRivalTeamMember()}\n");
        RivalBuilder.AppendLine($"Total Score: {RivalScore}");
        RivalResult.text = RivalBuilder.ToString();
        if (MyScore > RivalScore)
        {
            WinORLose.text = "Your Team Win";
        }
        else if (MyScore == RivalScore)
        {
            WinORLose.text = "Draw";
        }
        else
        {
            WinORLose.text = "Your Team Lose";
        }
    }

    public void Leave()
    {
        gameManager.HideResult();
        photonLogin.Leave();
    }

}
