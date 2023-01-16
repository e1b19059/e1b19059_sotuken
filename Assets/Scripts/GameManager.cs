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
    [SerializeField] Slider timer;// �^�C���o�[
    [SerializeField] Button FinishTurnButton;// ���Ԃ��c���Ă��Ă��^�[�����I����{�^��
    [SerializeField] Button ShowResultButton;// ���ʂ�\������{�^��
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
            //�o�ߎ��Ԃ���ړ��ʂ̌v�Z
            float amount = Time.deltaTime / TurnDuration;

            //�X���C�_�[�̈ړ��ʂ���
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
        if (IsShare)// �h���C�o�[�Ȃ�u���b�N�𑗐M����
        {
            // 0.5�b���Ƀu���b�N�𑗐M����
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 1f)
            {
                elapsedTime = 0f;
                SendMyBlock();
            }
        }
        if (codeFinPlayer >= playerCnt)// �S�v���C���[���u���b�N�����s���I����
        {
            codeFinPlayer = 0;
            photonView.RPC(nameof(FinishPhase), RpcTarget.AllViaServer);
        }
        if ((counterFlagA || counterFlagB) && timer.value == 0)// �^�C���A�E�g
        {
            FinishPhase();
        }
    }

    // �t�F�[�Y�̕ω��ɔ����������s�����\�b�h�ARpcTarget.AllViaServer�ŌĂ΂��
    [PunRPC]
    public void RPCPhaseChange(int _phase)
    {
        phase = _phase;
        phaseUIManager.SetHighLight(_phase);
        switch (_phase)
        {
            case 1:
                // ���[�N�X�y�[�X�A�����̃Z�b�g�ƃg���b�v�̐���
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
                // ��U�`�[�����v���O���~���O���s��
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
                // ��U�`�[�����v���O���~���O���s��
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
                // ��U�`�[���̃u���b�N�����s�����
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
                // ��U�`�[���̃u���b�N�����s�����
                IsFirst = !IsFirst;
                DoCode();
                break;
            default:
                break;
        }
    }

    // �^�C�}�[���J�n���郁�\�b�h
    public void StartTimer(bool _flag)
    {
        timer.value = 1.0f;// �^�C�}�[������

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
            photonView.RPC(nameof(RPCPhaseChange), RpcTarget.AllViaServer, _phase + 1);
        }
    }

    [PunRPC]
    public void FinishPhase()
    {
        StopTimer();
        if (PhotonNetwork.IsMasterClient) PhaseProgress(phase);
    }

    // RpcTarget.MasterClient�ŌĂ΂��
    [PunRPC]
    public void RPCTellFinished()
    {
        codeFinPlayer++;
    }

    // �^�[�����J�n���郁�\�b�h�ARpcTarget.AllViaServer�ŌĂ΂��
    [PunRPC]
    public void StartTurn()// �^�[�����J�n����
    {
        Debug.Log(++TurnCount + "�^�[���ڊJ�n");// �^�[�����Z
        phaseUIManager.Init(scoreBoard.GetMyTeam(), IsFirst);
        if (PhotonNetwork.IsMasterClient)
        {
            var point = createField.CreatePoint();
            createField.photonView.RPC(nameof(createField.RPCCreateCoin), RpcTarget.AllViaServer, new Vector3(point.x, 0, point.z));
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
        photonView.RPC(nameof(InitMode), RpcTarget.All);// ���[�h�̏�����
        if (Anum == 1) photonView.RPC(nameof(OnSoloModeA), RpcTarget.All);
        if (Bnum == 1) photonView.RPC(nameof(OnSoloModeB), RpcTarget.All);
        PhotonNetwork.CurrentRoom.IsOpen = false;// �r�������ł��Ȃ�����
        PhotonNetwork.CurrentRoom.SetInit(Anum, Bnum);
        photonView.RPC(nameof(RPCGameStart), RpcTarget.AllViaServer, Anum + Bnum);
    }

    [PunRPC]
    public void RPCGameStart(int _playerCnt)
    {
        Debug.Log("�Q�[���J�n");
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
        Debug.Log("�Q�[���I��");
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

    public void SendAndDoCode()
    {
        photonView.RPC(nameof(SetBlockAndDoCode), RpcTarget.Others, getBlockFromWorkspace());
    }

    // �u���b�N���Z�b�g���郁�\�b�h
    [PunRPC]
    public void SetOthersBlock(string block, PhotonMessageInfo info)
    {
        // �i�r�Q�[�^�[�Ȃ�A�G�����𔻕ʂ��ău���b�N���Z�b�g����
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
        bool _IsMyTurn = !IsFirst;// ��Ƀv���O���~���O����������Ɏ��s����
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

    // �^�[���I�����郁�\�b�h
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

    // �g���b�v�I�u�W�F�N�g��j�󂷂郁�\�b�h�ARpcTarget.AllViaServer�ŌĂ΂��
    [PunRPC]
    public void RPCTrapDestroy()
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (var trap in traps)
        {
            Destroy(trap);
        }
    }

    // �u���b�N�̕ҏW���I����
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
