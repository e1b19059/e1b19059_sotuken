using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObstacleManager : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void RPCDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
