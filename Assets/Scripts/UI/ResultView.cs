using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Runtime.InteropServices;

public class ResultView : MonoBehaviour
{
    [SerializeField] PhotonLogin photonLogin;
    [SerializeField] GameManager gameManager;
    [SerializeField] ScoreBoard scoreBoard;

    [SerializeField] TextMeshProUGUI WinORLose;
    [SerializeField] TextMeshProUGUI MyResult;
    [SerializeField] TextMeshProUGUI RivalResult;

    StringBuilder ATeamBuilder;
    StringBuilder BTeamBuilder;

    [DllImport("__Internal")]
    private static extern void switchEditable();

    private void Start()
    {
        ATeamBuilder = new StringBuilder();
        BTeamBuilder = new StringBuilder();
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

    // ゲーム終了時に呼び出される
    public void SetResult()
    {
        var players = PhotonNetwork.PlayerList;
        int coinA = PlayerPrefs.GetInt("CoinA");
        int coinB = PlayerPrefs.GetInt("CoinB");
        int damageA = PlayerPrefs.GetInt("DamageA");
        int damageB = PlayerPrefs.GetInt("DamageB");
        int scoreA = PlayerPrefs.GetInt("ScoreA");
        int scoreB = PlayerPrefs.GetInt("ScoreB");
        ATeamBuilder.Clear();
        BTeamBuilder.Clear();
        ATeamBuilder.AppendLine("チーム<color=#0000FF>A</color>");
        BTeamBuilder.AppendLine("チーム<color=#FF0000>B</color>");
        foreach (var player in players)
        {
            if (player.GetTeam() == "A")
            {
                if (player.IsLocal)
                {
                    ATeamBuilder.AppendLine("<color=#646464>   " + player.NickName + "</color>");
                }
                else
                {
                    ATeamBuilder.AppendLine("   " + player.NickName);
                }
            }
            else
            {
                BTeamBuilder.AppendLine("   " + player.NickName);
            }
        }
        ATeamBuilder.AppendLine("\nコイン獲得数 : " + coinA);
        BTeamBuilder.AppendLine("\nコイン獲得数 : " + coinB);
        ATeamBuilder.AppendLine("爆発に当たった回数 : " + damageA);
        BTeamBuilder.AppendLine("爆発に当たった回数 : " + damageB);
        ATeamBuilder.AppendLine("\nスコア : " + scoreA);
        BTeamBuilder.AppendLine("\nスコア : " + scoreB);
        if (scoreBoard.GetMyTeam() == "A")
        {
            MyResult.text = ATeamBuilder.ToString();
            RivalResult.text = BTeamBuilder.ToString();
            if (scoreA > scoreB)
            {
                WinORLose.text = "Your Team Win";
            }
            else if (scoreA == scoreB)
            {
                WinORLose.text = "Draw";
            }
            else
            {
                WinORLose.text = "Your Team Lose";
            }
        }
        else
        {
            MyResult.text = BTeamBuilder.ToString();
            RivalResult.text = ATeamBuilder.ToString();
            if (scoreA < scoreB)
            {
                WinORLose.text = "Your Team Win";
            }
            else if (scoreA == scoreB)
            {
                WinORLose.text = "Draw";
            }
            else
            {
                WinORLose.text = "Your Team Lose";
            }
        }
    }

    public void Leave()
    {
        gameManager.HideResult();
        photonLogin.Leave();
        switchEditable();
    }

}
