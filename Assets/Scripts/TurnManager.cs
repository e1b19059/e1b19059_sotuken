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

    [SerializeField] private TextMeshProUGUI TurnText;//�^�[�����̕\���e�L�X�g
    [SerializeField] private Image MeterImg;// �c�莞�Ԃ������摜
    [SerializeField] private Button FinishTurnButton;// ���Ԃ��c���Ă��Ă��^�[�����I����{�^��
    [SerializeField] private Button ShowResultButton;// ���ʂ�\������{�^��
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
            this.TurnText.text = TurnCount.ToString();//���^�[���ڂ��\��
        }
        if (this.TurnCount > 0 && this.MeterImg != null && !IsShowingResults && MiddleOfTurn)
        {
            MeterImg.fillAmount -= InitFillAmount / this.TurnDuration * Time.deltaTime;
        }
        if (TurnFlag && MiddleOfTurn)// �����̃^�[���Ȃ�u���b�N�𑗐M����
        {
            // 0.5�b���Ƀu���b�N�𑗐M����
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.5f)
            {
                elapsedTime = 0f;
                SendMyBlock();
            }
        }
        if (MiddleOfTurn && MeterImg.fillAmount == 0)// �^�C���A�E�g
        {
            OnTurnEnds();
        }
    }

    [PunRPC]
    public void OnTurnBegins()// �^�[���J�n��
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
    public void OnTurnEnds()// �^�[���I����
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

    // �u���b�N���s���A�Ō��JavaScript����Ăяo�����֐�����RpcTarget.AllViaServer�ŌĂ΂��
    [PunRPC]
    public void StartTurn()// �^�[�����J�n����
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

    // �Q�[���J�n�����A�}�X�^�[�̂ݎ��s
    public void GameStart()
    {
        // ���[�����̃����o�[�S��������������Ԃ̂Ƃ��̂݉�����悤�ɂ���
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
            CreateCoin();
            photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    public void RPCGameStart()
    {
        Debug.Log("�Q�[���J�n");
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
        Debug.Log("�Q�[���I��");
        PhotonLogin.instance.Finished();
        ShowResultButton.transform.localScale = Vector3.one;
    }

    // turn�������̃`�[���̃^�[�����m�F���郁�\�b�h
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

    // next�^�[����̃^�[���������̃^�[�����m�F���郁�\�b�h
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

    // �����̃`�[������U���m�F���Ē萔���擾����
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
        // ���̃^�[���Ƀh���C�o�[�ɂȂ�v���C���[�ɂ̓u���b�N�����L���Ȃ�
        if (!NextTurnFlag)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Debug.Log("�u���b�N���Z�b�g");
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
        Debug.Log("���s");
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

    // �u���b�N�̕ҏW���~�߁A�^�[�����I���郁�\�b�h
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
