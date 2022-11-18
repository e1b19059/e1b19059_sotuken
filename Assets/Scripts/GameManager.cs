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
    [SerializeField] private TextMeshProUGUI TurnText;//�^�[�����̕\���e�L�X�g
    [SerializeField] private Image MeterImg;// �c�莞�Ԃ������摜
    [SerializeField] private Button FinishTurnButton;// ���Ԃ��c���Ă��Ă��^�[�����I����{�^��
    [SerializeField] private Button ShowResultButton;// ���ʂ�\������{�^��
    [SerializeField] private TextMeshProUGUI PhaseUI;//�t�F�[�Y�̕\���e�L�X�g
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
            this.TurnText.text = TurnCount.ToString();//���^�[���ڂ��\��
        }
        if (this.TurnCount > 0 && this.MeterImg != null && !IsShowingResults && !IsFinished)
        {
            MeterImg.fillAmount -= InitFillAmount / this.TurnDuration * Time.deltaTime;
        }
        if (IsShare)// �h���C�o�[�Ȃ�u���b�N�𑗐M����
        {
            // 0.5�b���Ƀu���b�N�𑗐M����
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
        if (!IsFinished && MeterImg.fillAmount == 0)// �^�C���A�E�g
        {
            Debug.Log("�^�C���A�E�g");
            FinishPhase();
        }
    }

    // �t�F�[�Y�̕ω��ɔ����������s�����\�b�h�ARpcTarget.AllViaServer�ŌĂ΂��
    [PunRPC]
    public void RPCPhaseChange(int _phase)
    {
        phase = _phase;
        // phase��1,2,3�̓^�C���A�E�g����RPC�A4,5�̓u���b�N�̖����Ői�s������悤�ɂȂ��Ă���
        switch (_phase)
        {
            case 1:
                // ��U��U�ɂ�����炸�����̃`�[�����v���O���~���O���s���B����`�[���ւ̃u���b�N���L�͂Ȃ�
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
                // ��U�`�[�����v���O���~���O���s���B�u���b�N���L�͂���(�o����)
                // �����̃`�[������U���m�F���āA���[�N�X�y�[�X�̐؂�ւ�
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
                // ��U�`�[�����v���O���~���O���s���B�u���b�N���L�͂���(�o����)
                // �����̃`�[������U���m�F���āA���[�N�X�y�[�X�̐؂�ւ�
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
                // ��U�`�[���̃u���b�N�����s
                // �����̃`�[���̃��[�N�X�y�[�X��ǂݍ��ݐ�p�ɂ��āA��U���h���C�o�[�Ȃ�u���b�N�����s
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
                // ��U�`�[���̃u���b�N�����s
                // ��U���h���C�o�[�Ȃ�u���b�N�����s
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

    // �^�C�}�[���J�n���郁�\�b�h
    public void StartTimer()
    {
        MeterImg.fillAmount = InitFillAmount;// �^�C�}�[������
        IsFinished = false;// �^�C�}�[�J�n
    }

    // �t�F�[�Y��i�s�����郁�\�b�h
    public void PhaseProgress(int _phase)
    {
        // phase��5�ȏ�Ȃ�^�[�����I����,�����łȂ��Ȃ�phase+1
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
        IsFinished = true;// �^�C�}�[���~�߂�
        if (PhotonNetwork.IsMasterClient) PhaseProgress(phase);
    }

    // �^�[�����J�n���郁�\�b�h�ARpcTarget.AllViaServer�ŌĂ΂��
    [PunRPC]
    public void StartTurn()// �^�[�����J�n����
    {
        Debug.Log(++TurnCount + "�^�[���ڊJ�n");// �^�[�����Z
        if (PhotonNetwork.IsMasterClient)
        {
            createField.CreateCoin();
            PhaseProgress(0);
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
                player.SetOrder(Anum++);
            }
            else
            {
                player.SetOrder(Bnum++);
            }
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;// �r�������ł��Ȃ�����
        PhotonNetwork.CurrentRoom.SetInit(Anum, Bnum);
        photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void RPCGameStart()
    {
        Debug.Log("�Q�[���J�n");
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
        Debug.Log("�Q�[���I��");
        photonLogin.Finished();
        ShowResultButton.transform.localScale = Vector3.one;
    }

    // ���̃^�[���������h���C�o�[���m�F���郁�\�b�h
    // �����v���C���[��2�^�[�������ăh���C�o�[�ɂȂ�
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

    // �u���b�N�𑗐M���郁�\�b�h
    public void SendMyBlock()
    {
        photonView.RPC(nameof(SetOthersBlock), RpcTarget.Others, getBlockFromWorkspace());
    }

    // �u���b�N���Z�b�g���郁�\�b�h
    [PunRPC]
    public void SetOthersBlock(string block, PhotonMessageInfo info)
    {
        // �i�r�Q�[�^�[�Ȃ�A�G�����𔻕ʂ��ău���b�N���Z�b�g����
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
        Debug.Log("���s");
        GameObject obj = GameObject.FindGameObjectWithTag($"Player{scoreBoard.GetMyTeam()}");
        PhotonView photonView = obj.GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        setPlayerCharacter(obj.name);
        doCode();
    }

    // �^�[���I�����郁�\�b�h�ARpcTarget.AllViaServer�ŌĂ΂��
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

    // �t�F�[�Y���I��邩�e�X�g����
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
        targetPos.y = 0;// �v���C���[�L�����N�^�[��y���W�͑����ɂ��邽�ߑ��̃I�u�W�F�N�g�ɍ��킹��
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
        targetPos.y = 0;// �v���C���[�L�����N�^�[��y���W�͑����ɂ��邽�ߑ��̃I�u�W�F�N�g�ɍ��킹��
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
