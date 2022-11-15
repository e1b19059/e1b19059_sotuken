using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Runtime.InteropServices;
using MyConstant;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonLogin photonLogin;
    [SerializeField] private ScoreBoard scoreBoard;
    [SerializeField] private CreateField createField;
    [SerializeField] private ObjectContainer container;
    [SerializeField] private TextMeshProUGUI TurnText;//ターン数の表示テキスト
    [SerializeField] private Image MeterImg;// 残り時間を示す画像
    [SerializeField] private Button FinishTurnButton;// 時間が残っていてもターンを終えるボタン
    [SerializeField] private Button ShowResultButton;// 結果を表示するボタン
    [SerializeField] private TextMeshProUGUI PhaseUI;//フェーズの表示テキスト
    private int TurnCount = 0;
    private int MaxTurn;
    private float InitFillAmount;
    private float TurnDuration = 60f;
    private float elapsedTime;
    private bool IsShowingResults;

    bool IsDriver;
    bool IsFirst;
    bool IsFinished;
    bool IsShare;
    int phase;

    [DllImport("__Internal")]
    private static extern bool doCode();

    [DllImport("__Internal")]
    private static extern void setPlayerCharacter(string str);

    [DllImport("__Internal")]
    private static extern string getBlockFromWorkspace();

    [DllImport("__Internal")]
    private static extern void setFriendBlock(string block);

    [DllImport("__Internal")]
    private static extern void setRivalBlock(string block);

    [DllImport("__Internal")]
    private static extern void switchReadOnly();

    [DllImport("__Internal")]
    private static extern void switchEditable();

    [DllImport("__Internal")]
    private static extern void replaceBlock();

    [DllImport("__Internal")]
    private static extern void clearRival();

    [DllImport("__Internal")]
    private static extern void initWorkspace();

    private void Awake()
    {
        InitFillAmount = MeterImg.fillAmount;
        elapsedTime = 0f;
        MaxTurn = 4;
        ShowResultButton.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (!photonLogin.GetPlayingFlag()) { return; }
        if (this.TurnText != null)
        {
            this.TurnText.text = TurnCount.ToString();//何ターン目か表示
        }
        if (this.TurnCount > 0 && this.MeterImg != null && !IsShowingResults && !IsFinished)
        {
            MeterImg.fillAmount -= InitFillAmount / this.TurnDuration * Time.deltaTime;
        }
        if (IsShare)// ドライバーならブロックを送信する
        {
            // 0.5秒毎にブロックを送信する
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.5f)
            {
                elapsedTime = 0f;
                SendMyBlock();
            }
        }
        if (this.PhaseUI != null)
        {
            this.PhaseUI.text = "phase:" + phase.ToString();
        }
        if (!IsFinished && MeterImg.fillAmount == 0)// タイムアウト
        {
            Debug.Log("タイムアウト");
            FinishPhase();
        }
    }

    // フェーズの変化に伴う処理を行うメソッド、RpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void RPCPhaseChange(int _phase)
    {
        phase = _phase;
        // phaseは1,2,3はタイムアウト時のRPC、4,5はブロックの末尾で進行させるようになっている
        switch (_phase)
        {
            case 1:
                // 先攻後攻にかかわらず両方のチームがプログラミングを行う。相手チームへのブロック共有はなし
                StartTimer();
                initWorkspace();
                if (IsDriver = AmIDriver(TurnCount))
                {
                    IsShare = true;
                    switchEditable();
                }
                else
                {
                    switchReadOnly();
                }
                break;
            case 2:
                // 先攻チームがプログラミングを行う。ブロック共有はあり(双方向)
                // 自分のチームが先攻か確認して、ワークスペースの切り替え
                StartTimer();
                if (IsDriver)
                {
                    if (!IsFirst)
                    {
                        switchReadOnly();
                        replaceBlock();
                    }
                }
                break;
            case 3:
                // 後攻チームがプログラミングを行う。ブロック共有はあり(双方向)
                // 自分のチームが後攻か確認して、ワークスペースの切り替え
                StartTimer();
                if (IsDriver)
                {
                    if (!IsFirst)
                    {
                        switchEditable();
                    }
                    else
                    {
                        switchReadOnly();
                        replaceBlock();
                    }
                }
                break;
            case 4:
                // 先攻チームのブロックを実行
                // 両方のチームのワークスペースを読み込み専用にして、先攻かつドライバーならブロックを実行
                if (IsDriver && !IsFirst)
                {
                    switchReadOnly();
                    replaceBlock();
                }
                else if (IsDriver && IsFirst)
                {
                    DoCode();
                }
                IsShare = false;
                break;
            case 5:
                // 後攻チームのブロックを実行
                // 後攻かつドライバーならブロックを実行
                if (IsDriver && !IsFirst)
                {
                    DoCode();
                }
                IsFirst = !IsFirst;
                break;
            default:
                break;
        }
    }

    // タイマーを開始するメソッド
    public void StartTimer()
    {
        MeterImg.fillAmount = InitFillAmount;// タイマー初期化
        IsFinished = false;// タイマー開始
    }

    // フェーズを進行させるメソッド
    public void PhaseProgress(int _phase)
    {
        // phaseが5以上ならターンを終える,そうでないならphase+1
        if (_phase >= 5)
        {
            FinishTurn();
        }
        else
        {
            Debug.Log("PhaseProgress: " + _phase + ">>" + (_phase+1));
            photonView.RPC(nameof(RPCPhaseChange), RpcTarget.AllViaServer, _phase + 1);
        }
    }

    [PunRPC]
    public void FinishPhase()
    {
        Debug.Log("FinishPhase");
        IsFinished = true;// タイマーを止める
        if (PhotonNetwork.IsMasterClient) PhaseProgress(phase);
    }

    // ターンを開始するメソッド、RpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void StartTurn()// ターンを開始する
    {
        Debug.Log(++TurnCount + "ターン目開始");// ターン加算
        if (PhotonNetwork.IsMasterClient)
        {
            createField.CreateCoin();
            PhaseProgress(0);
        }
    }

    // ゲーム開始処理、マスターのみ実行
    public void GameStart()
    {
        // ルーム内のメンバー全員が準備完了状態のときのみ押せるようにする
        int Anum = 0, Bnum = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.GetTeam() == "A")
            {
                player.SetOrder(Anum++);
            }
            else
            {
                player.SetOrder(Bnum++);
            }
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;// 途中入室できなくする
        PhotonNetwork.CurrentRoom.SetInit(Anum, Bnum);
        photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCGameStart()
    {
        Debug.Log("ゲーム開始");
        photonLogin.GameInit();
        initWorkspace();
        ShowResultButton.transform.localScale = Vector3.zero;
        TurnCount = 0;
        IsFirst = (PhotonNetwork.LocalPlayer.GetTeam() == "A");
        StartTurn();
    }

    [PunRPC]
    public void RPCGameFinish()
    {
        Debug.Log("ゲーム終了");
        photonLogin.Finished();
        ShowResultButton.transform.localScale = Vector3.one;
    }

    // そのターン自分がドライバーか確認するメソッド
    // 同じプレイヤーが2ターン続けてドライバーになる
    public bool AmIDriver(int turn)
    {
        int num;
        if (scoreBoard.GetMyTeam() == "A")
        {
            num = PhotonNetwork.CurrentRoom.GetANum();
        }
        else
        {
            num = PhotonNetwork.CurrentRoom.GetBNum();
        }
        return (turn / 2 + turn % 2 + num - 1) % num == PhotonNetwork.LocalPlayer.GetOrder();
    }

    // ブロックを送信するメソッド
    public void SendMyBlock()
    {
        photonView.RPC(nameof(SetOthersBlock), RpcTarget.Others, getBlockFromWorkspace());
    }

    // ブロックをセットするメソッド
    [PunRPC]
    public void SetOthersBlock(string block, PhotonMessageInfo info)
    {
        // ナビゲーターなら、敵味方を判別してブロックをセットする
        if (!IsDriver)
        {
            if (scoreBoard.GetMyTeam() == info.Sender.GetTeam())
            {
                setFriendBlock(block);
            }
            else
            {
                if(phase != 1)setRivalBlock(block);
            }
        }
    }

    public void DoCode()
    {
        Debug.Log("実行");
        GameObject obj = GameObject.FindGameObjectWithTag($"Player{scoreBoard.GetMyTeam()}");
        PhotonView photonView = obj.GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        setPlayerCharacter(obj.name);
        doCode();
    }

    // ターン終了するメソッド、RpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void FinishTurn()
    {
        if (TurnCount < MaxTurn)
        {
            photonView.RPC(nameof(StartTurn), RpcTarget.AllViaServer);
        }
        else
        {
            photonView.RPC(nameof(RPCGameFinish), RpcTarget.AllViaServer);
        }
    }

    // フェーズが終わるかテストする
    public void FinTest()
    {
        photonView.RPC(nameof(FinishPhase), RpcTarget.AllViaServer);
    }

    public void PutObstacle(int direction)
    {
        GameObject obj = GameObject.FindWithTag($"Player{scoreBoard.GetMyTeam()}");
        Vector3 targetPos = obj.transform.position;
        switch (direction)
        {
            case 0:
                targetPos -= obj.transform.right;
                break;
            case 1:
                targetPos += obj.transform.right;
                break;
            case 2:
                targetPos += obj.transform.forward;
                break;
            case 3:
                targetPos -= obj.transform.forward;
                break;
        }
        targetPos.y = 0;// プレイヤーキャラクターのy座標は足元にあるため他のオブジェクトに合わせる
        createField.photonView.RPC(nameof(createField.RPCPutObstacle), RpcTarget.MasterClient, targetPos);
    }

    public void DestroyObstacle(int direction)
    {
        var enumerator = container.GetEnumerator();
        GameObject obj = GameObject.FindWithTag($"Player{scoreBoard.GetMyTeam()}");
        Vector3 targetPos = obj.transform.position;
        switch (direction)
        {
            case 0:
                targetPos -= obj.transform.right;
                break;
            case 1:
                targetPos += obj.transform.right;
                break;
            case 2:
                targetPos += obj.transform.forward;
                break;
            case 3:
                targetPos -= obj.transform.forward;
                break;
        }
        targetPos.y = 0;// プレイヤーキャラクターのy座標は足元にあるため他のオブジェクトに合わせる
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.transform.position == targetPos && enumerator.Current.gameObject.CompareTag("Destroyable"))
            {
                var _photonView = enumerator.Current.gameObject.GetComponent<PhotonView>();
                _photonView.RPC("RPCDestroy", RpcTarget.MasterClient);
                break;
            }
        }
    }

    public bool GetShowingResults()
    {
        return IsShowingResults;
    }

    public void ShowResult()
    {
        IsShowingResults = true;
        ShowResultButton.transform.localScale = Vector3.zero;
    }

    public void HideResult()
    {
        IsShowingResults = false;
    }

}
