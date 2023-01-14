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
        int scoreA = PlayerPrefs.GetInt("ScoreA");
        int scoreB = PlayerPrefs.GetInt("ScoreB");
        ATeamBuilder.Clear();
        BTeamBuilder.Clear();
        ATeamBuilder.AppendLine("チーム<color=#0000FF>A</color>\nメンバー");
        BTeamBuilder.AppendLine("チーム<color=#FF0000>B</color>\nメンバー");
        int Anum = 0, Bnum = 0;
        foreach (var player in players)
        {
            if (player.GetTeam() == "A")
            {
                Anum++;
                if (player.IsLocal)
                {
                    ATeamBuilder.AppendLine("   ・<color=#FFFFFF>" + player.NickName + "</color>");
                }
                else
                {
                    ATeamBuilder.AppendLine("   ・" + player.NickName);
                }
            }
            else
            {
                Bnum++;
                BTeamBuilder.AppendLine("   ・" + player.NickName);
            }
        }
        if(Anum == 1)
        {
            ATeamBuilder.AppendLine("");
        }
        if (Bnum == 1)
        {
            BTeamBuilder.AppendLine("");
        }
        ATeamBuilder.AppendLine("コイン獲得数 : " + PlayerPrefs.GetInt("CoinA"));
        BTeamBuilder.AppendLine("コイン獲得数 : " + PlayerPrefs.GetInt("CoinB"));
        ATeamBuilder.AppendLine("爆発に当たった回数 : " + PlayerPrefs.GetInt("DamageA"));
        BTeamBuilder.AppendLine("爆発に当たった回数 : " + PlayerPrefs.GetInt("DamageB"));
        ATeamBuilder.AppendLine("無効なプログラム : " + PlayerPrefs.GetInt("MissA"));
        BTeamBuilder.AppendLine("無効なプログラム : " + PlayerPrefs.GetInt("MissB"));
        ATeamBuilder.AppendLine("タイム : " + PlayerPrefs.GetFloat("TimeA").ToString("f2"));
        BTeamBuilder.AppendLine("タイム : " + PlayerPrefs.GetFloat("TimeB").ToString("f2"));
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
