using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using MyConstant;

public class CustomPropertiesCallbacks : MonoBehaviourPunCallbacks
{
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // カスタムプロパティが更新されたプレイヤーのプレイヤー名とIDをコンソールに出力する
        Debug.Log($"{targetPlayer.NickName}({targetPlayer.ActorNumber})");

        // 更新されたプレイヤーのカスタムプロパティのペアをコンソールに出力する
        foreach (var prop in changedProps)
        {
            Debug.Log($"{prop.Key}: {prop.Value}");
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // 更新されたルームのカスタムプロパティのペアをコンソールに出力する
        foreach (var prop in propertiesThatChanged)
        {
            if (prop.Key.ToString() == GrovalConst.FirstKey)
            {
                TurnManager.instance.SetFirstToNum(prop.Value.ToString());
            }
            else if (prop.Key.ToString() == GrovalConst.ScoreAKey 
                || prop.Key.ToString() == GrovalConst.ScoreBKey)
            {     
                if(prop.Key.ToString() == ScoreBoard.instance.GetMyTeam())
                {
                    ScoreBoard.instance.SetMyScore((int)prop.Value);
                }
                else
                {
                    ScoreBoard.instance.SetRivalScore((int)prop.Value);
                }
            }
            Debug.Log($"{prop.Key}: {prop.Value}");
        }
    }
}