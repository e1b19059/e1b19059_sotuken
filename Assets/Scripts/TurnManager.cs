using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Runtime.InteropServices;
using MyConstant;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager instance;

    [SerializeField] private TextMeshProUGUI TurnText;//ターン数の表示テキスト
    [SerializeField] private Image MeterImg;// 残り時間を示す画像
    [SerializeField] private Button FinishTurnButton;// 時間が残っていてもターンを終えるボタン
    [SerializeField] private Button ShowResultButton;// 結果を表示するボタン
    private int TurnCount = 0;
    private int MaxTurn;
    private int FirstOrSecond;
    private float InitFillAmount;
    private float TurnDuration = 60f;
    private float elapsedTime;
    private bool IsShowingResults;
    private bool MiddleOfTurn;
    private bool TurnFlag;
    private bool NextTurnFlag;
    private bool LastRun;
    [SerializeField] private TextMeshProUGUI Test;
    [SerializeField] private TextMeshProUGUI Test2;

    [DllImport("__Internal")]
    private static extern void doCode();

    [DllImport("__Internal")]
    private static extern void setTargetObject(string str);

    [DllImport("__Internal")]
    private static extern string getBlockFromWorkspace();

    [DllImport("__Internal")]
    private static extern void setFriendBlock(string block);

    [DllImport("__Internal")]
    private static extern void setRivalBlock(string block);

    [DllImport("__Internal")]
    private static extern void switchReadOnly();

    [DllImport("__Internal")]
    private static extern void replaceBlock();

    [DllImport("__Internal")]
    private static extern void clearRival();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitFillAmount = MeterImg.fillAmount;
        LastRun = true;
        TurnFlag = false;
        elapsedTime = 0f;
        MaxTurn = 4;
        FinishTurnButton.interactable = false;
        ShowResultButton.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (!PhotonLogin.instance.GetPlayingFlag()) { return; }
        if (this.TurnText != null)
        {
            this.TurnText.text = TurnCount.ToString();//何ターン目か表示
        }
        if (this.TurnCount > 0 && this.MeterImg != null && !IsShowingResults && MiddleOfTurn)
        {
            MeterImg.fillAmount -= InitFillAmount / this.TurnDuration * Time.deltaTime;
        }
        if (TurnFlag && MiddleOfTurn)// 自分のターンならブロックを送信する
        {
            // 0.5秒毎にブロックを送信する
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.5f)
            {
                elapsedTime = 0f;
                SendMyBlock();
            }
        }
        if (MiddleOfTurn && MeterImg.fillAmount == 0)// タイムアウト
        {
            OnTurnEnds();
        }
    }

    [PunRPC]
    public void OnTurnBegins()// ターン開始時
    {
        Debug.Log("OnTurnBegins() turn: " + ++TurnCount);
        MiddleOfTurn = true;
        IsShowingResults = false;
        MeterImg.fillAmount = InitFillAmount;
        if (CheckMyTurn(0))
        {
            Test.text = "Your Turn";
            Test2.text = "Next: Other's Turn";
            TurnFlag = true;
            NextTurnFlag = false;
            FinishTurnButton.interactable = true;
        }
        else
        {
            Test.text = "Other's Turn";
            TurnFlag = false;
            FinishTurnButton.interactable = false;
#if !UNITY_EDITOR && UNITY_WEBGL
            switchReadOnly();
#endif
            if (CheckMyTurn(1))
            {
                Test2.text = "Next: Your Turn";
                NextTurnFlag = true;
#if !UNITY_EDITOR && UNITY_WEBGL
                clearRival();
#endif
            }
            else
            {
                Test2.text = "Next: Other's Turn";
                NextTurnFlag = false;
            }
        }
    }

    [PunRPC]
    public void OnTurnEnds()// ターン終了時
    {
        Debug.Log("OnTurnEnds: " + TurnCount);
        MiddleOfTurn = false;
        if (TurnFlag)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            replaceBlock();
#endif
        }
        else if (NextTurnFlag)
        {
            DoCode();
        }
    }

    // ブロック実行時、最後にJavaScriptから呼び出される関数内でRpcTarget.AllViaServerで呼ばれる
    [PunRPC]
    public void StartTurn()// ターンを開始する
    {
        if (TurnCount >= MaxTurn)
        {
            if (!LastRun)
            {
                GameFinish();
            }
            if (TurnFlag && LastRun)
            {
                LastRun = false;
                DoCode();
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(OnTurnBegins), RpcTarget.AllViaServer);
            }
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
                Anum++;
                player.SetOrder(Anum);
            }
            else
            {
                Bnum++;
                player.SetOrder(Bnum);
            }
        }
        if (Anum == 0 || Bnum == 0)
        {
            Debug.Log("人数が足りていません");
        }
        else
        {
            // 乱数で先攻チームを決める
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
            CreateCoin();
            photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    public void RPCGameStart()
    {
        Debug.Log("ゲーム開始");
        PhotonLogin.instance.GameInit();
        StartTurn();
    }

    public void GameFinish()
    {
        photonView.RPC(nameof(RPCGameFinish), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCGameFinish()
    {
        Debug.Log("ゲーム終了");
        PhotonLogin.instance.Finished();
        ShowResultButton.transform.localScale = Vector3.one;
    }

    // turnが自分のチームのターンか確認するメソッド
    public bool CheckTeamTurn(int turn)
    {
        if (turn % 2 == FirstOrSecond)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // nextターン先のターンが自分のターンか確認するメソッド
    public bool CheckMyTurn(int next)
    {
        int turn = TurnCount + next;
        if (CheckTeamTurn(turn))
        {
            bool odd = turn % 2 == 1 ? true : false;
            turn /= 2;
            if (odd)
            {
                turn += 1;
            }
            if (PhotonNetwork.LocalPlayer.GetTeam() == "A")
            {
                int num = PhotonNetwork.CurrentRoom.GetANum();
                if ((turn + num - 1) % num + 1 == PhotonNetwork.LocalPlayer.GetOrder())
                {
                    return true;
                }
            }
            else
            {
                int num = PhotonNetwork.CurrentRoom.GetBNum();
                if ((turn + num - 1) % num + 1 == PhotonNetwork.LocalPlayer.GetOrder())
                {
                    return true;
                }
            }
        }
        return false;
    }

    // 自分のチームが先攻か確認して定数を取得する
    public void SetFirstToNum(string first)
    {
        if (first == PhotonNetwork.LocalPlayer.GetTeam())
        {
            FirstOrSecond = GrovalConst.FirstNum;
        }
        else
        {
            FirstOrSecond = GrovalConst.SecondNum;
        }
    }

    public void SendMyBlock()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        photonView.RPC(nameof(SetOthersBlock), RpcTarget.Others, getBlockFromWorkspace());
#endif
    }

    [PunRPC]
    public void SetOthersBlock(string block, PhotonMessageInfo info)
    {
        // 次のターンにドライバーになるプレイヤーにはブロックを共有しない
        if (!NextTurnFlag)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Debug.Log("ブロックをセット");
            if (ScoreBoard.instance.GetMyTeam() == info.Sender.GetTeam())
            {
                setFriendBlock(block);
            }
            else
            {
                setRivalBlock(block);
            }
#endif
        }
    }

    public void DoCode()
    {
        Debug.Log("実行");
        GameObject obj = GameObject.FindGameObjectWithTag($"Player{ScoreBoard.instance.GetMyTeam()}");
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

    // ブロックの編集を止め、ターンを終えるメソッド
    public void FinishTurn()
    {
        photonView.RPC(nameof(OnTurnEnds), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void CreateCoin()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Physics.OverlapSphere(new Vector3(0, -0.2f, 1), 0).Length <= 0)
            {
                PhotonNetwork.InstantiateRoomObject("Coin", new Vector3(0, 0, 1), Quaternion.Euler(90, 0, 0));
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

}
