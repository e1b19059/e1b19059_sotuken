using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MyTeamMember = default;
    [SerializeField] private TextMeshProUGUI RivalTeamMember = default;
    [SerializeField] private TextMeshProUGUI MyTeamLabel;
    [SerializeField] private TextMeshProUGUI RivalTeamLabel;
    [SerializeField] private TextMeshProUGUI MyScoreLabel;
    [SerializeField] private TextMeshProUGUI RivalScoreLabel;

    private StringBuilder MyBuilder;
    private StringBuilder RivalBuilder;
    private float elapsedTime;
    private int MyScore;
    private int RivalScore;

    public static ScoreBoard instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        MyBuilder = new StringBuilder();
        RivalBuilder = new StringBuilder();
        elapsedTime = 0f;
    }

    void Update()
    {
        // ゲーム中のみ更新する
        if (!PhotonLogin.instance.GetPlayingFlag()) { return; }
        // 0.1秒毎にテキストを更新する
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.1f)
        {
            elapsedTime = 0f;
            UpdateMember();
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
                MyBuilder.AppendLine($"{player.NickName}({player.ActorNumber})");
            }
            else
            {
                RivalBuilder.AppendLine($"{player.NickName}({player.ActorNumber})");
            }
        }
        MyTeamMember.text = MyBuilder.ToString();
        RivalTeamMember.text = RivalBuilder.ToString();
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

    // チームラベルの文字と色をセットするメソッド
    public void SetTeamLabel(string myTeam)
    {
        MyTeamLabel.text = myTeam;
        MyTeamLabel.color = GameObject.FindGameObjectWithTag($"Player{MyTeamLabel.text}").GetComponent<Renderer>().material.color;
        if (GetMyTeam() == "A")
        {
            RivalTeamLabel.text = "B";
            RivalTeamLabel.color = GameObject.FindGameObjectWithTag("PlayerB").GetComponent<Renderer>().material.color;
        }
        else
        {
            RivalTeamLabel.text = "A";
            RivalTeamLabel.color = GameObject.FindGameObjectWithTag("PlayerA").GetComponent<Renderer>().material.color;
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

}
