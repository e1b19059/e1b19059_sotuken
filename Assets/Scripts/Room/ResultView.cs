using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
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
        if (!TurnManager.instance.GetShowingResults())
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
        int MyScore = ScoreBoard.instance.GetMyScore();
        int RivalScore = ScoreBoard.instance.GetRivalScore();
        MyBuilder.Clear();
        MyBuilder.AppendLine($"team {ScoreBoard.instance.GetMyTeam()}");
        MyBuilder.AppendLine($"{ScoreBoard.instance.GetMyTeamMember()}\n");
        MyBuilder.AppendLine($"Score: {MyScore}");
        MyResult.text = MyBuilder.ToString();
        RivalBuilder.Clear();
        RivalBuilder.AppendLine($"team {ScoreBoard.instance.GetRivalTeam()}");
        RivalBuilder.AppendLine($"{ScoreBoard.instance.GetRivalTeamMember()}\n");
        RivalBuilder.AppendLine($"Score: {RivalScore}");
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

}
