using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using MyConstant;

public class CustomPropertiesCallbacks : MonoBehaviourPunCallbacks
{
    [SerializeField] private ScoreBoard scoreBoard;
    [SerializeField] private TeamSelect teamSelect;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // 更新されたプレイヤーのカスタムプロパティのペアをコンソールに出力する
        foreach (var prop in changedProps)
        {
            if (PhotonNetwork.IsMasterClient && prop.Key.ToString() == GrovalConst.TeamKey)
            {
                teamSelect.CheckAllReady();
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // 更新されたルームのカスタムプロパティのペアをコンソールに出力する
        foreach (var prop in propertiesThatChanged)
        {
            if (prop.Key.ToString() == GrovalConst.ScoreAKey 
                || prop.Key.ToString() == GrovalConst.ScoreBKey)
            {     
                if(prop.Key.ToString() == scoreBoard.GetMyTeam())
                {
                    scoreBoard.SetMyScore((int)prop.Value);
                }
                else
                {
                    scoreBoard.SetRivalScore((int)prop.Value);
                }
            }
        }
    }
}