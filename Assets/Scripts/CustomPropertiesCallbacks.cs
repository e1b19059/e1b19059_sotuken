using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using MyConstant;

public class CustomPropertiesCallbacks : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreBoard scoreBoard;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // �J�X�^���v���p�e�B���X�V���ꂽ�v���C���[�̃v���C���[����ID���R���\�[���ɏo�͂���
        Debug.Log($"{targetPlayer.NickName}({targetPlayer.ActorNumber})");

        // �X�V���ꂽ�v���C���[�̃J�X�^���v���p�e�B�̃y�A���R���\�[���ɏo�͂���
        foreach (var prop in changedProps)
        {
            Debug.Log($"{prop.Key}: {prop.Value}");
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // �X�V���ꂽ���[���̃J�X�^���v���p�e�B�̃y�A���R���\�[���ɏo�͂���
        foreach (var prop in propertiesThatChanged)
        {
            if (prop.Key.ToString() == GrovalConst.FirstKey)
            {
                gameManager.SetFirstToNum(prop.Value.ToString());
            }
            else if (prop.Key.ToString() == GrovalConst.ScoreAKey 
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
            Debug.Log($"{prop.Key}: {prop.Value}");
        }
    }
}