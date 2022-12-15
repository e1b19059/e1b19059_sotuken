using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] CreateField createField;
    [SerializeField] GameObject TeamSelectPanel;
    [SerializeField] ObjectContainer container;
    bool PlayingFlag;
    bool Joined;

    private void Awake()
    {
        PlayingFlag = false;
        Joined = false;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            // プレイヤー自身の名前を"Player"に設定する
            PhotonNetwork.NickName = "Player";
        }

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
        if (!Joined)
        {
            createField.CreateFloor();
            Joined = true;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            createField.CreateWallAndCharacter();
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Leave();
    }

    public void Leave()
    {
        var enumerator = container.GetEnumerator();
        PhotonNetwork.LeaveRoom();
        while (enumerator.MoveNext())
        {
            Destroy(enumerator.Current.gameObject);
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
        PlayerPrefs.SetInt("ScoreA", 0);
        PlayerPrefs.SetInt("ScoreB", 0);
        TeamSelectPanel.transform.localScale = Vector3.zero;
    }

    public void Finished()
    {
        PlayingFlag = false;
    }

}