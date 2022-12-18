using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class ResultView : MonoBehaviour
{
    [SerializeField] PhotonLogin photonLogin;
    [SerializeField] GameManager gameManager;
    [SerializeField] ScoreBoard scoreBoard;

    [SerializeField] TextMeshProUGUI WinORLose;
    [SerializeField] TextMeshProUGUI MyResult;
    [SerializeField] TextMeshProUGUI RivalResult;

    StringBuilder MyBuilder;
    StringBuilder RivalBuilder;

    [DllImport("__Internal")]
    private static extern void switchEditable();

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

    // �Q�[���I�����ɕ\������錋�ʕ\���{�^���ŌĂяo�����
    public void SetResult()
    {
        int MyScore = scoreBoard.GetMyScore();
        int RivalScore = scoreBoard.GetRivalScore();
        MyBuilder.Clear();
        MyBuilder.AppendLine($"�`�[�� {scoreBoard.GetMyTeam()}");
        MyBuilder.AppendLine($"{scoreBoard.GetMyTeamMember()}\n");
        MyBuilder.AppendLine($"Total Score: {MyScore}");
        MyResult.text = MyBuilder.ToString();
        RivalBuilder.Clear();
        RivalBuilder.AppendLine($"�`�[�� {scoreBoard.GetRivalTeam()}");
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
        switchEditable();
    }

}
