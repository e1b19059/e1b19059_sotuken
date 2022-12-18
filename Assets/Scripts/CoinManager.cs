using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
            if (other.gameObject.name.Substring(0, 1) == "A")// êÊì™ÇÃï∂éöÇî‰är
            {
            //PhotonNetwork.CurrentRoom.SetScoreA(PhotonNetwork.CurrentRoom.GetScoreA() + 1);
            PlayerPrefs.SetInt("ScoreA", PlayerPrefs.GetInt("ScoreA") + 1);
            }
            else
            {
            //PhotonNetwork.CurrentRoom.SetScoreB(PhotonNetwork.CurrentRoom.GetScoreB() + 1);
            PlayerPrefs.SetInt("ScoreB", PlayerPrefs.GetInt("ScoreB") + 1);
        }
            //PhotonNetwork.Destroy(gameObject);
            Destroy(gameObject);
        //}
    }

}
