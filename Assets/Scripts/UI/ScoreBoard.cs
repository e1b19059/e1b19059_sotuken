using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] PhotonLogin photonLogin;
    [SerializeField] TextMeshProUGUI MyTeamMember = default;
    [SerializeField] TextMeshProUGUI RivalTeamMember = default;
    [SerializeField] TextMeshProUGUI MyTeamLabel;
    [SerializeField] TextMeshProUGUI RivalTeamLabel;
    [SerializeField] TextMeshProUGUI MyScoreLabel;
    [SerializeField] TextMeshProUGUI RivalScoreLabel;
    [SerializeField] TextMeshProUGUI MyBombCnt;
    [SerializeField] TextMeshProUGUI RivalBombCnt;

    PlayerBehaviour MyCharacter;
    PlayerBehaviour RivalCharacter;
    StringBuilder MyBuilder;
    StringBuilder RivalBuilder;
    float elapsedTime;
    int MyScore;
    int RivalScore;

    private void Start()
    {
        MyBuilder = new StringBuilder();
        RivalBuilder = new StringBuilder();
        elapsedTime = 0f;
    }

    void Update()
    {
        // ゲーム中のみ更新する
        if (!photonLogin.GetPlayingFlag()) { return; }
        // 0.1秒毎にテキストを更新する
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.1f)
        {
            elapsedTime = 0f;
            UpdateMember();
            SetScore();
        }
    }

    private void UpdateMember()
    {
        var players = PhotonNetwork.PlayerList;
        MyBuilder.Clear();
        RivalBuilder.Clear();
        foreach (var player in players)
        {
            if (player.GetTeam() == GetMyTeam())
            {
                if (player.IsLocal)
                {
                    MyBuilder.AppendLine($"<color=#FAFAFA>{player.NickName}({player.ActorNumber})</color>");
                }
                else
                {
                    MyBuilder.AppendLine($"{player.NickName}({player.ActorNumber})");
                }
            }
            else
            {
                RivalBuilder.AppendLine($"{player.NickName}({player.ActorNumber})");
            }
        }
        MyTeamMember.text = MyBuilder.ToString();
        RivalTeamMember.text = RivalBuilder.ToString();
    }

    public void SetPlayerBehaviour()
    {
        MyCharacter = GameObject.FindGameObjectWithTag($"Player{GetMyTeam()}").GetComponent<PlayerBehaviour>();
        RivalCharacter = GameObject.FindGameObjectWithTag($"Player{GetRivalTeam()}").GetComponent<PlayerBehaviour>();
    }

    public void SetMyScore(int score)
    {
        MyScore = score;
        MyScoreLabel.text = "Score: " + score.ToString();
    }

    public void SetRivalScore(int score)
    {
        RivalScore = score;
        RivalScoreLabel.text = "Score: " + score.ToString();
    }

    public void SetMyBombCnt(int cnt)
    {
        MyBombCnt.text = "Bomb: " + cnt.ToString();
    }

    public void SetRivalBombCnt(int cnt)
    {
        RivalBombCnt.text = "Bomb: " + cnt.ToString();
    }

    // チームラベルの文字と色をセットするメソッド
    public void SetTeamLabel(string myTeam)
    {
        MyTeamLabel.text = myTeam;
        if (myTeam == "A")
        {
            RivalTeamLabel.text = "B";
            MyTeamLabel.color = new Color(0, 0, 1.0f, 1.0f); ;
            RivalTeamLabel.color = new Color(1.0f, 0, 0, 1.0f); ;
        }
        else
        {
            RivalTeamLabel.text = "A";
            MyTeamLabel.color = new Color(1.0f, 0, 0, 1.0f); ;
            RivalTeamLabel.color = new Color(0, 0, 1.0f, 1.0f); ;
        }
    }

    public string GetMyTeam()
    {
        return MyTeamLabel.text;
    }

    public string GetRivalTeam()
    {
        return RivalTeamLabel.text;
    }

    public string GetMyTeamMember()
    {
        return MyTeamMember.text;
    }

    public string GetRivalTeamMember()
    {
        return RivalTeamMember.text;
    }

    public int GetMyScore()
    {
        return MyScore;
    }

    public int GetRivalScore()
    {
        return RivalScore;
    }

    public void SetScore()
    {
        SetMyScore(PlayerPrefs.GetInt($"Score{GetMyTeam()}"));
        SetRivalScore(PlayerPrefs.GetInt($"Score{GetRivalTeam()}"));
        SetMyBombCnt(MyCharacter.GetBombCnt());
        SetRivalBombCnt(RivalCharacter.GetBombCnt());
    }

}
