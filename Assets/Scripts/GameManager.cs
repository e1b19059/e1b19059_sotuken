using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonLogin photonLogin;
    [SerializeField] ScoreBoard scoreBoard;
    [SerializeField] CreateField createField;
    [SerializeField] ResultView resultView;
    [SerializeField] PhaseUIManager phaseUIManager;
    [SerializeField] Slider timer;// タイムバー
    [SerializeField] Button FinishTurnButton;// 時間が残っていてもターンを終えるボタン
    [SerializeField] Button ShowResultButton;// 結果を表示するボタン
    int TurnCount;
    int MaxTurn;
    int phase;
    int TrapNumber;
    int playerCnt;
    int codeFinPlayer;
    float InitFillAmount;
    float TurnDuration = 180f;
    float elapsedTime;
    float timeCounterA;
    float timeCounterB;
    bool counterFlagA = false;
    bool counterFlagB = false;
    bool IsShowingResults;
    bool IsDriver;
    bool IsFirst;
    bool IsShare;
    bool soloMode;

    [DllImport("__Internal")]
    private static extern bool doCode(bool IsMyturn);

    [DllImport("__Internal")]
    private static extern bool setMaxBlocks(bool _isFirst);

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

    [DllImport("__Internal")]
    private static extern void initTrash();

    private void Awake()
    {
        elapsedTime = 0f;
        TrapNumber = 3;
        MaxTurn = 8;
        ShowResultButton.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (!photonLogin.GetPlayingFlag()) { return; }
        if (this.TurnCount > 0 && this.timer != null && !IsShowingResults && (counterFlagA || counterFlagB))
        {
            //経過時間から移動量の計算
            float amount = Time.deltaTime / TurnDuration;

            //スライダーの移動量を代入
            timer.value -= amount;
        }
        if (counterFlagA)
        {
            timeCounterA += Time.deltaTime;
        }
        if (counterFlagB)
        {
            timeCounterB += Time.deltaTime;
        }
        if (IsShare)// ドライバーならブロックを送信する
        {
            // 0.5秒毎にブロックを送信する
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 1f)
            {
                elapsedTime = 0f;
                SendMyBlock();
            }
        }
        if (codeFinPlayer >= playerCnt)// 全プレイヤーがブロックを実行し終えた
        {
            codeFinPlayer = 0;
            photonView.RPC(nameof(FinishPhase), RpcTarget.AllViaServer);
        }
        if ((counterFlagA || counterFlagB) && timer.value == 0)// タイムアウト
        {
            FinishPhase();
        }
    }

    // フェーズの変化に伴う処理を行うメソッド、RpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void RPCPhaseChange(int _phase)
    {
        phase = _phase;
        phaseUIManager.SetHighLight(_phase);
        switch (_phase)
        {
            case 1:
                // ワークスペース、役割のセットとトラップの生成
                initWorkspace();
                if (IsDriver = AmIDriver(TurnCount))
                {
                    switchEditable();
                    setMaxBlocks(IsFirst);
                }
                else
                {
                    switchReadOnly();
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    for (int i = 0; i < TrapNumber; i++)
                    {
                        var point = createField.CreatePoint();
                        photonView.RPC(nameof(CreateTrap), RpcTarget.AllViaServer, point.x, point.z, point.type);
                    }
                    PhaseProgress(phase);
                }
                break;
            case 2:
                // 後攻チームがプログラミングを行う
                StartTimer(IsFirst);
                if (IsDriver)
                {
                    if (!IsFirst)
                    {
                        switchReadOnly();
                        replaceBlock();
                        SendMyBlock();
                        IsShare = false;
                    }
                    else
                    {
                        IsShare = true;
                    }
                }
                break;
            case 3:
                // 先攻チームがプログラミングを行う
                StartTimer(!IsFirst);
                if (IsDriver)
                {
                    if (!IsFirst)
                    {
                        switchEditable();
                        IsShare = true;
                    }
                    else
                    {
                        switchReadOnly();
                        replaceBlock();
                        SendMyBlock();
                        IsShare = false;
                    }
                }
                break;
            case 4:
                // 先攻チームのブロックが実行される
                if (IsDriver)
                {
                    if (!IsFirst)
                    {
                        switchReadOnly();
                        replaceBlock();
                    }
                    SendAndDoCode();
                }
                IsShare = false;
                break;
            case 5:
                // 後攻チームのブロックが実行される
                IsFirst = !IsFirst;
                DoCode();
                break;
            default:
                break;
        }
    }

    // タイマーを開始するメソッド
    public void StartTimer(bool _flag)
    {
        timer.value = 1.0f;// タイマー初期化

        if (scoreBoard.GetMyTeam() == "A")
        {
            if (_flag)
            {
                counterFlagA = true;
                if (IsDriver) FinishTurnButton.interactable = true;
            }
            else
            {
                counterFlagB = true;
            }
        }
        else
        {
            if (_flag)
            {
                counterFlagB = true;
                if (IsDriver) FinishTurnButton.interactable = true;
            }
            else
            {
                counterFlagA = true;
            }
        }
    }

    public void StopTimer()
    {
        counterFlagA = false;
        counterFlagB = false;
        FinishTurnButton.interactable = false;
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
            photonView.RPC(nameof(RPCPhaseChange), RpcTarget.AllViaServer, _phase + 1);
        }
    }

    [PunRPC]
    public void FinishPhase()
    {
        StopTimer();
        if (PhotonNetwork.IsMasterClient) PhaseProgress(phase);
    }

    // RpcTarget.MasterClientで呼ばれる
    [PunRPC]
    public void RPCTellFinished()
    {
        codeFinPlayer++;
    }

    // ターンを開始するメソッド、RpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void StartTurn()// ターンを開始する
    {
        Debug.Log(++TurnCount + "ターン目開始");// ターン加算
        phaseUIManager.Init(scoreBoard.GetMyTeam(), IsFirst);
        if (PhotonNetwork.IsMasterClient)
        {
            var point = createField.CreatePoint();
            createField.photonView.RPC(nameof(createField.RPCCreateCoin), RpcTarget.AllViaServer, new Vector3(point.x, 0, point.z));
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
        photonView.RPC(nameof(InitMode), RpcTarget.All);// モードの初期化
        if (Anum == 1) photonView.RPC(nameof(OnSoloModeA), RpcTarget.All);
        if (Bnum == 1) photonView.RPC(nameof(OnSoloModeB), RpcTarget.All);
        PhotonNetwork.CurrentRoom.IsOpen = false;// 途中入室できなくする
        PhotonNetwork.CurrentRoom.SetInit(Anum, Bnum);
        photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer, Anum + Bnum);
    }

    [PunRPC]
    public void RPCGameStart(int _playerCnt)
    {
        Debug.Log("ゲーム開始");
        photonLogin.GameInit();
        initWorkspace();
        initTrash();
        ShowResultButton.transform.localScale = Vector3.zero;
        FinishTurnButton.interactable = false;
        counterFlagA = false;
        counterFlagB = false;
        timeCounterA = 0;
        timeCounterB = 0;
        TurnCount = 0;
        codeFinPlayer = 0;
        playerCnt = _playerCnt;
        scoreBoard.SetPlayerBehaviour();
        IsFirst = (PhotonNetwork.LocalPlayer.GetTeam() == "A");
        StartTurn();
    }

    [PunRPC]
    public void RPCGameFinish()
    {
        Debug.Log("ゲーム終了");
        photonLogin.Finished();
        phaseUIManager.Finished();
        PlayerPrefs.SetFloat("TimeA", timeCounterA);
        PlayerPrefs.SetFloat("TimeB", timeCounterB);
        resultView.SetResult();
        ShowResultButton.transform.localScale = Vector3.one;
    }

    [PunRPC]
    public void InitMode()
    {
        soloMode = false;
    }

    [PunRPC]
    public void OnSoloModeA()
    {
        if (scoreBoard.GetMyTeam() == "A")
        {
            soloMode = true;
        }
    }

    [PunRPC]
    public void OnSoloModeB()
    {
        if (scoreBoard.GetMyTeam() == "B")
        {
            soloMode = true;
        }
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

    public void SendAndDoCode()
    {
        photonView.RPC(nameof(SetBlockAndDoCode), RpcTarget.Others, getBlockFromWorkspace());
    }

    // ブロックをセットするメソッド
    [PunRPC]
    public void SetOthersBlock(string block, PhotonMessageInfo info)
    {
        // ナビゲーターなら、敵味方を判別してブロックをセットする
        if (!IsDriver || (IsDriver && soloMode))
        {
            if (scoreBoard.GetMyTeam() == info.Sender.GetTeam())
            {
                setFriendBlock(block);
            }
            else
            {
                if (phase != 1) setRivalBlock(block);
            }
        }
    }

    [PunRPC]
    public void SetBlockAndDoCode(string block, PhotonMessageInfo info)
    {
        bool _IsMyTurn = IsFirst;
        if (scoreBoard.GetMyTeam() == info.Sender.GetTeam())
        {
            setFriendBlock(block);
        }
        else
        {
            setRivalBlock(block);
        }
        if (IsDriver && IsFirst) photonView.RPC(nameof(DoCode), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void DoCode()
    {
        bool _IsMyTurn = !IsFirst;// 後にプログラミングした方が先に実行する
        GameObject obj;
        if (_IsMyTurn)
        {
            obj = GameObject.FindWithTag($"Player{scoreBoard.GetMyTeam()}");
        }
        else
        {
            obj = GameObject.FindWithTag($"Player{scoreBoard.GetRivalTeam()}");
        }
        setPlayerCharacter(obj.name);
        doCode(_IsMyTurn);
    }

    // ターン終了するメソッド
    public void FinishTurn()
    {
        photonView.RPC(nameof(RPCExplodeBomb), RpcTarget.AllViaServer);

        if (TurnCount % 2 == 0)
        {
            photonView.RPC(nameof(RPCTrapDestroy), RpcTarget.AllViaServer);
        }

        if (TurnCount < MaxTurn)
        {
            photonView.RPC(nameof(StartTurn), RpcTarget.AllViaServer);
        }
        else
        {
            photonView.RPC(nameof(RPCGameFinish), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void RPCExplodeBomb()
    {
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (var bomb in bombs)
        {
            bomb.GetComponent<BombManager>().Explode();
        }
    }

    // トラップオブジェクトを破壊するメソッド、RpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void RPCTrapDestroy()
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (var trap in traps)
        {
            Destroy(trap);
        }
    }

    // ブロックの編集を終える
    public void FinEdit()
    {
        photonView.RPC(nameof(FinishPhase), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void CreateTrap(int posX, int posZ, int trapType)
    {
        if (IsDriver)
        {
            createField.RPCCreateTrap(posX, posZ, trapType, false);
        }
        else
        {
            createField.RPCCreateTrap(posX, posZ, trapType, true);
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
