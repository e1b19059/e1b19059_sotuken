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
    private bool PlayingFlag;

    [SerializeField] private TextMeshProUGUI MyTeamLabel;
    [SerializeField] private TextMeshProUGUI RivalTeamLabel;

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
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
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
        Debug.Log("実行");
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
        Debug.Log("setOthersBlock実行");
#if !UNITY_EDITOR && UNITY_WEBGL
        setBlockToWorkspace(block);
#endif
    }

    private void Update()
    { 
        if (PhotonNetwork.CurrentRoom != null)//ルーム内である
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            PhotonView photonView = obj.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                SendMyBlock();
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
        // ルーム内のメンバー全員が準備完了状態のときのみ押せるようにする
        photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCGameStart()
    {
        Debug.Log("ゲーム開始");
        TeamSelectPanel.transform.localScale = Vector3.zero;
        PlayingFlag = true;
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