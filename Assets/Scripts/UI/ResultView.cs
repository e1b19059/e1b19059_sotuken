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

    // �Q�[���I�����ɌĂяo�����
    public void SetResult()
    {
        var players = PhotonNetwork.PlayerList;
        int scoreA = PlayerPrefs.GetInt("ScoreA");
        int scoreB = PlayerPrefs.GetInt("ScoreB");
        ATeamBuilder.Clear();
        BTeamBuilder.Clear();
        ATeamBuilder.AppendLine("�`�[��<color=#0000FF>A</color>\n�����o�[");
        BTeamBuilder.AppendLine("�`�[��<color=#FF0000>B</color>\n�����o�[");
        int Anum = 0, Bnum = 0;
        foreach (var player in players)
        {
            if (player.GetTeam() == "A")
            {
                Anum++;
                if (player.IsLocal)
                {
                    ATeamBuilder.AppendLine("   �E<color=#FFFFFF>" + player.NickName + "</color>");
                }
                else
                {
                    ATeamBuilder.AppendLine("   �E" + player.NickName);
                }
            }
            else
            {
                Bnum++;
                BTeamBuilder.AppendLine("   �E" + player.NickName);
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
        ATeamBuilder.AppendLine("�R�C���l���� : " + PlayerPrefs.GetInt("CoinA"));
        BTeamBuilder.AppendLine("�R�C���l���� : " + PlayerPrefs.GetInt("CoinB"));
        ATeamBuilder.AppendLine("�����ɓ��������� : " + PlayerPrefs.GetInt("DamageA"));
        BTeamBuilder.AppendLine("�����ɓ��������� : " + PlayerPrefs.GetInt("DamageB"));
        ATeamBuilder.AppendLine("�����ȃv���O���� : " + PlayerPrefs.GetInt("MissA"));
        BTeamBuilder.AppendLine("�����ȃv���O���� : " + PlayerPrefs.GetInt("MissB"));
        ATeamBuilder.AppendLine("�^�C�� : " + PlayerPrefs.GetFloat("TimeA").ToString("f2"));
        BTeamBuilder.AppendLine("�^�C�� : " + PlayerPrefs.GetFloat("TimeB").ToString("f2"));
        ATeamBuilder.AppendLine("\n�X�R�A : " + scoreA);
        BTeamBuilder.AppendLine("\n�X�R�A : " + scoreB);
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
