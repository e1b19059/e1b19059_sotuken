using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InRoomButton : MonoBehaviourPunCallbacks
{

    void Start()
    {
        // ��\��
        transform.localScale = Vector3.zero;
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        // ���[���ւ̎Q��������������AUI��\������
        transform.localScale = Vector3.one;
    }

    public override void OnLeftRoom()
    {
        // ��\��
        transform.localScale = Vector3.zero;
    }

}
