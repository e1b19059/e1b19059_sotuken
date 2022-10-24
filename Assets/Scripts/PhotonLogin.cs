using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [DllImport("__Internal")]
    private static extern void doCode();

    [DllImport("__Internal")]
    private static extern void setTargetObject(string str);

    [DllImport("__Internal")]
    private static extern string getBlockFromWorkspace();
    
    [DllImport("__Internal")]
    private static extern void setBlockToWorkspace(string block);

    public GameObject obstacle;
    [SerializeField] private GameObject TeamSelectPanel;
    [SerializeField] private TextMeshProUGUI MyTeamLabel;
    [SerializeField] private TextMeshProUGUI RivalTeamLabel;
    private bool PlayingFlag;
    private float elapsedTime;

    public static PhotonLogin instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        PlayingFlag = false;
        elapsedTime = 0f;
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
            var position = new Vector3(0, 0, 0);
            PhotonNetwork.InstantiateRoomObject("Player", position, Quaternion.identity);

            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(-1f, 0, 1f), Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(1f, 0, 1f), Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(1f, 0, 2f), Quaternion.identity);
        }
    }

    public void DoCode()
    {
        Debug.Log("���s");
        GameObject obj =  GameObject.FindGameObjectWithTag("Player");
        PhotonView photonView = obj.GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        setTargetObject(obj.name);
        doCode();
#endif
    }

    public void SendMyBlock()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        photonView.RPC(nameof(SetOthersBlock), RpcTarget.Others, getBlockFromWorkspace());
#endif
    }

    [PunRPC]
    public void SetOthersBlock(string block)
    {
        Debug.Log("setOthersBlock���s");
#if !UNITY_EDITOR && UNITY_WEBGL
        setBlockToWorkspace(block);
#endif
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)//���[�����ł���
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            PhotonView photonView = obj.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                // 0.5�b���Ƀe�L�X�g���X�V����
                elapsedTime += Time.deltaTime;
                if (elapsedTime > 0.5f)
                {
                    elapsedTime = 0f;
                    SendMyBlock();
                }
            }
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

    public void GameStart()
    {
        // ���[�����̃����o�[�S��������������Ԃ̂Ƃ��̂݉�����悤�ɂ���
        int Anum = 0, Bnum = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if(player.GetTeam() == "A")
            {
                Anum++;
                player.SetOrder(Anum);
            }
            else
            {
                Bnum++;
                player.SetOrder(Bnum);
            }
        }
        if(Anum == 0 || Bnum == 0)
        {
            Debug.Log("�l��������Ă��܂���");
        }
        else
        {
            // �����Ő�U�`�[�������߂�
            if (Random.Range(0, 2) == 0)
            {
                PhotonNetwork.CurrentRoom.SetFirst("A");
            }
            else
            {
                PhotonNetwork.CurrentRoom.SetFirst("B");
            }
            PhotonNetwork.CurrentRoom.SetANum(Anum);
            PhotonNetwork.CurrentRoom.SetBNum(Bnum);
            photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    public void RPCGameStart()
    {
        Debug.Log("�Q�[���J�n");
        TeamSelectPanel.transform.localScale = Vector3.zero;
        PlayingFlag = true;
        if (PhotonNetwork.IsMasterClient)
        {
            TurnManager.instance.StartTurn();
        }
    }

    public void SetTeamLabel(string myTeam)
    {
        MyTeamLabel.text = myTeam;
        if (MyTeamLabel.text == "A")
        {
            RivalTeamLabel.text = "B";
        }
        else
        {
            RivalTeamLabel.text = "A";
        }
    }

    public bool GetPlayingFlag()
    {
        return PlayingFlag;
    }

    public string GetMyTeamLabel()
    {
        return MyTeamLabel.text;
    }
}