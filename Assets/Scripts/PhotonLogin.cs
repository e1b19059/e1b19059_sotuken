using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    public GameObject obstacle;
    [SerializeField] private GameObject TeamSelectPanel;
    private bool PlayingFlag;

    public static PhotonLogin instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        PlayingFlag = false;
    }

    private void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var positionA = new Vector3(0, 0, 0);
            var positionB = new Vector3(5, 0, 5);
            PhotonNetwork.InstantiateRoomObject("PlayerA", positionA, Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject("PlayerB", positionB, Quaternion.identity);

            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(-1f, 0, 1f), Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(1f, 0, 1f), Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(1f, 0, 2f), Quaternion.identity);
        }
    }

    public void FocusCanvas(string p_focus)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        if (p_focus == "0")
        {
            WebGLInput.captureAllKeyboardInput = false;
        }
        else
        {
            WebGLInput.captureAllKeyboardInput = true;
        }
#endif
    }

    public bool GetPlayingFlag()
    {
        return PlayingFlag;
    }

    public void GameInit()
    {
        PlayingFlag = true;
        TeamSelectPanel.transform.localScale = Vector3.zero;
    }
}