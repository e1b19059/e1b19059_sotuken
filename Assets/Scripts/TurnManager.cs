using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TurnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI TurnText;//ターン数の表示テキスト
    [SerializeField] private Image UIobj;// 残り時間を示す画像
    private int TurnCount = 0;
    private float InitFillAmount;
    private float TurnDuration = 5f;
    private bool IsShowingResults;
    private bool TurnFlag;

    private void Awake()
    {
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

}
