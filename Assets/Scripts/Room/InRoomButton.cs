using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;

public class InRoomButton : MonoBehaviourPunCallbacks
{
    [DllImport("__Internal")]
    private static extern string getBlockFromWorkspace();

    void Start()
    {
        // 非表示
        transform.localScale = Vector3.zero;
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        // ルームへの参加が成功したら、UIを表示する
        transform.localScale = Vector3.one;
    }

    public override void OnLeftRoom()
    {
        // 非表示
        transform.localScale = Vector3.zero;
    }

    public void SendMyBlock()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        PhotonNetwork.CurrentRoom.SetBlock(getBlockFromWorkspace());
#endif
    }

}
