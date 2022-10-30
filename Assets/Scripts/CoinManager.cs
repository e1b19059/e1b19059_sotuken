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

    /*void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(collision.gameObject.name.Substring(collision.gameObject.name.Length - 8, 1) == "A")// (Clone)を抜いた末尾の文字を切り出す
            {
                PhotonNetwork.CurrentRoom.SetScoreA(PhotonNetwork.CurrentRoom.GetScoreA() + 1);
            }
            else
            {
                PhotonNetwork.CurrentRoom.SetScoreB(PhotonNetwork.CurrentRoom.GetScoreB() + 1);
            }
        }
        photonView.RPC(nameof(RPCDestroy), RpcTarget.MasterClient);
    }*/

    void OnTriggerEnter(Collider other)
    {
        /*
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Player"))
        {
            //オブジェクトの色を赤に変更する
            GetComponent<Renderer>().material.color = Color.red;
        }*/
        Debug.Log("衝突");
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.gameObject.name.Substring(other.gameObject.name.Length - 8, 1) == "A")// (Clone)を抜いた末尾の文字を切り出す
            {
                PhotonNetwork.CurrentRoom.SetScoreA(PhotonNetwork.CurrentRoom.GetScoreA() + 1);
            }
            else
            {
                PhotonNetwork.CurrentRoom.SetScoreB(PhotonNetwork.CurrentRoom.GetScoreB() + 1);
            }
            PhotonNetwork.Destroy(gameObject);
        }
        //photonView.RPC(nameof(RPCDestroy), RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RPCDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}
