using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using MyConstant;

public class TurnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI TurnText;//ターン数の表示テキスト
    [SerializeField] private Image UIobj;// 残り時間を示す画像
    private int TurnCount = 0;
    private float InitFillAmount;
    private float TurnDuration = 5f;
    private bool IsShowingResults;
    private bool TurnFlag;
    private int TeamNumber;
    [SerializeField] private TextMeshProUGUI Test;
    [SerializeField] private TextMeshProUGUI Test2;
    public static TurnManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitFillAmount = UIobj.fillAmount;
        TurnFlag = true;
    }

    private void Update()
    {
        if (this.TurnText != null)
        {
            this.TurnText.text = TurnCount.ToString();//何ターン目か表示
        }
        if (this.TurnCount > 0 && this.UIobj != null && !IsShowingResults)
        {
            UIobj.fillAmount -= InitFillAmount / this.TurnDuration * Time.deltaTime;
        }
        if (TurnFlag && UIobj.fillAmount == 0)// タイムアウト
        {
            OnTurnEnds();
        }
    }

    [PunRPC]
    public void OnTurnBegins()// ターン開始時
    {
        Debug.Log("OnTurnBegins() turn: " + ++TurnCount);
        TurnFlag = true;
        IsShowingResults = false;
        UIobj.fillAmount = InitFillAmount;
        if (CheckMyTurn(0))
        {
            Test.text = "Your Turn";
        }
        else
        {
            Test.text = "Other's Turn";
        }
        if (CheckMyTurn(1))
        {
            Test2.text = "Next: Your Turn";
        }
        else
        {
            Test2.text = "Next: Other's Turn";
        }
    }

    public void OnTurnEnds()// ターン終了時
    {
        Debug.Log("OnTurnEnds: " + TurnCount);
        TurnFlag = false;
        StartTurn();
    }

    public void StartTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(OnTurnBegins), RpcTarget.AllViaServer);
        }
    }

    public bool CheckTeamTurn(int turn)
    {
        if (turn % 2 == TeamNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

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
                int num = PhotonNetwork.CurrentRoom.GetANum();
                if ((turn + num - 1) % num + 1 == PhotonNetwork.LocalPlayer.GetOrder())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetFirstToNum(string first)
    {
        if (first == PhotonNetwork.LocalPlayer.GetTeam())
        {
            TeamNumber = GrovalConst.FirstNum;
        }
        else
        {
            TeamNumber = GrovalConst.SecondNum;
        }
    }

}
