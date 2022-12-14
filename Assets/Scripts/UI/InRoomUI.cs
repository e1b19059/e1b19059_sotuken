using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InRoomUI : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // 非表示
        transform.localScale = Vector3.zero;
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

}
