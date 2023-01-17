using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Runtime.InteropServices;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] CreateField createField;
    [SerializeField] TeamSelect teamSelect;
    [SerializeField] ObjectContainer container;
    [SerializeField] GameObject mainCamera;
    bool finished;
    bool PlayingFlag;
    bool Joined;

    [DllImport("__Internal")]
    private static extern void leaveAlert();

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
        finished = false;
        mainCamera.transform.position = new Vector3(3, 10, 3);
        mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (!finished)
        {
            GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
            foreach (var trap in traps)
            {
                Destroy(trap);
            }
            teamSelect.Cancel();
            Leave();
            Finished();
            leaveAlert();
        }
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
        PlayerPrefs.SetInt("CoinA", 0);
        PlayerPrefs.SetInt("DamageA", 0);
        PlayerPrefs.SetInt("ScoreA", 0);
        PlayerPrefs.SetInt("MissA", 0);
        PlayerPrefs.SetInt("CoinB", 0);
        PlayerPrefs.SetInt("DamageB", 0);
        PlayerPrefs.SetInt("ScoreB", 0);
        PlayerPrefs.SetInt("MissB", 0);
        teamSelect.HidePanel();
    }

    public void Finished()
    {
        PlayingFlag = false;
        finished = true;
    }

}