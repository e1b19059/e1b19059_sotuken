using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Runtime.InteropServices;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [DllImport("__Internal")]
    private static extern void doCode();

    [DllImport("__Internal")]
    private static extern void setTargetObject(string str);

    [DllImport("__Internal")]
    private static extern string getBlockFromWorkspace();
    
    [DllImport("__Internal")]
    private static extern void setBlockToWorkspace(string block);

    public GameObject obstacle;

    private void Start()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";

        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);

        PhotonNetwork.JoinLobby();
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var position = new Vector3(0, 0, 0);
            PhotonNetwork.InstantiateRoomObject("Player", position, Quaternion.identity);

            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(-1f, 0, 1f), Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(1f, 0, 1f), Quaternion.identity);
            PhotonNetwork.InstantiateRoomObject(obstacle.name, new Vector3(1f, 0, 2f), Quaternion.identity);
        }
    }

    public void DoCode()
    {
        Debug.Log("���s");
        GameObject obj =  GameObject.FindGameObjectWithTag("Player");
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

    public void SendMyBlock()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        photonView.RPC(nameof(SetOthersBlock), RpcTarget.Others, getBlockFromWorkspace());
#endif
    }

    [PunRPC]
    public void SetOthersBlock(string block)
    {
        Debug.Log("setOthersBlock���s");
#if !UNITY_EDITOR && UNITY_WEBGL
        setBlockToWorkspace(block);
#endif
    }

    private void Update()
    { 
        if (PhotonNetwork.CurrentRoom != null)//���[�����ł���
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            PhotonView photonView = obj.GetComponent<PhotonView>();
            if (photonView.IsMine)
            {
                SendMyBlock();
            }
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
    
}