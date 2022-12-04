using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CoinManager : MonoBehaviourPunCallbacks
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.gameObject.name.Substring(0, 1) == "A")// �擪�̕������r
            {
                PhotonNetwork.CurrentRoom.SetScoreA(PhotonNetwork.CurrentRoom.GetScoreA() + 1);
            }
            else
            {
                PhotonNetwork.CurrentRoom.SetScoreB(PhotonNetwork.CurrentRoom.GetScoreB() + 1);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
