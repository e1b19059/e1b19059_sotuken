using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomMember : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label = default;

    private StringBuilder builder;
    private float elapsedTime;

    private void Start()
    {
        builder = new StringBuilder();
        elapsedTime = 0f;
    }

    private void Update()
    {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }

        // 0.1秒毎にテキストを更新する
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.1f)
        {
            elapsedTime = 0f;
            UpdateLabel();
        }
    }

    private void UpdateLabel()
    {
        var players = PhotonNetwork.PlayerList;
        builder.Clear();
        builder.AppendLine("ルームメンバー");
        foreach (var player in players)
        {
            string team;
            if (player.GetTeam() == "A")
            {
                team = "<color=#0000FF>A</color>";
            }
            else if (player.GetTeam() == "B")
            {
                team = "<color=#FF0000>B</color>";
            }
            else
            {
                team = "";
            }
            builder.AppendLine($"{player.NickName}({player.ActorNumber}) - {team}");
        }
        label.text = builder.ToString();
    }

}