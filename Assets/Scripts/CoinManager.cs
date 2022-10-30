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
            if(collision.gameObject.name.Substring(collision.gameObject.name.Length - 8, 1) == "A")// (Clone)�𔲂��������̕�����؂�o��
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
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Player"))
        {
            //�I�u�W�F�N�g�̐F��ԂɕύX����
            GetComponent<Renderer>().material.color = Color.red;
        }*/
        Debug.Log("�Փ�");
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.gameObject.name.Substring(other.gameObject.name.Length - 8, 1) == "A")// (Clone)�𔲂��������̕�����؂�o��
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
