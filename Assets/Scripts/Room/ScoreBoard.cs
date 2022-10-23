using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MyTeamMember = default;
    [SerializeField] private TextMeshProUGUI RivalTeamMember = default;

    private StringBuilder MyBuilder;
    private StringBuilder RivalBuilder;
    private float elapsedTime;

    private void Start()
    {
        MyBuilder = new StringBuilder();
        RivalBuilder = new StringBuilder();
        elapsedTime = 0f;
    }

    void Update()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }
        
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
            if (player.GetTeam() == PhotonLogin.instance.GetMyTeamLabel())
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

}
